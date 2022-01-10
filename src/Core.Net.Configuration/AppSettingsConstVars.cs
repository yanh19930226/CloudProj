using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Configuration
{
    public class AppSettingsConstVars
    {

        #region 全局地址================================================================================
        /// <summary>
        /// 系统后端地址
        /// </summary>
        public static readonly string AppConfigAppUrl = AppSettingsHelper.GetContent("AppConfig", "AppUrl");
        /// <summary>
        /// 系统接口地址
        /// </summary>
        public static readonly string AppConfigAppInterFaceUrl = AppSettingsHelper.GetContent("AppConfig", "AppInterFaceUrl");
        #endregion

        #region 数据库================================================================================
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        public static readonly string DbSqlConnection = AppSettingsHelper.GetContent("ConnectionStrings", "SqlConnection");
        /// <summary>
        /// 获取数据库类型
        /// </summary>
        public static readonly string DbDbType = AppSettingsHelper.GetContent("ConnectionStrings", "DbType");
        #endregion

        #region redis================================================================================

        /// <summary>
        /// 获取redis是否开启
        /// </summary>
        public static readonly bool RedisConfigEnabled = AppSettingsHelper.GetContent("RedisConfig", "Enabled").ObjToBool();
        /// <summary>
        /// 获取redis连接字符串
        /// </summary>
        public static readonly string RedisConfigConnectionString = AppSettingsHelper.GetContent("RedisConfig", "ConnectionString");
        /// <summary>
        /// 获取是否开启使用redis缓存
        /// </summary>
        public static readonly bool RedisConfigUseRedisCache = AppSettingsHelper.GetContent("RedisConfig", "UseRedisCache").ObjToBool();
        /// <summary>
        /// 获取是否开启使用redis队列
        /// </summary>
        public static readonly bool RedisConfigUseRedisMessageQueue = AppSettingsHelper.GetContent("RedisConfig", "UseRedisMessageQueue").ObjToBool();

        #endregion

        #region swagger================================================================================

        /// <summary>
        /// Enabled
        /// </summary>
        public static readonly bool Enabled = AppSettingsHelper.GetContent("Swagger", "Enabled").ObjToBool();

        /// <summary>
        /// Version
        /// </summary>
        public static readonly string Version = AppSettingsHelper.GetContent("Swagger", "Version");

        /// <summary>
        /// Title
        /// </summary>
        public static readonly string Title = AppSettingsHelper.GetContent("Swagger", "Title");

        /// <summary>
        /// Description
        /// </summary>
        public static readonly string Description = AppSettingsHelper.GetContent("Swagger", "Description");

        /// <summary>
        /// ContactEmail
        /// </summary>
        public static readonly string ContactEmail = AppSettingsHelper.GetContent("Swagger", "ContactEmail");

        /// <summary>
        /// ContactName
        /// </summary>
        public static readonly string ContactName = AppSettingsHelper.GetContent("Swagger", "ContactName");

        /// <summary>
        /// ContactUrl
        /// </summary>
        public static readonly string ContactUrl = AppSettingsHelper.GetContent("Swagger", "ContactUrl");

        #endregion

        #region Cors跨域设置================================================================================
        public static readonly string CorsPolicyName = AppSettingsHelper.GetContent("Cors", "PolicyName");
        public static readonly bool CorsEnableAllIPs = AppSettingsHelper.GetContent("Cors", "EnableAllIPs").ObjToBool();
        public static readonly string CorsIPs = AppSettingsHelper.GetContent("Cors", "IPs");
        #endregion

        #region Middleware中间件================================================================================
        /// <summary>
        /// Ip限流
        /// </summary>
        public static readonly bool MiddlewareIpLogEnabled = AppSettingsHelper.GetContent("Middleware", "IPLog", "Enabled").ObjToBool();
        /// <summary>
        /// 记录请求与返回数据
        /// </summary>
        public static readonly bool MiddlewareRequestResponseLogEnabled = AppSettingsHelper.GetContent("Middleware", "RequestResponseLog", "Enabled").ObjToBool();
        /// <summary>
        /// 用户访问记录-是否开启
        /// </summary>
        public static readonly bool MiddlewareRecordAccessLogsEnabled = AppSettingsHelper.GetContent("Middleware", "RecordAccessLogs", "Enabled").ObjToBool();
        /// <summary>
        /// 用户访问记录-过滤ip
        /// </summary>
        public static readonly string MiddlewareRecordAccessLogsIgnoreApis = AppSettingsHelper.GetContent("Middleware", "RecordAccessLogs", "IgnoreApis");

        #endregion

        #region Jwt授权配置================================================================================

        public static readonly string JwtConfigSecretKey = AppSettingsHelper.GetContent("JwtConfig", "SecretKey");
        public static readonly string JwtConfigIssuer = AppSettingsHelper.GetContent("JwtConfig", "Issuer");
        public static readonly string JwtConfigAudience = AppSettingsHelper.GetContent("JwtConfig", "Audience");
        #endregion

    }
}
