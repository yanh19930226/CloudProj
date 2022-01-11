using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models.Dtos
{
    public class BusinessSearchDto : FMPage
    {
        /// <summary>
        /// 公告
        /// </summary>
        public string businessText { get; set; }
    }
}
