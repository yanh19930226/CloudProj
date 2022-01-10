using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model.Users
{
    /// <summary>
    /// 用户等级表
    /// </summary>
    [SugarTable("CoreCmsUserGrade", TableDescription = "用户等级表")]
    public partial class CoreCmsUserGrade
    {
        /// <summary>
        /// 用户等级表
        /// </summary>
        public CoreCmsUserGrade()
        {
        }

        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
        [SugarColumn(ColumnDescription = "id", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [Display(Name = "标题")]
        [SugarColumn(ColumnDescription = "标题")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(60, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String title { get; set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        [Display(Name = "是否默认")]
        [SugarColumn(ColumnDescription = "是否默认")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isDefault { get; set; }
    }
}
