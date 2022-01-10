using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Configuration
{
    /// <summary>
    /// 权限变量配置
    /// </summary>
    public static class Permissions
    {
        public const string Name = "Permission";

        /// <summary>
        /// 当前项目是否启用IDS4权限方案
        /// true：表示启动IDS4
        /// false：表示使用JWT
        public static bool IsUseIds4 = false;
    }
    public class GlobalConstVars
    {

        /// <summary>
        /// Excel导出成功
        /// </summary>
        public const string ExcelExportSuccess = "Excel导出成功";
        /// <summary>
        /// 数据删除成功
        /// </summary>
        public const string DeleteSuccess = "数据删除成功";
        /// <summary>
        /// 数据删除失败
        /// </summary>
        public const string DeleteFailure = "数据删除失败";
        /// <summary>
        /// 系统禁止删除此数据
        /// </summary>
        public const string DeleteProhibitDelete = "系统禁止删除此数据";
        /// <summary>
        /// 此数据含有子类信息，禁止删除
        /// </summary>
        public const string DeleteIsHaveChildren = "此数据含有子类信息，禁止删除";
        /// <summary>
        /// 数据处理异常
        /// </summary>
        public const string DataHandleEx = "数据接口出现异常";
        /// <summary>
        /// 数据添加成功
        /// </summary>
        public const string CreateSuccess = "数据添加成功";
        /// <summary>
        /// 数据添加失败
        /// </summary>
        public const string CreateFailure = "数据添加失败";
        /// <summary>
        /// 数据移动成功
        /// </summary>
        public const string MoveSuccess = "数据移动成功";
        /// <summary>
        /// 数据移动失败
        /// </summary>
        public const string MoveFailure = "数据移动失败";
        /// <summary>
        /// 系统禁止添加数据
        /// </summary>
        public const string CreateProhibitCreate = "系统禁止添加数据";
        /// <summary>
        /// 数据编辑成功
        /// </summary>
        public const string EditSuccess = "数据编辑成功";
        /// <summary>
        /// 数据编辑失败
        /// </summary>
        public const string EditFailure = "数据编辑失败";
        /// <summary>
        /// 系统禁止编辑此数据
        /// </summary>
        public const string EditProhibitEdit = "系统禁止编辑此数据";
        /// <summary>
        /// 数据已存在
        /// </summary>
        public const string DataIsHave = "数据已存在";
        /// <summary>
        /// 数据不存在
        /// </summary>
        public const string DataisNo = "数据不存在";
        /// <summary>
        /// 请提交必要的参数
        /// </summary>
        public const string DataParameterError = "请提交必要的参数";
        /// <summary>
        /// 数据插入成功
        /// </summary>
        public const string InsertSuccess = "数据插入成功！";
        /// <summary>
        /// 数据插入失败
        /// </summary>
        public const string InsertFailure = "数据插入失败！";
        /// <summary>
        /// 设置数据成功
        /// </summary>
        public const string SetDataSuccess = "设置数据成功！";
        /// <summary>
        /// 设置数据异常
        /// </summary>
        public const string SetDataException = "设置数据异常！";
        /// <summary>
        /// 设置数据失败
        /// </summary>
        public const string SetDataFailure = "设置数据失败！";


        /// <summary>
        /// RedisMqKey队列
        /// </summary>
        public static class RedisMessageQueueKey
        {
            /// <summary>
            /// 微信支付成功后推送到接口进行数据处理
            /// </summary>
            public const string WeChatPayNoticeQueue = "WeChatPayNoticeQueue";
            /// <summary>
            /// 订单完结后走代理或分销商提成处理
            /// </summary>
            public const string OrderAgentOrDistributionSubscribe = "OrderAgentOrDistributionSubscribe";
            /// <summary>
            /// 订单完结后走打印模块
            /// </summary>
            public const string OrderPrintQueue = "OrderPrintQueue";
            /// <summary>
            /// 日志队列
            /// </summary>
            public const string LogingQueue = "LogingQueue";
            /// <summary>
            /// 短信发送队列
            /// </summary>
            public const string SmsQueue = "SmsQueue";
        }

        public static class Permissions
        {
            public const string Name = "Permission";

            /// <summary>
            /// 当前项目是否启用IDS4权限方案
            /// true：表示启动IDS4
            /// false：表示使用JWT
            public static bool IsUseIds4 = false;
        }
    }
}
