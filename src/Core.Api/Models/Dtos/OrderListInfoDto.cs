using Core.Net.Entity.Model.DinnerCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models.Dtos
{

    /// <summary>
    ///我的订单搜索
    /// </summary>
    public class OrderSearchDto:SearchDto
    {
    }

    public class OrderListInfoDto
    {
        /// <summary>
        /// 订单总笔数
        /// </summary>
        public int OrderCount { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal OrderTotalFeet { get; set; }
        /// <summary>
        /// 公司充值总金额
        /// </summary>
        public decimal CompanyTotalFee { get; set; } = 11M;
        /// <summary>
        /// 个人充值总金额
        /// </summary>
        public decimal PersonTotalFee { get; set; } = 11M;
        /// <summary>
        /// 订单列表
        /// </summary>
        public List<CoreOrder>orders{ get; set; }

}
}
