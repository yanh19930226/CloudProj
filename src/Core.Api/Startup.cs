using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Core.Api.Models.Configs;
using Core.Net.AutoFac;
using Core.Net.Config;
using Core.Net.Configuration;
using Core.Net.Util.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Core.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }
        /// <summary>
        /// web环境
        /// </summary>
        public IWebHostEnvironment Env { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddControllers().AddNewtonsoftJson(option => {
                //数据格式首字母小写 不使用驼峰
                //option.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //数据库数据类型为String 值为Null 返回 空字符串
                option.SerializerSettings.ContractResolver = new NullToEmptyStringResolver();
                //忽略循环引用
                option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //时间格式
                option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            //添加本地路径获取支持
            services.AddSingleton(new AppSettingsHelper(Env.ContentRootPath));
          
            //内存缓存
            services.AddMemoryCacheSetup();
            //启用Redis
            services.AddRedisSetup();
            //Redis消息队列
            services.AddRedisMessageQueueSetup();
            //配置跨域（CORS）
            services.AddCorsSetup();
            //Swagger接口文档注入
            services.AddSwaggerSetup();



            #region Authentication

            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));

            var jwtsettings = services.BuildServiceProvider().GetService<IOptions<JwtConfig>>().Value;

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,//是否验证Issuer
                    ValidateAudience = true,//是否验证Audience
                    ValidateLifetime = true,//是否验证失效时间
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    ValidAudience = jwtsettings.Audience,//Audience
                    ValidIssuer = jwtsettings.Issuer,//Issuer，这两项和签发jwt的设置一致
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtsettings.SecretKey))//SecurityKey
                };

                o.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.Response.Headers.Add("Token-Error", context.ErrorDescription);
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var token = context.Request.Headers["Authorization"].ObjectToString().Replace("Bearer ", "");
                        var jwtToken = (new JwtSecurityTokenHandler()).ReadJwtToken(token);

                        if (jwtToken.Issuer != jwtsettings.Issuer)
                        {
                            context.Response.Headers.Add("Token-Error-Iss", "issuer is wrong!");
                        }

                        if (jwtToken.Audiences.FirstOrDefault() != jwtsettings.Audience)
                        {
                            context.Response.Headers.Add("Token-Error-Aud", "Audience is wrong!");
                        }

                        // 如果过期，则把<是否过期>添加到，返回头信息中
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            #endregion

        }
        /// <summary>
        /// Autofac
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {

            builder.RegisterModule(new AutofacModuleRegister());

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //Swagger接口文档注入
            app.UseCoreSwagger();
            // CORS跨域
            app.UseCors(AppSettingsConstVars.CorsPolicyName);

            app.UseRouting();
            //认证
            app.UseAuthentication();

            //授权
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
