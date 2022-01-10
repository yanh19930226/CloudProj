using Core.Net.Entity.Model.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Model.Goods
{
    /// <summary>
    /// 商品表
    /// </summary>
    [SugarTable("CoreCmsGoods", TableDescription = "商品表")]
    public partial class CoreCmsGoods
    {
        /// <summary>
        /// 商品表
        /// </summary>
        public CoreCmsGoods()
        {
        }

        /// <summary>
        /// 商品ID
        /// </summary>
        [Display(Name = "商品ID")]
        [SugarColumn(ColumnDescription = "商品ID", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 商品条码
        /// </summary>
        [Display(Name = "商品条码")]
        [SugarColumn(ColumnDescription = "商品条码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(30, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String bn { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [Display(Name = "商品名称")]
        [SugarColumn(ColumnDescription = "商品名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 商品简介
        /// </summary>
        [Display(Name = "商品简介")]
        [SugarColumn(ColumnDescription = "商品简介", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String brief { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        [Display(Name = "缩略图")]
        [SugarColumn(ColumnDescription = "缩略图", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String image { get; set; }
        /// <summary>
        /// 图集
        /// </summary>
        [Display(Name = "图集")]
        [SugarColumn(ColumnDescription = "图集", IsNullable = true)]
        public System.String images { get; set; }
        /// <summary>
        /// 视频
        /// </summary>
        [Display(Name = "视频")]
        [SugarColumn(ColumnDescription = "视频", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String video { get; set; }
        /// <summary>
        /// 佣金分配方式
        /// </summary>
        [Display(Name = "佣金分配方式")]
        [SugarColumn(ColumnDescription = "佣金分配方式")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 productsDistributionType { get; set; }
        /// <summary>
        /// 商品分类
        /// </summary>
        [Display(Name = "商品分类")]
        [SugarColumn(ColumnDescription = "商品分类")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 goodsCategoryId { get; set; }
        /// <summary>
        /// 商品类别
        /// </summary>
        [Display(Name = "商品类别")]
        [SugarColumn(ColumnDescription = "商品类别")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 goodsTypeId { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        [Display(Name = "品牌")]
        [SugarColumn(ColumnDescription = "品牌")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 brandId { get; set; }
        /// <summary>
        /// 是否虚拟商品
        /// </summary>
        [Display(Name = "是否虚拟商品")]
        [SugarColumn(ColumnDescription = "是否虚拟商品")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isNomalVirtual { get; set; }
        /// <summary>
        /// 是否上架
        /// </summary>
        [Display(Name = "是否上架")]
        [SugarColumn(ColumnDescription = "是否上架")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isMarketable { get; set; }
        /// <summary>
        /// 商品单位
        /// </summary>
        [Display(Name = "商品单位")]
        [SugarColumn(ColumnDescription = "商品单位", IsNullable = true)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String unit { get; set; }
        /// <summary>
        /// 商品详情
        /// </summary>
        [Display(Name = "商品详情")]
        [SugarColumn(ColumnDescription = "商品详情", IsNullable = true)]
        public System.String intro { get; set; }
        /// <summary>
        /// 商品规格序列号存储
        /// </summary>
        [Display(Name = "商品规格序列号存储")]
        [SugarColumn(ColumnDescription = "商品规格序列号存储", IsNullable = true)]
        public System.String spesDesc { get; set; }
        /// <summary>
        /// 参数序列化
        /// </summary>
        [Display(Name = "参数序列化")]
        [SugarColumn(ColumnDescription = "参数序列化", IsNullable = true)]
        public System.String parameters { get; set; }
        /// <summary>
        /// 评论次数
        /// </summary>
        [Display(Name = "评论次数")]
        [SugarColumn(ColumnDescription = "评论次数")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 commentsCount { get; set; }
        /// <summary>
        /// 浏览次数
        /// </summary>
        [Display(Name = "浏览次数")]
        [SugarColumn(ColumnDescription = "浏览次数")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 viewCount { get; set; }
        /// <summary>
        /// 购买次数
        /// </summary>
        [Display(Name = "购买次数")]
        [SugarColumn(ColumnDescription = "购买次数")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 buyCount { get; set; }
        /// <summary>
        /// 上架时间
        /// </summary>
        [Display(Name = "上架时间")]
        [SugarColumn(ColumnDescription = "上架时间", IsNullable = true)]
        public System.DateTime? uptime { get; set; }
        /// <summary>
        /// 下架时间
        /// </summary>
        [Display(Name = "下架时间")]
        [SugarColumn(ColumnDescription = "下架时间", IsNullable = true)]
        public System.DateTime? downtime { get; set; }
        /// <summary>
        /// 商品排序
        /// </summary>
        [Display(Name = "商品排序")]
        [SugarColumn(ColumnDescription = "商品排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sort { get; set; }
        /// <summary>
        /// 标签id逗号分隔
        /// </summary>
        [Display(Name = "标签id逗号分隔")]
        [SugarColumn(ColumnDescription = "标签id逗号分隔", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String labelIds { get; set; }
        /// <summary>
        /// 自定义规格名称
        /// </summary>
        [Display(Name = "自定义规格名称")]
        [SugarColumn(ColumnDescription = "自定义规格名称", IsNullable = true)]
        public System.String newSpec { get; set; }
        /// <summary>
        /// 开启规则
        /// </summary>
        [Display(Name = "开启规则")]
        [SugarColumn(ColumnDescription = "开启规则")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 openSpec { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间", IsNullable = true)]
        public System.DateTime? createTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
        public System.DateTime? updateTime { get; set; }
        /// <summary>
        /// 是否推荐
        /// </summary>
        [Display(Name = "是否推荐")]
        [SugarColumn(ColumnDescription = "是否推荐")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isRecommend { get; set; }
        /// <summary>
        /// 是否热门
        /// </summary>
        [Display(Name = "是否热门")]
        [SugarColumn(ColumnDescription = "是否热门")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isHot { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        [SugarColumn(ColumnDescription = "是否删除")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isDel { get; set; }
    }

    /// <summary>
    ///     商品类型扩展
    /// </summary>
    public partial class CoreCmsGoods
    {
        /// <summary>
        ///     货品编码
        /// </summary>
        [Display(Name = "货品编码")]
        [SugarColumn(IsIgnore = true)]
        public string sn { get; set; }

        /// <summary>
        ///     销售价
        /// </summary>
        [Display(Name = "销售价")]
        [SugarColumn(IsIgnore = true)]
        public decimal price { get; set; } = 0;

        /// <summary>
        ///     成本价
        /// </summary>
        [Display(Name = "成本价")]
        [SugarColumn(IsIgnore = true)]
        public decimal costprice { get; set; } = 0;

        /// <summary>
        ///     市场价
        /// </summary>
        [Display(Name = "市场价")]
        [SugarColumn(IsIgnore = true)]
        public decimal mktprice { get; set; } = 0;


        /// <summary>
        ///     库存
        /// </summary>
        [Display(Name = "库存")]
        [SugarColumn(IsIgnore = true)]
        public int stock { get; set; } = 0;

        /// <summary>
        ///     冻结库存
        /// </summary>
        [Display(Name = "冻结库存")]
        [SugarColumn(IsIgnore = true)]
        public int freezeStock { get; set; } = 0;

        /// <summary>
        ///     重量
        /// </summary>
        [Display(Name = "重量")]
        [SugarColumn(IsIgnore = true)]
        public decimal weight { get; set; } = 0;


        /// <summary>
        ///     图集
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string[] album { get; set; }

        /// <summary>
        ///     品牌数据
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public Brand brand { get; set; }

        ///// <summary>
        /////     关联参数
        ///// </summary>
        //[SugarColumn(IsIgnore = true)]
        //public CoreCmsProducts product { get; set; }


        /// <summary>
        ///     是否收藏
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool isFav { get; set; } = false;


        ///// <summary>
        /////     关联拼团规则
        ///// </summary>
        //[SugarColumn(IsIgnore = true)]
        //public CoreCmsPinTuanRule pinTuanRule { get; set; } = null;

        /// <summary>
        ///     拼团价格
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public decimal pinTuanPrice { get; set; } = 0;

        ///// <summary>
        /////     拼团记录
        ///// </summary>
        //[SugarColumn(IsIgnore = true)]
        //public List<CoreCmsPinTuanRecord> pinTuanRecord { get; set; } = new();


        ///// <summary>
        /////     拼团记录数量
        ///// </summary>
        //[SugarColumn(IsIgnore = true)]
        //public int pinTuanRecordNums { get; set; } = 0;

        ///// <summary>
        /////     拼团总单数
        ///// </summary>
        //[SugarColumn(IsIgnore = true)]
        //public int buyPinTuanCount { get; set; } = 0;

        ///// <summary>
        /////     团购秒杀促销总单数
        ///// </summary>
        //[SugarColumn(IsIgnore = true)]
        //public int buyPromotionCount { get; set; } = 0;

        /// <summary>
        ///     标签列表
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsLabel> labels { get; set; } = new List<CoreCmsLabel>();

        ///// <summary>
        /////     所属团购秒杀
        ///// </summary>
        //[SugarColumn(IsIgnore = true)]
        //public int groupId { get; set; }

        //[SugarColumn(IsIgnore = true)] public int groupType { get; set; }

        //[SugarColumn(IsIgnore = true)] public bool groupStatus { get; set; }

        //[SugarColumn(IsIgnore = true)] public DateTime groupTime { get; set; }

        //[SugarColumn(IsIgnore = true)] public DateTime groupStartTime { get; set; }

        //[SugarColumn(IsIgnore = true)] public DateTime groupEndTime { get; set; }

        //[SugarColumn(IsIgnore = true)] public int groupTimestamp { get; set; }
    }
}
