using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Net.Configuration;
using Core.Net.Entity.Model.DinnerCard;
using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.ViewModels;
using Core.Net.Filter;
using Core.Net.Service.DinnerCards;
using Core.Net.Util.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Core.Net.Web.Admin.Controllers.DinnerCards
{
    /// <summary>
    /// 商家金额表
    /// </summary>
    [Description("商家金额表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class BusinessOrderController : Controller
    {

        private readonly IBusinessOrderServices _businessOrderServices;
        private readonly IBusinessServices _businessServices;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BusinessOrderController(IBusinessOrderServices businessOrderServices, IWebHostEnvironment webHostEnvironment, IBusinessServices businessServices)
        {
            _businessOrderServices = businessOrderServices;
            _businessServices = businessServices;
            _webHostEnvironment = webHostEnvironment;
        }
        #region 获取列表============================================================
        // POST: Api/CoreCmsArticleType/GetPageList
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public JsonResult GetPageList()
        {
            var jm = new AdminUiCallBack();
            var where = PredicateBuilder.True<BusinessOrder>();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<BusinessOrder, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "createTime":
                    orderEx = p => p.createtime;
                    break;
                default:
                    orderEx = p => p.id;
                    break;
            }
            //设置排序方式
            var orderDirection = Request.Form["orderDirection"].FirstOrDefault();
            var orderBy = orderDirection switch
            {
                "asc" => OrderByType.Asc,
                "desc" => OrderByType.Desc,
                _ => OrderByType.Desc
            };
            var businessId = Request.Form["businessId"].FirstOrDefault().ObjectToInt(0);
            if (businessId > 0) @where = @where.And(p => p.businessid == businessId);

            var ordertype = Request.Form["ordertype"].FirstOrDefault().ObjectToInt(0);
            if (ordertype > 0) @where = @where.And(p => p.ordertype == ordertype);
            var createTime = Request.Form["createTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(createTime))
            {
                if (createTime.Contains("到"))
                {
                    var dts = createTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.createtime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.createtime < dtEnd);
                }
                else
                {
                    var dt = createTime.ObjectToDate();
                    where = where.And(p => p.createtime > dt);
                }
            }
            //获取数据
            var list = _businessOrderServices.QueryListByClause(where).OrderByDescending(p => p.createtime).ToList();
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/CoreCmsArticleType/GetIndex
        /// <summary>
        /// 首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public JsonResult GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            var business = _businessServices.Query();
            jm.data = new
            {
                business
            };
            return new JsonResult(jm);
        }
        #endregion
    }
}
