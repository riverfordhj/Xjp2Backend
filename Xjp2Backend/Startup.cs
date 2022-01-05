using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ModelCompany;
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
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDirectoryBrowser();
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 52428800;
            });
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
            //services.AddDbContext<xjpCompanyContext>(options => options.UseSqlServer(Configuration.GetConnectionString("XjpBuildingEcoDatabase")));
            services.AddDbContext<CompanyContext>(options => options.UseSqlServer(Configuration.GetConnectionString("XjpCompanyInfoDatabase")));

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

            services.AddMvc();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Administrator"));
                options.AddPolicy("Grider", policy => policy.RequireClaim(ClaimTypes.Role, "����Ա"));
                options.AddPolicy("Community", policy => policy.RequireClaim(ClaimTypes.Role, "����"));
                options.AddPolicy("Audit", policy => policy.RequireClaim(ClaimTypes.Role, "Administrator","����"));
            });
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton<IAuthorizationHandler, ResourceAuthorizationHandler>();
            #endregion
            #endregion

            #endregion

            //��������
            #region 
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin()
                                             .WithMethods("PUT","DELETE","GET","POST")
                                             .AllowAnyHeader();
                                      //builder.WithOrigins("https://127.0.0.1:5502")
                                      //              .AllowCredentials()
                                      //              .AllowAnyMethod()
                                      //              .AllowAnyHeader();
                                  });
            });
            #endregion


            services.AddControllers();
            //mvc·������
            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(new ActionFilter());
            //}).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //services.AddControllers().AddNewtonsoftJson(option =>
            //    //����ѭ������
            //    option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //                    Path.Combine(env.ContentRootPath, "UploadFile")),
            //    RequestPath = "/UploadFile"
            //});
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                                 Path.Combine("F:\\houkunkun\\XJP", "UploadFile")),
                //Path.Combine(env.ContentRootPath.Substring(0, 3), "UploadFile")),
                //Path.Combine(env.ContentRootPath, "UploadFile")),
                RequestPath = "/UploadFile",
                EnableDirectoryBrowsing = true
            });
            // using Microsoft.Extensions.FileProviders;
            // using System.IO;


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);//����
           // app.UseResponseCaching();

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
