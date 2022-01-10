using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model.Goods
{
    /// <summary>
    /// 商品类型
    /// </summary>
    [SugarTable("CoreCmsGoodsType", TableDescription = "商品类型")]
    public partial class CoreCmsGoodsType
    {
        /// <summary>
        /// 商品类型
        /// </summary>
        public CoreCmsGoodsType()
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
        /// 类型名称
        /// </summary>
        [Display(Name = "类型名称")]
        [SugarColumn(ColumnDescription = "类型名称", IsNullable = true)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 参数序列号数组
        /// </summary>
        [Display(Name = "参数序列号数组")]
        [SugarColumn(ColumnDescription = "参数序列号数组", IsNullable = true)]
        public System.String parameters { get; set; }
    }

    /// <summary>
    ///     商品类型扩展
    /// </summary>
    public partial class CoreCmsGoodsType
    {
        /// <summary>
        ///     关联属性
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsGoodsTypeSpec> spec { get; set; } = new List<CoreCmsGoodsTypeSpec>();


        /// <summary>
        ///     关联参数
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsGoodsParams> parameter { get; set; } = new List<CoreCmsGoodsParams>();
    }
}
