using Autofac;
using Core.Net.Auth;
using Core.Net.AutoFac;
using Core.Net.Config;
using Core.Net.Configuration;
using Core.Net.Filter;
using Core.Net.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Net.Web.Admin
{
    public class Startup
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }
        /// <summary>
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// </summary>
        public IWebHostEnvironment Env { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            //添加本地路径获取支持
            services.AddSingleton(new AppSettingsHelper(Env.ContentRootPath));
            //内存缓存
            services.AddMemoryCacheSetup();
            //启用Redis
            services.AddRedisSetup();
            //Redis消息队列
            services.AddRedisMessageQueueSetup();
            //jwt授权支持注入
            services.AddAuthorizationSetupForAdmin();
            //注册mvc，注册razor引擎视图
            services.AddMvc(options =>
            {
                //实体验证
                options.Filters.Add<RequiredErrorForAdmin>();
                ////异常处理
                options.Filters.Add<GlobalExceptionsFilterForAdmin>();
                ////Swagger剔除不需要加入api展示的列表
                //options.Conventions.Add(new ApiExplorerIgnores());
                //options.EnableEndpointRouting = false;
            }).AddNewtonsoftJson(p =>
                {
                    //数据格式首字母小写 不使用驼峰
                    //p.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    //不使用驼峰样式的key
                    //p.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //忽略循环引用
                    //p.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //设置时间格式
                    p.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                });
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
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            // 记录请求与返回数据 (注意开启权限，不然本地无法写入)
            app.UseReuestResponseLog();
            // 记录ip请求 (注意开启权限，不然本地无法写入)
            app.UseIpLogMildd();
            // 用户访问记录(必须放到外层，不然如果遇到异常，会报错，因为不能返回流)(注意开启权限，不然本地无法写入)
            //app.UseRecordAccessLogsMildd();
            // signalr
            //app.UseSignalRSendMildd();

            app.UseStaticFiles();
            app.UseRouting();
            // 先开启认证
            app.UseAuthentication();
            // 然后是授权中间件
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "areas",
                    "{area:exists}/{controller=Default}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });

            //设置默认起始页 index.html
            //此处的路径是相对于wwwroot文件夹的相对路径
            var defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(defaultFilesOptions);
            app.UseStaticFiles();
          
        }
    }
}
