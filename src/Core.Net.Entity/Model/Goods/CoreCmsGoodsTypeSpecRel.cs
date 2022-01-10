using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model.Goods
{
    /// <summary>
    /// 商品类型和属性关联表
    /// </summary>
    [SugarTable("CoreCmsGoodsTypeSpecRel", TableDescription = "商品类型和属性关联表")]
    public partial class CoreCmsGoodsTypeSpecRel
    {
        /// <summary>
        /// 商品类型和属性关联表
        /// </summary>
        public CoreCmsGoodsTypeSpecRel()
        {
        }

        /// <summary>
        /// 属性ID
        /// </summary>
        [Display(Name = "属性ID")]
        [SugarColumn(ColumnDescription = "属性ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 specId { get; set; }
        /// <summary>
        /// 类型ID
        /// </summary>
        [Display(Name = "类型ID")]
        [SugarColumn(ColumnDescription = "类型ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 typeId { get; set; }
    }
}
