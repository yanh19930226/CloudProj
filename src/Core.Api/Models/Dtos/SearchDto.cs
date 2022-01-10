using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models.Dtos
{
    /// <summary>
    /// 搜索模型
    /// </summary>
    public class SearchDto:FMPage
    {
        /// <summary>
        /// 关键字搜索
        /// </summary>
        public string Text { get; set; }
    }
}
