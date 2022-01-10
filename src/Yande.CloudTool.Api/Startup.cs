using Autofac;
using Core.Net.AutoFac;
using Core.Net.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Yande.CloudTool.Api
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
            #region 基础配置

            //添加本地路径获取支持
            services.AddSingleton(new AppSettingsHelper(Env.ContentRootPath));

            services.AddMvc(options =>
            {
                ////实体验证
                //options.Filters.Add<ModelValidateErrorFilter>();
                //异常处理
                //options.Filters.Add<GlobalExceptionsFilter>();
            });

            services.AddMvc().AddJsonOptions(options =>
            {
                // 忽略null数据
                options.JsonSerializerOptions.IgnoreNullValues = true;
                // 不设定 = 原样输出
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            services.AddConfig();

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            });
     
            #endregion

            #region 设置跨域
            services.AddCors(options => options.AddPolicy("CorsPolicy",
             builder =>
             {
                 builder.AllowAnyMethod()
                     .AllowAnyHeader()
                     .SetIsOriginAllowed(_ => true) // =AllowAnyOrigin()
                     .AllowCredentials();
             }));
            #endregion

            #region Swagger
            services.AddOpenApiDocument(settings =>
            {
                settings.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Yande.CloudCostTool.Api";
                    document.Info.Description = "Yande.CloudCostTool.Api";
                    document.Info.TermsOfService = "None";
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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #region Swagger
            app.UseOpenApi(); //添加swagger生成api文档（默认路由文档 /swagger/v1/swagger.json）
            app.UseSwaggerUi3();//添加Swagger UI到请求管道中(默认路由: /swagger). 
            #endregion

            //允许跨域请求
            app.UseCors("CorsPolicy");

            app.UseAuthorization();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
