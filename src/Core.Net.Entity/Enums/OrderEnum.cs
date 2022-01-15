using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Entity.Enums
{
    /// <summary>
    /// 订单类型
    /// </summary>
    public enum OrderTypeEnum
    {
        /// <summary>
        /// 刷卡扣费
        /// </summary>
        CardCost = 0,
         /// <summary>
         /// 现金充值
         /// </summary>
         MoneyRecharge=1,
        /// <summary>
        /// 余额查询
        /// </summary>
        BalanceSearch = 2,
        /// <summary>
        /// 钱包转账
        /// </summary>
         Wallet=3,
         /// <summary>
         /// 商品订单
         /// </summary>
         GoodOrder=4,
         /// <summary>
         /// 公司福利
         /// </summary>
         Company = 5,
    }
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderStatusEnum
    {
        /// <summary>
        /// 未付款
        /// </summary>
        UnPayed=0,
        /// <summary>
        /// 已取消
        /// </summary>
        Canceled=1,
        /// <summary>
        /// 已付款
        /// </summary>
        Payed=2,
        /// <summary>
        /// 已退款
        /// </summary>
        Return=3,
        /// <summary>
        /// 已发货
        /// </summary>
        Deliver=4,
        /// <summary>
        /// 完成
        /// </summary>
        Complete = 5
        ///// <summary>
        ///// 完成
        ///// </summary>
        //Complete = 6
    }

    /// <summary>
    /// 支付类型
    /// </summary>
    public enum PayTypeEnum
    {
        /// <summary>
        /// 刷卡
        /// </summary>
        Card = 6,
        /// <summary>
        /// 其他
        /// </summary>
        Other = 7
    }
}
