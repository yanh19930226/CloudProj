using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models.Dtos
{
    /// <summary>
    /// 商品搜索
    /// </summary>
    public class GoodSearchDto:FMPage
    {
        /// <summary>
        /// 商品信息
        /// </summary>
        public string goodText { get; set; }
        /// <summary>
        /// 商家Id
        /// </summary>
        public string businessId { get; set; }
    }
}
