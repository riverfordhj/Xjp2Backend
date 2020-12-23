using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.Authentication.JWT;
using Models.Authentication.JWT.AuthHelper;
using ModelsBuildingEconomy.buildingCompany;
using Newtonsoft.Json;
using Xjp2Backend.Helper;

namespace Xjp2Backend
{
    public class Startup
    {
        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(SecretKey));

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //#region Jwt����
            ////��appsettings.json�е�JwtSettings�����ļ���ȡ��JwtSettings�У����Ǹ������ط��õ�
            //services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

            ////���ڳ�ʼ����ʱ�����Ǿ���Ҫ�ã�����ʹ��Bind�ķ�ʽ��ȡ����
            ////�����ð󶨵�JwtSettingsʵ����
            //var jwtSettings = new JwtSettings();
            //Configuration.Bind("JwtSettings", jwtSettings);

            ////��������֤
            //services.AddAuthentication(options =>
            //{
            //    //��֤middleware����
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(o =>
            //{
            //    //jwt token��������
            //    o.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        NameClaimType = JwtClaimTypes.Name,
            //        RoleClaimType = JwtClaimTypes.Role,
            //        //Token�䷢����
            //        ValidIssuer = jwtSettings.Issuer,
            //        //�䷢��˭
            //        ValidAudience = jwtSettings.Audience,
            //        //�����keyҪ���м���
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

            //        /***********************************TokenValidationParameters�Ĳ���Ĭ��ֵ***********************************/
            //        // RequireSignedTokens = true,
            //        // SaveSigninToken = false,
            //        // ValidateActor = false,
            //        // ������������������Ϊfalse�����Բ���֤Issuer��Audience�����ǲ�������������
            //        // ValidateAudience = true,
            //        // ValidateIssuer = true, 
            //        // ValidateIssuerSigningKey = false,
            //        // �Ƿ�Ҫ��Token��Claims�б������Expires
            //        // RequireExpirationTime = true,
            //        // ����ķ�����ʱ��ƫ����
            //        // ClockSkew = TimeSpan.FromSeconds(300),
            //        // �Ƿ���֤Token��Ч�ڣ�ʹ�õ�ǰʱ����Token��Claims�е�NotBefore��Expires�Ա�
            //        // ValidateLifetime = true
            //    };
            //});

            ////services.AddAuthorization()

            //#endregion
            services.AddControllers().AddNewtonsoftJson(option =>
                //����ѭ������
                option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            );
            //�������ݿ�context
            //services.AddDbContext<XjpContext>(opt => opt.UseInMemoryDatabase("XjpDB"));
            services.AddDbContext<StreetContext>(options => options.UseSqlServer(Configuration.GetConnectionString("XjpDatabase")));
            services.AddDbContext<xjpCompanyContext>(options => options.UseSqlServer(Configuration.GetConnectionString("XjpBuildingEcoDatabase")));

            //��ȡ��������ݿ������ַ��������ֵ�ȫ�־�̬������
            SystemParameterHelper.XjpDBConnectionString = Configuration.GetConnectionString("XjpDatabase");

            #region
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IBlogService, BlogService>();

            services.AddSingleton<IJwtFactory, JwtFactory>();

            services.AddMemoryCache();

            #region Jwt token Authentication
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],
                    ValidateAudience = true,
                    ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _signingKey,
                    RequireExpirationTime = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                configureOptions.SaveToken = true;
            });

            #region Authorization
            services.AddAuthorization(options =>
            {
                //options.AddPolicy("Administrator", policy => policy.RequireRole("administrator"));
                options.AddPolicy("Administrator", policy => policy.RequireClaim(ClaimTypes.Role, "administrator"));
                options.AddPolicy("APIAccess", policy => policy.RequireClaim(ClaimTypes.Role, "api_access"));

                options.AddPolicy("Permission", policy => policy.Requirements.Add(new PermissionRequirement()));
            });
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton<IAuthorizationHandler, ResourceAuthorizationHandler>();
            #endregion
            #endregion

            #endregion



            services.AddControllers();
            //mvc·������
            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(new ActionFilter());
            //}).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();//�����֤
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseMvc();
        }
    }
}
