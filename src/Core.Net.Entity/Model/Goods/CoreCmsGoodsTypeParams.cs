using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model.Goods
{
    /// <summary>
    /// 商品参数类型关系表
    /// </summary>
    [SugarTable("CoreCmsGoodsTypeParams", TableDescription = "商品参数类型关系表")]
    public partial class CoreCmsGoodsTypeParams
    {
        /// <summary>
        /// 商品参数类型关系表
        /// </summary>
        public CoreCmsGoodsTypeParams()
        {
        }

        /// <summary>
        /// 商品参数id
        /// </summary>
        [Display(Name = "商品参数id")]
        [SugarColumn(ColumnDescription = "商品参数id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 paramsId { get; set; }
        /// <summary>
        /// 商品类型id
        /// </summary>
        [Display(Name = "商品类型id")]
        [SugarColumn(ColumnDescription = "商品类型id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 typeId { get; set; }
    }
}
