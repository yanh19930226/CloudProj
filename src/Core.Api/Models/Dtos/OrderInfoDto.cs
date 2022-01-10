using Core.Net.Entity.Model.DinnerCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models.Dtos
{
    /// <summary>
    /// 我的订单
    /// </summary>
    public class OrderInfoDto
    {
        /// <summary>
        /// 订单
        /// </summary>
        public CoreOrder order { get; set; }
        /// <summary>
        /// 订单详细
        /// </summary>
        public List<CoreGoodOrderDetail> orderDeatilList { get; set; } = new List<CoreGoodOrderDetail>();
    }
}
