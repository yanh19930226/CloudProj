using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model.DinnerCard
{
    /// <summary>
    /// 商品统计
    /// </summary>
    [SugarTable("CoreGoodStatic", TableDescription = "商品统计")]
    public class CoreGoodStatic
    {
        /// <summary>
        /// 商品统计
        /// </summary>
        public CoreGoodStatic()
        {

        }
        /// <summary>
        /// 商品ID
        /// </summary>
        [Display(Name = "ID")]
        [SugarColumn(ColumnDescription = "ID", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [Display(Name = "商品名称")]
        [SugarColumn(ColumnDescription = "商品名称")]
        [Required(ErrorMessage = "请输入{0}")]
        public string goodName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间", IsNullable = true)]
        public System.DateTime? createTime { get; set; }
    }
}
