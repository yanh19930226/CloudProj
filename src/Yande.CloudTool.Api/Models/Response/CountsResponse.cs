using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yande.CloudTool.Api.Models.Response
{
    /// <summary>
    /// CountsResponse
    /// </summary>
    public class CountsResponse : BaseResponse
    {
        /// <summary>
        /// 自定义显示文本（Status为1时传入，屏幕显示此值，内容内使用\n换行，最多支持4行，每行不超过8个汉字）
        /// </summary>
        public string Text { get; set; }
    }
}
