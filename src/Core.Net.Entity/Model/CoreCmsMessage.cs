using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model
{
    /// <summary>
    /// 消息发送表
    /// </summary>
    [SugarTable("CoreCmsMessage", TableDescription = "消息发送表")]
    public partial class CoreCmsMessage
    {
        /// <summary>
        /// 消息发送表
        /// </summary>
        public CoreCmsMessage()
        {
        }

        /// <summary>
        /// 序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(ColumnDescription = "序列", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public Int32 id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        [Display(Name = "用户id")]
        [SugarColumn(ColumnDescription = "用户id")]
        [Required(ErrorMessage = "请输入{0}")]
        public Int32 userId { get; set; }
        /// <summary>
        /// 消息编码
        /// </summary>
        [Display(Name = "消息编码")]
        [SugarColumn(ColumnDescription = "消息编码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public String code { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        [Display(Name = "参数")]
        [SugarColumn(ColumnDescription = "参数", IsNullable = true)]
        public String parameters { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [Display(Name = "内容")]
        [SugarColumn(ColumnDescription = "内容", IsNullable = true)]
        public String contentBody { get; set; }
        /// <summary>
        /// 是否查看
        /// </summary>
        [Display(Name = "是否查看")]
        [SugarColumn(ColumnDescription = "是否查看")]
        [Required(ErrorMessage = "请输入{0}")]
        public Boolean status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime createTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
        public DateTime? updateTime { get; set; }
    }
}
