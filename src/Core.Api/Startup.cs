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
        /// web����
        /// </summary>
        public IWebHostEnvironment Env { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddControllers().AddNewtonsoftJson(option => {
                //���ݸ�ʽ����ĸСд ��ʹ���շ�
                //option.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //���ݿ���������ΪString ֵΪNull ���� ���ַ���
                option.SerializerSettings.ContractResolver = new NullToEmptyStringResolver();
                //����ѭ������
                option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //ʱ���ʽ
                option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            //��ӱ���·����ȡ֧��
            services.AddSingleton(new AppSettingsHelper(Env.ContentRootPath));
          
            //�ڴ滺��
            services.AddMemoryCacheSetup();
            //����Redis
            services.AddRedisSetup();
            //Redis��Ϣ����
            services.AddRedisMessageQueueSetup();
            //���ÿ���CORS��
            services.AddCorsSetup();
            //Swagger�ӿ��ĵ�ע��
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
                    ValidateIssuer = true,//�Ƿ���֤Issuer
                    ValidateAudience = true,//�Ƿ���֤Audience
                    ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
                    ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
                    ValidAudience = jwtsettings.Audience,//Audience
                    ValidIssuer = jwtsettings.Issuer,//Issuer���������ǩ��jwt������һ��
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

                        // ������ڣ����<�Ƿ����>��ӵ�������ͷ��Ϣ��
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
            //Swagger�ӿ��ĵ�ע��
            app.UseCoreSwagger();
            // CORS����
            app.UseCors(AppSettingsConstVars.CorsPolicyName);

            app.UseRouting();
            //��֤
            app.UseAuthentication();

            //��Ȩ
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
