using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model.DinnerCard
{
    /// <summary>
    /// 商品表
    /// </summary>
    [SugarTable("CoreGoods", TableDescription = "商品表")]
    public class CoreGoods
    {
        /// <summary>
        /// 商品表
        /// </summary>
        public CoreGoods()
        {
        }
        /// <summary>
        /// 商品ID
        /// </summary>
        [Display(Name = "商品ID")]
        [SugarColumn(ColumnDescription = "商品ID", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 商家Id
        /// </summary>
        [Display(Name = "商家Id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int businessId { get; set; }
        /// <summary>
        /// 商家名称
        /// </summary>
        [Display(Name = "商家名称")]
        public string businessName { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        [Display(Name = "商品编码")]
        [SugarColumn(ColumnDescription = "商品编码")]
        //[Required(ErrorMessage = "请输入{0}")]
        public string goodNo { get; set; }
        /// <summary>
        ///餐卡号
        /// </summary>
        [Display(Name = "商品名称")]
        [SugarColumn(ColumnDescription = "商品名称", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String goodName { get; set; }
        /// <summary>
        ///图片
        /// </summary>
        [Display(Name = "图片")]
        [SugarColumn(ColumnDescription = "图片")]
        [Required(ErrorMessage = "请输入{0}")]
        public string url { get; set; }
        /// <summary>
        ///库存
        /// </summary>
        [Display(Name = "库存")]
        [SugarColumn(ColumnDescription = "库存")]
        [Required(ErrorMessage = "请输入{0}")]
        public int stock { get; set; }
        /// <summary>
        ///订购数量
        /// </summary>
        [Display(Name = "订购数量")]
        [SugarColumn(ColumnDescription = "订购数量")]
        [Required(ErrorMessage = "请输入{0}")]
        public int purchaseNumber { get; set; }
        /// <summary>
        ///是否上架
        /// </summary>
        [Display(Name = "是否上架")]
        [SugarColumn(ColumnDescription = "是否上架")]
        [Required(ErrorMessage = "请输入{0}")]
        public int status { get; set; }
        /// <summary>
        ///单价
        /// </summary>
        [Display(Name = "单价")]
        [SugarColumn(ColumnDescription = "单价")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal unitPrice { get; set; }
        /// <summary>
        ///有效时间
        /// </summary>
        [Display(Name = "有效时间")]
        [SugarColumn(ColumnDescription = "有效时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime avilableTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间", IsNullable = true)]
        public System.DateTime? createTime { get; set; }
    }
}
