using Core.Net.Entity.Model.DinnerCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models.Dtos
{
    /// <summary>
    /// 订单详细
    /// </summary>
    public class OrderDetailDto
    {
        /// <summary>
        /// 主订单
        /// </summary>
        public CoreOrderDto coreOrder { get; set; }
        /// <summary>
        /// 订单详细
        /// </summary>
        public List<CoreOrderDetailDto> coreOrderDetail { get; set; }
    }

    /// <summary>
    /// 订单信息
    /// </summary>
    public class CoreOrderDto
    {
        /// <summary>
        ///订单ID
        /// </summary>
        public System.Int32 id { get; set; }
        /// <summary>
        /// 机构Id
        /// </summary>
        public System.Int32 oraganizationId { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public System.Int32 roleId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public System.Int32 sysUserId { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string organizationName { get; set; }
        /// <summary>
        /// roleName
        /// </summary>
        public string roleName { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string orderNo { get; set; }
        /// <summary>
        /// 订单类型:刷卡扣费0,现金充值1,余额查询2,钱包转账,3商品订单4,公司福利5
        /// </summary>
        public int orderType { get; set; }
        /// <summary>
        /// 订单状态： 未付款0，已取消1，已付款2，已退款3，已发货4, 已完成5
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 订单总价
        /// </summary>
        public decimal totalPrice { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime createTime { get; set; }

        #region 付款信息
        /// <summary>
        /// 订单付款方式:刷卡扣费6,现金充值7
        /// </summary>
        public int payType { get; set; }
        #endregion

        #region 用户信息
        /// <summary>
        ///用户名
        /// </summary>
        public System.String userName { get; set; }
        /// <summary>
        ///用户手机号
        /// </summary>
        public string telePhone { get; set; }
        #endregion
    }

    /// <summary>
    /// 订单详细
    /// </summary>
    public class CoreOrderDetailDto
    {
        /// <summary>
        /// 商品订单详细Id
        /// </summary>
        public System.Int32 id { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string orderNo { get; set; }
        /// <summary>
        /// 商品Id
        /// </summary>
        public int goodId { get; set; }
        /// <summary>
        ///商品图片
        /// </summary>
        public System.String url { get; set; }
        /// <summary>
        ///商品名称
        /// </summary>
        public System.String goodName { get; set; }
        /// <summary>
        ///商品编号
        /// </summary>
        public System.String goodNo { get; set; }
        /// <summary>
        ///商品数量
        /// </summary>
        public int goodNum { get; set; }
        /// <summary>
        ///商品单价
        /// </summary>
        public decimal unitPrice { get; set; }
    }
}
