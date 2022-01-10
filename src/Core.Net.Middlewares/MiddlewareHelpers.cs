using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Middlewares
{
    /// <summary>
    /// 中间件
    /// </summary>
    public static class MiddlewareHelpers
    {
        /// <summary>
        /// 请求响应中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseReuestResponseLog(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequRespLogMildd>();
        }

        /// <summary>
        /// IP请求中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseIpLogMildd(this IApplicationBuilder app)
        {
            return app.UseMiddleware<IPLogMildd>();
        }

        ///// <summary>
        ///// 异常处理中间件（后端模式）
        ///// </summary>
        ///// <param name="app"></param>
        ///// <returns></returns>
        //public static IApplicationBuilder UseExceptionHandlerMiddForAdmin(this IApplicationBuilder app)
        //{
        //    return app.UseMiddleware<ExceptionHandlerMiddForAdmin>();
        //}

        ///// <summary>
        ///// 异常处理中间件（客户端）
        ///// </summary>
        ///// <param name="app"></param>
        ///// <returns></returns>
        //public static IApplicationBuilder UseExceptionHandlerMiddForClent(this IApplicationBuilder app)
        //{
        //    return app.UseMiddleware<ExceptionHandlerMiddForClent>();
        //}

        ///// <summary>
        ///// SignalR中间件
        ///// </summary>
        ///// <param name="app"></param>
        ///// <returns></returns>
        //public static IApplicationBuilder UseSignalRSendMildd(this IApplicationBuilder app)
        //{
        //    return app.UseMiddleware<SignalRSendMildd>();
        //}

        ///// <summary>
        ///// 用户访问接口日志中间件
        ///// </summary>
        ///// <param name="app"></param>
        ///// <returns></returns>
        //public static IApplicationBuilder UseRecordAccessLogsMildd(this IApplicationBuilder app)
        //{
        //    return app.UseMiddleware<RecordAccessLogsMildd>();
        //}

    }
}
