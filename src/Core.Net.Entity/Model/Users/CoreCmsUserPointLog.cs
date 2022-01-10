using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model.Users
{
    /// <summary>
    /// 用户积分记录表
    /// </summary>
    [SugarTable("CoreCmsUserPointLog", TableDescription = "用户积分记录表")]
    public partial class CoreCmsUserPointLog
    {
        /// <summary>
        /// 用户积分记录表
        /// </summary>
        public CoreCmsUserPointLog()
        {
        }

        /// <summary>
        /// ID
        /// </summary>
        [Display(Name = "ID")]
        [SugarColumn(ColumnDescription = "ID", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [Display(Name = "用户ID")]
        [SugarColumn(ColumnDescription = "用户ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userId { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [Display(Name = "类型")]
        [SugarColumn(ColumnDescription = "类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 type { get; set; }
        /// <summary>
        /// 积分数量
        /// </summary>
        [Display(Name = "积分数量")]
        [SugarColumn(ColumnDescription = "积分数量")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 num { get; set; }
        /// <summary>
        /// 积分余额
        /// </summary>
        [Display(Name = "积分余额")]
        [SugarColumn(ColumnDescription = "积分余额")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 balance { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [SugarColumn(ColumnDescription = "备注", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String remarks { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
    }

    /// <summary>
    ///     用户积分
    /// </summary>
    public partial class CoreCmsUserPointLog
    {
        /// <summary>
        ///     类型说明
        /// </summary>
        [Display(Name = "类型说明")]
        [SugarColumn(IsIgnore = true)]
        public string typeName { get; set; }
    }
}
