using Core.Net.Configuration;
using Core.Net.RedisMq;
using InitQ;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Config
{
    public static class RedisMessageQueueSetup
    {
        public static IServiceCollection AddRedisMessageQueueSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (AppSettingsConstVars.RedisConfigUseRedisMessageQueue)
            {
                services.AddInitQ(m =>
                {
                    //时间间隔
                    m.SuspendTime = 1000;
                    //redis服务器地址
                    m.ConnectionString = AppSettingsConstVars.RedisConfigConnectionString;
                    //对应的订阅者类，需要new一个实例对象，当然你也可以传参，比如日志对象
                    m.ListSubscribe = new List<Type>() {
                        //typeof(OrderSubscribe),
                        //typeof(OrderPrintSubscribe),
                        typeof(LogingSubscribe),
                    };
                    //显示日志
                    m.ShowLog = false;
                });
            }
            return services;
        }
    }
}
