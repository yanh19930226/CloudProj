using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Yande.CloudTool.Api
{
    public static class SevicesExtension
    {
        public static IServiceCollection AddConfig(this IServiceCollection services)
        {
            services.AddSeriLog();

            return services;
        }

        public static IServiceCollection AddSeriLog(this IServiceCollection services)
        {
            services.AddSingleton(sp =>
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "log.txt");

                string logTemplete = "[{Timestamp:HH:mm:ss}][{Level}]{NewLine}Source:{SourceContext}{NewLine}Message:{Message}{NewLine}{Exception}{NewLine}";

                var LoggerConfiguration = new LoggerConfiguration();


                LoggerConfiguration = LoggerConfiguration
                                                   .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                                                   .MinimumLevel.Override("System", LogEventLevel.Information);

                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .WriteTo.File(path, LogEventLevel.Error, logTemplete,
                 rollingInterval: RollingInterval.Day,
                 rollOnFileSizeLimit: true)
                 .CreateLogger();
                return Log.Logger;
            });

            services.AddSingleton((Func<IServiceProvider, ILoggerFactory>)((IServiceProvider provider) => new Serilog.Extensions.Logging.SerilogLoggerFactory(provider.GetService<Serilog.ILogger>())));

            return services;
        }


        //public static IServiceCollection AddRedisSetup(this IServiceCollection services)
        //{
        //    if (services == null) throw new ArgumentNullException(nameof(services));
        //    //配置文件是否启用Redis
        //    if (AppSettingsConstVars.RedisConfigEnabled)
        //    {
        //        // 配置启动Redis服务，虽然可能影响项目启动速度，但是不能在运行的时候报错，所以是合理的
        //        services.AddSingleton<ConnectionMultiplexer>(sp =>
        //        {
        //            //获取连接字符串
        //            string redisConfiguration = AppSettingsConstVars.RedisConfigConnectionString;

        //            var configuration = ConfigurationOptions.Parse(redisConfiguration, true);

        //            configuration.ResolveDns = true;

        //            return ConnectionMultiplexer.Connect(configuration);
        //        });
        //        services.AddTransient<IRedisOperationRepository, RedisOperationRepository>();
        //    }

        //    return services;
        //}
    }
}
