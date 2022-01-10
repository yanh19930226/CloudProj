using Aliyun.Log.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Core.Net.Tests
{
    public class AliyunLogTest
    {
        private  AliyunLogClient _aliyunLogClient;
        public  IConfiguration Configuration { get; set; }
        private ServiceCollection services { get; set; }
        public ServiceProvider serviceProvider { get; set; }
        public AliyunLogTest()
        {
            Configuration = new ConfigurationBuilder().Build(); ;
            AliyunSLSOptions sls = new AliyunSLSOptions()
            {
                AccessKey = "",
                AccessKeyId = "",
                Endpoint = "cn-shenzhen.log.aliyuncs.com",
                LogStoreName = "yande",
                Project = "yande",
            };
            services = new ServiceCollection();
            Configuration.Bind(sls);
            services.AddAliyunLog(m =>
            {
                //m.AccessKey = Configuration.GetValue<string>("AccessKey");
                //m.AccessKeyId = Configuration.GetValue<string>("AccessKeyId");
                //m.Endpoint = Configuration.GetValue<string>("Endpoint");
                //m.Project = Configuration.GetValue<string>("Project");
                //m.LogStoreName = Configuration.GetValue<string>("LogstoreName");

                m.AccessKey = "";
                m.AccessKeyId = "aaa";
                m.Endpoint = "cn-shenzhen.log.aliyuncs.com";
                m.Project = "yande";
                m.LogStoreName = "yande";

            });

             serviceProvider = services.BuildServiceProvider();
             _aliyunLogClient = serviceProvider.GetRequiredService<AliyunLogClient>();
        }


        [Fact]
        public async Task WriteLog()
        {
            LogModel logModel = new LogModel()
            {
                ClassName = "Aliyun.log",
                Desc = "6666666666xxxxxx",
                Html = "99999999999xxxxx",
                Topic = "1",
                OrderNo = Guid.NewGuid().ToString("N"),
                PostDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
            await _aliyunLogClient.Log(logModel);
        }

        [Fact]
        public async Task ReadLog()
        {
           
        }
    }
}
