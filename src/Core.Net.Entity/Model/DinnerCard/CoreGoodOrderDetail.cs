using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model.DinnerCard
{
    /// <summary>
    /// 商品订单详细表
    /// </summary>
    [SugarTable("CoreGoodOrderDetail", TableDescription = "商品订单详细表")]
    public class CoreGoodOrderDetail
    {
        /// <summary>
        /// 商品订单详细表
        /// </summary>
        public CoreGoodOrderDetail()
        {
        }
        /// <summary>
        /// 商品订单详细Id
        /// </summary>
        [Display(Name = "商品订单详细Id")]
        [SugarColumn(ColumnDescription = "商品订单详细Id", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        [Display(Name = "订单Id")]
        [SugarColumn(ColumnDescription = "订单Id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int orderId { get; set; }
        /// <summary>
        /// 商品订单Id
        /// </summary>
        [Display(Name = "商品Id")]
        [SugarColumn(ColumnDescription = "商品Id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int goodId { get; set; }
        /// <summary>
        ///商品名称
        /// </summary>
        [Display(Name = "商品名称")]
        [SugarColumn(ColumnDescription = "商品名称", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String goodName { get; set; }
        /// <summary>
        ///商品编号
        /// </summary>
        [Display(Name = "商品编号")]
        [SugarColumn(ColumnDescription = "商品编号", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String goodNo { get; set; }
        /// <summary>
        ///商品数量
        /// </summary>
        [Display(Name = "商品数量")]
        [SugarColumn(ColumnDescription = "商品数量")]
        [Required(ErrorMessage = "请输入{0}")]
        public int goodNum { get; set; }
        /// <summary>
        ///商品单价(冗余字段)
        /// </summary>
        [Display(Name = "商品单价")]
        [SugarColumn(ColumnDescription = "商品单价")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal unitPrice { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间", IsNullable = true)]
        public System.DateTime? createTime { get; set; }
    }
}
