using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model.Goods
{
    /// <summary>
    /// 商品类型属性表
    /// </summary>
    [SugarTable("CoreCmsGoodsTypeSpec", TableDescription = "商品类型属性表")]
    public partial class CoreCmsGoodsTypeSpec
    {
        /// <summary>
        /// 商品类型属性表
        /// </summary>
        public CoreCmsGoodsTypeSpec()
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
        /// 属性名称
        /// </summary>
        [Display(Name = "属性名称")]
        [SugarColumn(ColumnDescription = "属性名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 属性排序
        /// </summary>
        [Display(Name = "属性排序")]
        [SugarColumn(ColumnDescription = "属性排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sort { get; set; }

        /// <summary>
        ///     子类
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsGoodsTypeSpecValue> specValues { get; set; } = new List<CoreCmsGoodsTypeSpecValue>();
    }
    
}
