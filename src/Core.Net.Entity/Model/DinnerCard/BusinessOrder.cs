using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model.DinnerCard
{
    /// <summary>
    /// 商家订单表
    /// </summary>
    [SugarTable("BusinessOrder", TableDescription = "商家订单表")]
    public class BusinessOrder
    {
        /// <summary>
        /// 商家订单表
        /// </summary>
        public BusinessOrder()
        {

        }
        /// <summary>
        /// 商家ID
        /// </summary>
        [Display(Name = "商家ID")]
        [SugarColumn(ColumnDescription = "商家ID", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 商家ID
        /// </summary>
        [Display(Name = "商家ID")]
        [SugarColumn(ColumnDescription = "商家ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 businessid { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        [Display(Name = "订单类型")]
        [SugarColumn(ColumnDescription = "订单类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 ordertype { get; set; }
        /// <summary>
        /// 商家名称
        /// </summary>
        [Display(Name = "商家名称")]
        [SugarColumn(ColumnDescription = "商家名称")]
        public string businessName { get; set; }
        /// <summary>
        /// 商家名称
        /// </summary>
        [Display(Name = "商家名称")]
        [SugarColumn(ColumnDescription = "商家名称")]
        [Required(ErrorMessage = "请输入{0}")]
        public string orderNo { get; set; }
        /// <summary>
        /// 商家名称
        /// </summary>
        [Display(Name = "商家名称")]
        [SugarColumn(ColumnDescription = "商家名称")]
        public decimal before { get; set; }
        /// <summary>
        /// 商家余额
        /// </summary>
        [Display(Name = "商家余额")]
        [SugarColumn(ColumnDescription = "商家余额")]
        public decimal after { get; set; }
        /// <summary>
        /// 商家余额
        /// </summary>
        [Display(Name = "商家余额")]
        [SugarColumn(ColumnDescription = "商家余额")]
        public decimal change { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间", IsNullable = true)]
        public System.DateTime? createtime { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        [Display(Name = "用户id")]
        [SugarColumn(ColumnDescription = "用户id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userid { get; set; }
        /// <summary>
        /// 商家名称
        /// </summary>
        [Display(Name = "商家名称")]
        [SugarColumn(ColumnDescription = "商家名称")]
        [Required(ErrorMessage = "请输入{0}")]
        public string userName { get; set; }
    }
}
