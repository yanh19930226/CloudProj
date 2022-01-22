using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model.DinnerCard
{
    /// <summary>
    /// 餐卡详细记录表
    /// </summary>
    [SugarTable("DinnerCardDetail", TableDescription = "餐卡详细记录表")]
    public class DinnerCardDetail
    {
        /// <summary>
        /// 餐卡详细记录表
        /// </summary>
        public DinnerCardDetail()
        {
        }
        /// <summary>
        /// 餐卡详细ID
        /// </summary>
        [Display(Name = "餐卡详细ID")]
        [SugarColumn(ColumnDescription = "餐卡详细ID", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        ///餐卡号
        /// </summary>
        [Display(Name = "餐卡号")]
        [SugarColumn(ColumnDescription = "餐卡号", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String cardno { get; set; }
        /// <summary>
        ///用户
        /// </summary>
        [Display(Name = "用户")]
        [SugarColumn(ColumnDescription = "用户", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String username { get; set; }
        /// <summary>
        ///机构名
        /// </summary>
        [Display(Name = "机构名")]
        [SugarColumn(ColumnDescription = "机构名", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String orgname { get; set; }
        /// <summary>
        ///电话
        /// </summary>
        [Display(Name = "电话")]
        [SugarColumn(ColumnDescription = "电话", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String telephone { get; set; }
        /// <summary>
        ///action
        /// </summary>
        [Display(Name = "action")]
        [SugarColumn(ColumnDescription = "action", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String action { get; set; }
        /// <summary>
        ///opuser
        /// </summary>
        [Display(Name = "opuser")]
        [SugarColumn(ColumnDescription = "opuser", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String opuser { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间", IsNullable = true)]
        public System.DateTime? createtime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [Display(Name = "修改时间")]
        [SugarColumn(ColumnDescription = "修改时间", IsNullable = true)]
        public System.DateTime? optime { get; set; }
    }
}
