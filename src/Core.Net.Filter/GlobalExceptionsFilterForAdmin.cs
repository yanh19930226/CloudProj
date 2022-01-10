using Core.Net.Entity.ViewModels;
using Core.Net.Loging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Core.Net.Filter
{
    /// <summary>
    /// 接口全局异常错误日志
    /// </summary>
    public class GlobalExceptionsFilterForAdmin : IExceptionFilter
    {

        public void OnException(ExceptionContext context)
        {

            NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.Web, "全局异常", "全局捕获异常", context.Exception);

            HttpStatusCode status = HttpStatusCode.InternalServerError;

            //处理各种异常
            var jm = new AdminUiCallBack();
            jm.code = (int)status;
            jm.msg = "发生了全局异常请联系管理员";
            jm.data = context.Exception;
            context.ExceptionHandled = true;
            context.Result = new ObjectResult(jm);
        }

    }
}
