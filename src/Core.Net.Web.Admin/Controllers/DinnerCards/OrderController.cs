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
using Core.Net.Service.Goods;
using Core.Net.Service.Systems;
using Core.Net.Util.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Core.Net.Web.Admin.Controllers.DinnerCards
{

    /// <summary>
    ///     订单表
    /// </summary>
    [Description("订单表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class OrderController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDinnerCardServices _dinnerCardServices;
        private readonly ICoreOrderServices _coreOrderServices;

        private readonly IBusinessServices _businessServices;
        private readonly ICoreGoodsServices _coreGoodsServices;
        private readonly ISysDictionaryServices _sysDictionaryServices;
        private readonly ISysDictionaryDataServices _sysDictionaryDataServices;
        private readonly ICoreCmsGoodsCategoryServices _coreCmsGoodsCategoryServices;

        ///  <summary>
        ///  构造函数
        /// </summary>
        ///   <param name="webHostEnvironment"></param>
        /// <param name="coreCmsArticleTypeServices"></param>
        ///  <param name="coreCmsArticleServices"></param>
        public OrderController(
            IWebHostEnvironment webHostEnvironment,
            IDinnerCardServices dinnerCardServices,
            ISysDictionaryServices sysDictionaryServices,
            IBusinessServices businessServices,
             ICoreOrderServices coreOrderServices,
            ISysDictionaryDataServices sysDictionaryDataServices,
                  ICoreCmsGoodsCategoryServices coreCmsGoodsCategoryServices,
            ICoreGoodsServices coreGoodsServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _businessServices = businessServices;
            _sysDictionaryServices = sysDictionaryServices;
            _sysDictionaryDataServices = sysDictionaryDataServices;
            _dinnerCardServices = dinnerCardServices;
            _coreGoodsServices = coreGoodsServices;
            _coreOrderServices = coreOrderServices;
            _coreCmsGoodsCategoryServices = coreCmsGoodsCategoryServices;
        }
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
            var jm = new AdminUiCallBack { code = 0 };
            return Json(jm);
        }
        #endregion

        #region 获取列表============================================================
        // POST: Api/DinnerCard/GetPageList
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<JsonResult> GetPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreOrder>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreOrder, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "createTime":
                    orderEx = p => p.createTime;
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
            //查询筛选
   
            //获取数据
            var list = await _coreOrderServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);

            //返回数据

            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return Json(jm);
        }
        #endregion

    }
}
