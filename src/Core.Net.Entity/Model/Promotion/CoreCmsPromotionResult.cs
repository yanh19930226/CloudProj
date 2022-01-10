using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model.Promotion
{
    /// <summary>
    /// 促销结果表
    /// </summary>
    [SugarTable("CoreCmsPromotionResult", TableDescription = "促销结果表")]
    public partial class CoreCmsPromotionResult
    {
        /// <summary>
        /// 促销结果表
        /// </summary>
        public CoreCmsPromotionResult()
        {
        }

        /// <summary>
        /// 序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(ColumnDescription = "序列", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 促销ID
        /// </summary>
        [Display(Name = "促销ID")]
        [SugarColumn(ColumnDescription = "促销ID", IsNullable = true)]
        public System.Int32? promotionId { get; set; }
        /// <summary>
        /// 促销条件编码
        /// </summary>
        [Display(Name = "促销条件编码")]
        [SugarColumn(ColumnDescription = "促销条件编码", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String code { get; set; }
        /// <summary>
        /// 支付配置参数序列号存储
        /// </summary>
        [Display(Name = "支付配置参数序列号存储")]
        [SugarColumn(ColumnDescription = "支付配置参数序列号存储", IsNullable = true)]
        public System.String parameters { get; set; }
    }
}
