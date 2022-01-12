using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model.DinnerCard
{
    /// <summary>
    /// 订单表
    /// </summary>
    [SugarTable("CoreOrder", TableDescription = "订单表")]
    public class CoreOrder
    {
        /// <summary>
        /// 商品订单表
        /// </summary>
        public CoreOrder()
        {

        }
        /// <summary>
        ///单ID
        /// </summary>
        [Display(Name = "订单ID")]
        [SugarColumn(ColumnDescription = "商品订单ID", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 机构Id
        /// </summary>
        [Display(Name = "机构Id")]
        [SugarColumn(ColumnDescription = "机构Id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 oraganizationId { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        [Display(Name = "角色Id")]
        [SugarColumn(ColumnDescription = "角色Id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 roleId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        [Display(Name = "用户Id")]
        [SugarColumn(ColumnDescription = "用户Id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sysUserId { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        [Display(Name = "机构名称")]
        [SugarColumn(ColumnDescription = "机构名称")]
        [Required(ErrorMessage = "请输入{0}")]
        public string organizationName { get; set; }
        /// <summary>
        /// roleName
        /// </summary>
        [Display(Name = "roleName")]
        [SugarColumn(ColumnDescription = "roleName")]
        [Required(ErrorMessage = "请输入{0}")]
        public string roleName { get; set; }
        /// <summary>
        /// 外部订单编号
        /// </summary>
        [Display(Name = "外部订单编号")]
        [SugarColumn(ColumnDescription = "外部订单编号")]
        [Required(ErrorMessage = "请输入{0}")]
        public string costOrderNo { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        [Display(Name = "订单编号")]
        [SugarColumn(ColumnDescription = "商品订单编号")]
        [Required(ErrorMessage = "请输入{0}")]
        public string orderNo { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        [Display(Name = "订单类型")]
        [SugarColumn(ColumnDescription = "订单类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public int orderType { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        [Display(Name = "订单状态")]
        [SugarColumn(ColumnDescription = "订单状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public int status { get; set; }
        /// <summary>
        /// 消费金额
        /// </summary>
        [Display(Name = "消费金额")]
        [SugarColumn(ColumnDescription = "消费金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal amount { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        [Display(Name = "余额")]
        [SugarColumn(ColumnDescription = "余额")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal balance { get; set; }
        /// <summary>
        /// 订单总价
        /// </summary>
        [Display(Name = "订单总价")]
        [SugarColumn(ColumnDescription = "订单总价")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal totalPrice { get; set; }
        /// <summary>
        ///订单完成时间
        /// </summary>
        [Display(Name = "订单完成时间")]
        [SugarColumn(ColumnDescription = "订单完成时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime completeTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间", IsNullable = true)]
        public System.DateTime createTime { get; set; }

        #region 商品信息
        ///// <summary>
        /////商品名称
        ///// </summary>
        //[Display(Name = "商品名称")]
        //[SugarColumn(ColumnDescription = "商品名称", IsNullable = true)]
        //[StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        //public System.String goodName { get; set; }
        ///// <summary>
        /////商品编号
        ///// </summary>
        //[Display(Name = "商品编号")]
        //[SugarColumn(ColumnDescription = "商品编号", IsNullable = true)]
        //[StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        //public System.String goodNo { get; set; }

        ///// <summary>
        /////商品图片
        ///// </summary>
        //[Display(Name = "商品图片")]
        //[SugarColumn(ColumnDescription = "商品图片", IsNullable = true)]
        //[StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        //public System.String goodUrl { get; set; }

        ///// <summary>
        ///// 商品数量
        ///// </summary>
        //[Display(Name = "商品数量")]
        //[SugarColumn(ColumnDescription = "商品数量")]
        //[Required(ErrorMessage = "请输入{0}")]
        //public int? goodNumber { get; set; }
        ///// <summary>
        /////商品单价(冗余字段)
        ///// </summary>
        //[Display(Name = "商品单价")]
        //[SugarColumn(ColumnDescription = "商品单价")]
        //[Required(ErrorMessage = "请输入{0}")]
        //public decimal? unitPrice { get; set; }

        #endregion

        #region 付款信息
        /// <summary>
        /// 订单付款方式
        /// </summary>
        [Display(Name = "订单付款方式")]
        [SugarColumn(ColumnDescription = "订单付款方式")]
        [Required(ErrorMessage = "请输入{0}")]
        public int payType { get; set; } 
        #endregion

        #region 用户信息
        /// <summary>
        ///用户名
        /// </summary>
        [Display(Name = "用户名")]
        [SugarColumn(ColumnDescription = "用户名", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String userName { get; set; }
        /// <summary>
        ///用户手机号
        /// </summary>
        [Display(Name = "用户手机号")]
        [SugarColumn(ColumnDescription = "用户手机号")]
        [Required(ErrorMessage = "请输入{0}")]
        public string telePhone { get; set; }
        /// <summary>
        ///餐卡号
        /// </summary>
        [Display(Name = "餐卡号")]
        [SugarColumn(ColumnDescription = "餐卡号")]
        [Required(ErrorMessage = "请输入{0}")]
        public string cardNo { get; set; }
        #endregion
    
    }
}
