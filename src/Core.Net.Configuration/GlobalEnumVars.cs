using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Net.Configuration
{
    public class GlobalEnumVars
    {

        #region User用户相关===========================================================================
        /// <summary>
        /// 性别[1男2女3未知]
        /// 对应CoreCmsUserWX表的gender类型
        /// </summary>
        public enum UserSexTypes
        {
            [Description("男")]
            男 = 1,
            [Description("女")]
            女 = 2,
            [Description("未知")]
            未知 = 3
        }
        /// <summary>
        /// 用户状态
        /// </summary>
        public enum UserStatus
        {
            [Description("正常")]
            正常 = 1,
            [Description("停用")]
            停用 = 2
        }
        /// <summary>
        /// 第三方账号来源
        /// [对应CoreCmsUserWX表的type类型]
        /// </summary>
        public enum UserAccountTypes
        {
            [Description("微信公众号")]
            微信公众号 = 1,
            [Description("微信小程序")]
            微信小程序 = 2,
            [Description("支付宝小程序")]
            支付宝小程序 = 3,
            [Description("微信APP快捷登陆")]
            微信APP快捷登陆 = 4,
            [Description("QQ在APP中快捷登陆")]
            QQ在APP中快捷登陆 = 5,
            [Description("头条系小程序")]
            头条系小程序 = 6,
        }
        /// <summary>
        /// 用户余额变动来源类型【对应CoreCmsUserBalance.type字段】
        /// </summary>
        public enum UserBalanceSourceTypes
        {
            /// <summary>
            /// 用户消费
            /// </summary>
            [Description("用户消费")]
            Pay = 1,
            /// <summary>
            /// 用户退款
            /// </summary>
            [Description("用户退款")]
            Refund = 2,
            /// <summary>
            /// 充值
            /// </summary>
            [Description("充值")]
            Recharge = 3,
            /// <summary>
            /// 提现
            /// </summary>
            [Description("提现")]
            Tocash = 4,
            /// <summary>
            /// 三级分销佣金
            /// </summary>
            [Description("三级分销佣金")]
            Distribution = 5,
            /// <summary>
            /// 平台调整
            /// </summary>
            [Description("平台调整")]
            Admin = 6,
            /// <summary>
            /// 奖励
            /// </summary>
            [Description("奖励")]
            Prize = 7,
            /// <summary>
            /// 服务项目
            /// </summary>
            [Description("服务订单")]
            Service = 8,
            /// <summary>
            /// 代理商提成
            /// </summary>
            [Description("代理商提成")]
            Agent = 9,
        }
        /// <summary>
        /// 用户积分变动来源类型
        /// 对应CoreCmsUserPointLog表type字段
        /// </summary>
        public enum UserPointSourceTypes
        {
            /// <summary>
            /// 签到
            /// </summary>
            [Description("签到")]
            PointTypeSign = 1,
            /// <summary>
            /// 购物返积分
            /// </summary>
            [Description("购物返积分")]
            PointTypeRebate = 2,
            /// <summary>
            /// 购物使用积分
            /// </summary>
            [Description("购物使用积分")]
            PointTypeDiscount = 3,
            /// <summary>
            /// 后台编辑
            /// </summary>
            [Description("后台编辑")]
            PointTypeAdminEdit = 4,
            /// <summary>
            /// 奖励积分
            /// </summary>
            [Description("奖励积分")]
            PointTypePrize = 5,
            /// <summary>
            /// 积分兑换
            /// </summary>
            [Description("积分兑换")]
            PointTypeExchange = 6,
            /// <summary>
            /// 售后退款返还
            /// </summary>
            [Description("售后退款返还")]
            PointRefundReturn = 7,
            /// <summary>
            /// 取消订单返还
            /// </summary>
            [Description("取消订单返还")]
            PointCanCelOrder = 8,
        }

        /// <summary>
        /// 用户签到积分类型
        /// </summary>
        public enum UserPointSignTypes
        {
            /// <summary>
            /// 签到固定积分
            /// </summary>
            [Description("签到固定积分")]
            FixedPoint = 1,
            /// <summary>
            /// 签到随机积分
            /// </summary>
            [Description("签到随机积分")]
            RandomPoint = 2
        }


        /// <summary>
        /// 用户日志状态[对应CoreCmsUserLog表的state字段]
        /// </summary>
        public enum UserLogTypes
        {
            [Description("登录")]
            登录 = 1,
            [Description("退出")]
            退出 = 2,
            [Description("注册")]
            注册 = 3
        }
        /// <summary>
        /// 用户提现状态[对应CoreCmsUserTocash表的status字段]
        /// </summary>
        public enum UserTocashTypes
        {
            [Description("待审核")]
            待审核 = 1,
            [Description("提现成功")]
            提现成功 = 2,
            [Description("提现失败")]
            提现失败 = 3
        }
        #endregion


        /// <summary>
        /// 附件存储支持类型
        /// </summary>
        public enum FilesStorageOptionsType
        {
            [Description("本地存储")]
            LocalStorage = 0,
            [Description("阿里云OSS")]
            AliYunOSS = 1,
            [Description("腾讯云OSS")]
            QCloudOSS = 2,
        }
        /// <summary>
        /// 用户登录日志类型
        /// </summary>

        public enum LoginRecordType
        {
            登录成功 = 0,
            登录失败 = 1,
            退出登录 = 2,
            刷新Token = 0
        }

        /// <summary>
        /// 广告模板编码
        /// </summary>
        public enum AdvertTemplateCode
        {
            /// <summary>
            /// 首页幻灯片广告位
            /// </summary>
            [Description("首页幻灯片广告位")]
            TplSlider = 1,
            /// <summary>
            /// 首页广告位1
            /// </summary>
            [Description("首页广告位1")]
            TplIndexBanner1 = 2,
            /// <summary>
            /// 首页广告位2
            /// </summary>
            [Description("首页广告位2")]
            TplIndexBanner2 = 3,
            /// <summary>
            /// 首页广告位3
            /// </summary>
            [Description("首页广告位3")]
            TplIndexBanner3 = 4,
            /// <summary>
            /// 分类页广告位
            /// </summary>
            [Description("分类页广告位")]
            TplClassBanner1 = 5
        }


        #region 广告相关==================================================
        /// <summary>
        /// 广告表类型【关联CoreCmsAdvertisement.type字段】
        /// </summary>
        public enum AdvertPositionType
        {
            /// <summary>
            /// 网址URL
            /// </summary>
            [Description("网址URL")]
            Url = 1,
            /// <summary>
            /// 商品
            /// </summary>
            [Description("商品")]
            Good = 2,
            /// <summary>
            /// 文章
            /// </summary>
            [Description("文章")]
            Article = 3,
            /// <summary>
            /// 文章分类
            /// </summary>
            [Description("文章分类")]
            ArticleType = 4,
            /// <summary>
            /// 智能表单
            /// </summary>
            [Description("智能表单")]
            IntelligenceForm = 5
        }

        #endregion

        #region 促销相关===================================================
        /// <summary>
        /// 促销形式类型【对应CoreCmsPromotion.type字段】
        /// </summary>
        public enum PromotionType
        {
            /// <summary>
            /// 促销
            /// </summary>
            [Description("促销")]
            Promotion = 1,
            /// <summary>
            /// 优惠券
            /// </summary>
            [Description("优惠券")]
            Coupon = 2,
            /// <summary>
            /// 团购
            /// </summary>
            [Description("团购")]
            Group = 3,
            /// <summary>
            /// 秒杀
            /// </summary>
            [Description("秒杀")]
            Seckill = 4,
        }


        /// <summary>
        /// 团购秒杀活动状态
        /// </summary>
        public enum GroupSeckillStatus
        {
            /// <summary>
            /// 即将开始
            /// </summary>
            [Description("即将开始")]
            Upcoming = 0,
            /// <summary>
            /// 进行中
            /// </summary>
            [Description("进行中")]
            InProgress = 1,
            /// <summary>
            /// 已结束
            /// </summary>
            [Description("已结束")]
            Finished = 2
        }


        #endregion

        #region 商品相关==============================================================
        /// <summary>
        /// 商品参数表类型
        /// </summary>
        public enum GoodsParamTypes
        {
            /// <summary>
            /// 文本框
            /// </summary>
            [Description("文本框")]
            text = 1,
            /// <summary>
            /// 单选
            /// </summary>
            [Description("单选")]
            radio = 2,
            /// <summary>
            /// 复选框
            /// </summary>
            [Description("复选框")]
            checkbox = 3,
        }
        /// <summary>
        /// 商品分销方式
        /// </summary>
        public enum ProductsDistributionType
        {
            /// <summary>
            /// 全局设置
            /// </summary>
            [Description("全局设置")]
            Global = 1,
            /// <summary>
            /// 单独设置
            /// </summary>
            [Description("单独设置")]
            Detail = 2,
        }


        #endregion
    }
}
