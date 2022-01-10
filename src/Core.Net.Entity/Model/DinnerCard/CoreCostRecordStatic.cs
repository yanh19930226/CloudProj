using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model.DinnerCard
{
    /// <summary>
    /// 对账表
    /// </summary>
    [SugarTable("CoreCostRecordStatic", TableDescription = "对账表")]
    public class CoreCostRecordStatic
    {
        /// <summary>
        /// 对账表
        /// </summary>
        public CoreCostRecordStatic()
        {
        }
        /// <summary>
        /// 对账ID
        /// </summary>
        [Display(Name = "对账ID")]
        [SugarColumn(ColumnDescription = "对账ID", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        [Display(Name = "订单类型")]
        [SugarColumn(ColumnDescription = "订单类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 orderType { get; set; }
        /// <summary>
        ///机构id
        /// </summary>
        [Display(Name = "机构id")]
        [SugarColumn(ColumnDescription = "机构id", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.Int32 organizationId { get; set; }
        /// <summary>
        ///用户id
        /// </summary>
        [Display(Name = "用户id")]
        [SugarColumn(ColumnDescription = "用户id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sysUserId { get; set; }
        /// <summary>
        /// 订单日期
        /// </summary>
        [Display(Name = "订单日期")]
        [SugarColumn(ColumnDescription = "订单日期", IsNullable = true)]
        public System.DateTime? date { get; set; }
        /// <summary>
        ///余额
        /// </summary>
        [Display(Name = "余额")]
        [SugarColumn(ColumnDescription = "余额")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal balance { get; set; }
        /// <summary>
        ///消费金额
        /// </summary>
        [Display(Name = "消费金额")]
        [SugarColumn(ColumnDescription = "消费金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal costFee { get; set; }
        /// <summary>
        ///充值金额
        /// </summary>
        [Display(Name = "充值金额")]
        [SugarColumn(ColumnDescription = "充值金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal rechargeFee { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [Display(Name = "创建日期")]
        [SugarColumn(ColumnDescription = "创建日期", IsNullable = true)]
        public System.DateTime? createDate { get; set; }
    }
}
