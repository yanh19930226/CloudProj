using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model.Systems
{
    /// <summary>
    /// 用户表
    /// </summary>
    [SugarTable("SysUser", TableDescription = "用户表")]
    public partial class SysUser
    {
        /// <summary>
        /// 用户表
        /// </summary>
        public SysUser()
        {
        }

        /// <summary>
        /// 用户id
        /// </summary>
        [Display(Name = "用户id")]
        [SugarColumn(ColumnDescription = "用户id", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        [Display(Name = "账号")]
        [SugarColumn(ColumnDescription = "账号", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string userName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        [SugarColumn(ColumnDescription = "密码", IsNullable = true)]
        [StringLength(100, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string passWord { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [Display(Name = "昵称")]
        [SugarColumn(ColumnDescription = "昵称", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string nickName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [Display(Name = "头像")]
        [SugarColumn(ColumnDescription = "头像", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string avatar { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "性别")]
        [SugarColumn(ColumnDescription = "性别")]
        [Required(ErrorMessage = "请输入{0}")]
        public int sex { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Display(Name = "手机号")]
        [SugarColumn(ColumnDescription = "手机号", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string phone { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [Display(Name = "邮箱")]
        [SugarColumn(ColumnDescription = "邮箱", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string email { get; set; }
        /// <summary>
        /// 邮箱是否验证
        /// </summary>
        [Display(Name = "邮箱是否验证")]
        [SugarColumn(ColumnDescription = "邮箱是否验证")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool emailVerified { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        [Display(Name = "真实姓名")]
        [SugarColumn(ColumnDescription = "真实姓名", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string trueName { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [Display(Name = "出生日期")]
        [SugarColumn(ColumnDescription = "出生日期", IsNullable = true)]
        public DateTime? birthday { get; set; }
        /// <summary>
        /// 个人简介
        /// </summary>
        [Display(Name = "个人简介")]
        [SugarColumn(ColumnDescription = "个人简介", IsNullable = true)]
        [StringLength(500, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string introduction { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        [Display(Name = "卡号")]
        [SugarColumn(ColumnDescription = "卡号", IsNullable = true)]
        [StringLength(500, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string cardNo { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        [Display(Name = "身份证号码")]
        [SugarColumn(ColumnDescription = "身份证号码", IsNullable = true)]
        [StringLength(500, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string idCardNo { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        [Display(Name = "余额")]
        [SugarColumn(ColumnDescription = "余额", IsNullable = true)]
        [StringLength(500, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string balance { get; set; }

        /// <summary>
        /// 机构id
        /// </summary>
        [Display(Name = "机构id")]
        [SugarColumn(ColumnDescription = "机构id", IsNullable = true)]
        public int? organizationId { get; set; }
        /// <summary>
        /// 状态,0正常,1冻结
        /// </summary>
        [Display(Name = "状态,0正常,1冻结")]
        [SugarColumn(ColumnDescription = "状态,0正常,1冻结")]
        [Required(ErrorMessage = "请输入{0}")]
        public int state { get; set; }
        /// <summary>
        /// 是否删除,0否,1是
        /// </summary>
        [Display(Name = "是否删除,0否,1是")]
        [SugarColumn(ColumnDescription = "是否删除,0否,1是")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool deleted { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        [Display(Name = "注册时间")]
        [SugarColumn(ColumnDescription = "注册时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime createTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [Display(Name = "修改时间")]
        [SugarColumn(ColumnDescription = "修改时间", IsNullable = true)]
        public DateTime? updateTime { get; set; }
        /// <summary>
        ///     权限序列
        /// </summary>
        [Display(Name = "权限序列")]
        [SugarColumn(IsIgnore = true)]
        public string roleIds { get; set; }
        /// <summary>
        /// 权限列表
        /// </summary>
        [Display(Name = "权限序列")]
        [SugarColumn(IsIgnore = true)]
        public List<SysRole> roles { get; set; }
        /// <summary>
        /// 组织机构名称
        /// </summary>
        [Display(Name = "组织机构名称")]
        //[SugarColumn(IsIgnore = true)]
        public string organizationName { get; set; }
    }
}
