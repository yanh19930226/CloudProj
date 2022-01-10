using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model.Systems
{
    /// <summary>
    /// 用户角色关联表
    /// </summary>
    [SugarTable("SysUserRole", TableDescription = "用户角色关联表")]
    public partial class SysUserRole
    {
        /// <summary>
        /// 用户角色关联表
        /// </summary>
        public SysUserRole()
        {
        }

        /// <summary>
        /// 主键
        /// </summary>
        [Display(Name = "主键")]
        [SugarColumn(ColumnDescription = "主键", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        [Display(Name = "用户id")]
        [SugarColumn(ColumnDescription = "用户id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userId { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        [Display(Name = "角色id")]
        [SugarColumn(ColumnDescription = "角色id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 roleId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [Display(Name = "修改时间")]
        [SugarColumn(ColumnDescription = "修改时间", IsNullable = true)]
        public System.DateTime? updateTime { get; set; }
    }
}
