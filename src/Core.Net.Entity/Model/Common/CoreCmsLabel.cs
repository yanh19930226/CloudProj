using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model.Common
{
    /// <summary>
    /// 标签表
    /// </summary>
    [SugarTable("CoreCmsLabel", TableDescription = "标签表")]
    public partial class CoreCmsLabel
    {
        /// <summary>
        /// 标签表
        /// </summary>
        public CoreCmsLabel()
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
        /// 标签名称
        /// </summary>
        [Display(Name = "标签名称")]
        [SugarColumn(ColumnDescription = "标签名称", IsNullable = true)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 标签样式
        /// </summary>
        [Display(Name = "标签样式")]
        [SugarColumn(ColumnDescription = "标签样式", IsNullable = true)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String style { get; set; }
    }
}
