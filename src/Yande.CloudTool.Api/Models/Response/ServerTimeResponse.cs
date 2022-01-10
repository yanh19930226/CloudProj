using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yande.CloudTool.Api.Models.Response
{
    public class ServerTimeResponse:BaseResponse
    {
        /// <summary>
        /// 服务器时间，固定格式为yyyyMMddHHmmssd最后一位d代表星期,星期
        /// </summary>
        public string Time { get; set; }
    }
}
