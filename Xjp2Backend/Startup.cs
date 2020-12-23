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
            //#region Jwt配置
            ////将appsettings.json中的JwtSettings部分文件读取到JwtSettings中，这是给其他地方用的
            //services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

            ////由于初始化的时候我们就需要用，所以使用Bind的方式读取配置
            ////将配置绑定到JwtSettings实例中
            //var jwtSettings = new JwtSettings();
            //Configuration.Bind("JwtSettings", jwtSettings);

            ////添加身份验证
            //services.AddAuthentication(options =>
            //{
            //    //认证middleware配置
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(o =>
            //{
            //    //jwt token参数设置
            //    o.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        NameClaimType = JwtClaimTypes.Name,
            //        RoleClaimType = JwtClaimTypes.Role,
            //        //Token颁发机构
            //        ValidIssuer = jwtSettings.Issuer,
            //        //颁发给谁
            //        ValidAudience = jwtSettings.Audience,
            //        //这里的key要进行加密
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

            //        /***********************************TokenValidationParameters的参数默认值***********************************/
            //        // RequireSignedTokens = true,
            //        // SaveSigninToken = false,
            //        // ValidateActor = false,
            //        // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
            //        // ValidateAudience = true,
            //        // ValidateIssuer = true, 
            //        // ValidateIssuerSigningKey = false,
            //        // 是否要求Token的Claims中必须包含Expires
            //        // RequireExpirationTime = true,
            //        // 允许的服务器时间偏移量
            //        // ClockSkew = TimeSpan.FromSeconds(300),
            //        // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
            //        // ValidateLifetime = true
            //    };
            //});

            ////services.AddAuthorization()

            //#endregion
            services.AddControllers().AddNewtonsoftJson(option =>
                //忽略循环引用
                option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            );
            //配置数据库context
            //services.AddDbContext<XjpContext>(opt => opt.UseInMemoryDatabase("XjpDB"));
            services.AddDbContext<StreetContext>(options => options.UseSqlServer(Configuration.GetConnectionString("XjpDatabase")));
            services.AddDbContext<xjpCompanyContext>(options => options.UseSqlServer(Configuration.GetConnectionString("XjpBuildingEcoDatabase")));

            //读取徐家棚数据库连接字符串，保持到全局静态变量中
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
            //mvc路由配置
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

            app.UseAuthentication();//身份验证
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseMvc();
        }
    }
}
