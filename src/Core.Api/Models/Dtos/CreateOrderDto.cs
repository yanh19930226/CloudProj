using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models.Dtos
{
    /// <summary>
    /// 创建订单参数
    /// </summary>
    public class CreateOrderDto
    {
        /// <summary>
        /// 创建订单商品信息集合
        /// </summary>
        public List<GoodInfo> GoodInfos { get; set; }
    }

    /// <summary>
    /// 创建订单商品信息
    /// </summary>
    public class GoodInfo
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public int GoodId { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int Number { get; set; }
    }
}
