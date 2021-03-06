using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models.Dtos
{
    /// <summary>
    /// 商品统计搜索
    /// </summary>
    public class BusinessGoodDto:FMPage
    {
        /// <summary>
        /// 商家名称
        /// </summary>
        public string businessId { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string orderStatus { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string startTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string endTime { get; set; }
    }
}
