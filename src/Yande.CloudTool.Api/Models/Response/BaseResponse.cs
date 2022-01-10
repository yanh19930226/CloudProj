using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yande.CloudTool.Api.Models.Response
{
    /// <summary>
    /// BaseResponse
    /// </summary>
    public class BaseResponse
    {
        /// <summary>
        /// 服务器处理结果，1为成功，0为失败
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Msg
        /// </summary>
        public string Msg { get; set; }
    }
}
