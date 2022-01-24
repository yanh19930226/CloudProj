using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model.DinnerCard
{
    /// <summary>
    /// 商家表
    /// </summary>
    [SugarTable("Business", TableDescription = "商家表")]
    public class Business
    {
        /// <summary>
        /// 商家表
        /// </summary>
        public Business()
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
        /// 商家名称
        /// </summary>
        [Display(Name = "商家名称")]
        [SugarColumn(ColumnDescription = "商家名称")]
        [Required(ErrorMessage = "请输入{0}")]
        public string businessName { get; set; }
        /// <summary>
        /// 商家名称
        /// </summary>
        [Display(Name = "商家名称")]
        [SugarColumn(ColumnDescription = "商家名称")]
        public int sort { get; set; }
        /// <summary>
        /// 商家余额
        /// </summary>
        [Display(Name = "商家余额")]
        [SugarColumn(ColumnDescription = "商家余额")]
        public decimal money { get; set; }
        /// <summary>
        ///状态
        /// </summary>
        [Display(Name = "状态")]
        [SugarColumn(ColumnDescription = "状态", IsNullable = true)]
        public bool status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间", IsNullable = true)]
        public System.DateTime? createTime { get; set; }
    }
}
