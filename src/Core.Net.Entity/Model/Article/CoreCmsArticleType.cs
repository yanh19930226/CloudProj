using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model.Article
{
    /// <summary>
    /// 文章分类表
    /// </summary>
    [SugarTable("CoreCmsArticleType", TableDescription = "文章分类表")]
    public partial class CoreCmsArticleType
    {
        /// <summary>
        /// 文章分类表
        /// </summary>
        public CoreCmsArticleType()
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
        /// 分类名称
        /// </summary>
        [Display(Name = "分类名称")]
        [SugarColumn(ColumnDescription = "分类名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(32, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 父id
        /// </summary>
        [Display(Name = "父id")]
        [SugarColumn(ColumnDescription = "父id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 parentId { get; set; }
        /// <summary>
        /// 排序 
        /// </summary>
        [Display(Name = "排序 ")]
        [SugarColumn(ColumnDescription = "排序 ")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sort { get; set; }
    }
}
