using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models.Dtos
{
    /// <summary>
    /// 创建订单
    /// </summary>
    public class CreateOrderDto
    {
        /// <summary>
        ///商品单价(冗余字段)
        /// </summary>
        public decimal? unitPrice { get; set; }
        /// <summary>
        /// 订单总价
        /// </summary>
        public decimal totalPrice { get; set; }

        #region 商品信息
        /// <summary>
        ///商品名称
        /// </summary>
        public System.String goodName { get; set; }
        /// <summary>
        ///商品编号
        /// </summary>
        public System.String goodNo { get; set; }
        /// <summary>
        ///商品图片
        /// </summary>
        public System.String goodUrl { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int? goodNumber { get; set; }

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
}
