using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model.DinnerCard
{
    /// <summary>
    /// 餐卡表
    /// </summary>
    [SugarTable("DinnerCard", TableDescription = "餐卡表")]
    public class DinnerCard
    {
        /// <summary>
        /// 餐卡表
        /// </summary>
        public DinnerCard()
        {
        }
        /// <summary>
        /// 餐卡ID
        /// </summary>
        [Display(Name = "餐卡ID")]
        [SugarColumn(ColumnDescription = "餐卡ID", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 会员id
        /// </summary>
        [Display(Name = "用户id")]
        [SugarColumn(ColumnDescription = "用户id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sysUserId { get; set; }
        /// <summary>
        ///餐卡号
        /// </summary>
        [Display(Name = "餐卡号")]
        [SugarColumn(ColumnDescription = "餐卡号", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String cardNo { get; set; }
        /// <summary>
        ///餐卡状态
        /// </summary>
        [Display(Name = "餐卡状态")]
        [SugarColumn(ColumnDescription = "餐卡状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 status { get; set; }
        /// <summary>
        ///余额
        /// </summary>
        [Display(Name = "余额")]
        [SugarColumn(ColumnDescription = "余额")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal Balance { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间", IsNullable = true)]
        public System.DateTime? createTime { get; set; }
    }
}
