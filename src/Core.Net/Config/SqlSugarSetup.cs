using Core.Net.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Config
{
    /// <summary>
    /// SqlSugar 启动服务
    /// </summary>
    public static class SqlSugarSetup
    {
        public static void AddSqlSugarSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            string connectionString = AppSettingsConstVars.DbSqlConnection;
            string dbTypeString = AppSettingsConstVars.DbDbType;

            //获取数据类型
            var dbType = dbTypeString == DbType.MySql.ToString() ? DbType.MySql : DbType.SqlServer;

            ////判断是否开启redis设置二级缓存方式
            //ICacheService myCache = AppSettingsConstVars.RedisConfigEnabled
            //    ? (ICacheService)new SqlSugarRedisCache()
            //    : new SqlSugarMemoryCache();

            var connectionConfig = new ConnectionConfig()
            {
                ConnectionString = connectionString, //必填
                DbType = dbType, //必填
                IsAutoCloseConnection = false,
                InitKeyType = InitKeyType.Attribute,

                //ConfigureExternalServices = new ConfigureExternalServices()
                //{
                //    DataInfoCacheService = myCache
                //},
            };

            services.AddScoped<ISqlSugarClient>(o =>
            {
                var db = new SqlSugarClient(connectionConfig); //默认SystemTable

                #region Db

                //日志处理
                ////SQL执行前 可以修改SQL
                //db.Aop.OnLogExecuting = (sql, pars) =>
                //{
                //    //获取sql
                //    Console.WriteLine(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                //    Console.WriteLine();

                //    //通过TempItems这个变量来算出这个SQL执行时间（1）
                //    if (db.TempItems == null) db.TempItems = new Dictionary<string, object>();
                //    db.TempItems.Add("logTime", DateTime.Now);
                //    //通过TempItems这个变量来算出这个SQL执行时间（2）
                //    var startingTime = db.TempItems["logTime"];
                //    db.TempItems.Remove("time");
                //    var completedTime = DateTime.Now;
                //};
                //db.Aop.OnLogExecuted = (sql, pars) => //SQL执行完事件
                //{
                //};
                //db.Aop.OnLogExecuting = (sql, pars) => //SQL执行前事件
                //{
                //};
                //db.Aop.OnError = (exp) =>//执行SQL 错误事件
                //{
                //    NLogUtil.WriteFileLog(NLog.LogLevel.Error, LogType.Other, "SqlSugar", "执行SQL错误事件", exp);
                //}; 

                #endregion

                return db;
            });

        }
    }
}
