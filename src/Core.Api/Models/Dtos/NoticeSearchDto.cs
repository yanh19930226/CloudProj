using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models.Dtos
{
    /// <summary>
    /// 搜索参数
    /// </summary>
    public class NoticeSearchDto : FMPage
    {
        /// <summary>
        /// 公告
        /// </summary>
        public string noticeText { get; set; }
    }
}
