using InitQ.Abstractions;
using InitQ.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Core.Net.Configuration.GlobalConstVars;

namespace Core.Net.RedisMq
{
    public class LogingSubscribe : IRedisSubscribe
    {

        [Subscribe(RedisMessageQueueKey.LogingQueue)]
        private async Task SubRedisOrder2(string msg)
        {
            Console.WriteLine($"消息队列:接口端订阅从队列{RedisMessageQueueKey.LogingQueue} 接受到 消息:{msg}");

            await Task.CompletedTask;
        }


        [Subscribe(RedisMessageQueueKey.SmsQueue)]
        private async Task SubSmsQueue1(string msg)
        {
            Console.WriteLine($"消息队列:接口端订阅从队列{RedisMessageQueueKey.SmsQueue} 接受到 消息:{msg}");

            await Task.CompletedTask;
        }

    }
}
