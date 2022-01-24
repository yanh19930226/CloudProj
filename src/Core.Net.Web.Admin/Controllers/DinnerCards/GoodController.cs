using Core.Net.Configuration;
using Core.Net.Entity.Dtos;
using Core.Net.Entity.Model.DinnerCard;
using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.Model.Systems;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.DinnerCards;
using Core.Net.Service.Impl.DinnerCards;
using Core.Net.Service.Systems;
using Core.Net.Util.Extensions;
using Core.Net.Util.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Web.Admin.Controllers.DinnerCards
{
    /// <summary>
    ///商品管理
    /// </summary>
    [Description("商品管理")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GoodController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDinnerCardServices _dinnerCardServices;

        private readonly IBusinessServices _businessServices;
        private readonly ICoreGoodsServices _coreGoodsServices;
        private readonly ISysDictionaryServices _sysDictionaryServices;
        private readonly ISysDictionaryDataServices _sysDictionaryDataServices;

        ///  <summary>
        ///  构造函数
        /// </summary>
        ///   <param name="webHostEnvironment"></param>
        /// <param name="coreCmsArticleTypeServices"></param>
        ///  <param name="coreCmsArticleServices"></param>
        public GoodController(
            IWebHostEnvironment webHostEnvironment,
            IDinnerCardServices dinnerCardServices,
            ISysDictionaryServices sysDictionaryServices,
            IBusinessServices businessServices,
            ISysDictionaryDataServices sysDictionaryDataServices,
            ICoreGoodsServices coreGoodsServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _businessServices = businessServices;
            _sysDictionaryServices = sysDictionaryServices;
            _sysDictionaryDataServices = sysDictionaryDataServices;
            _dinnerCardServices = dinnerCardServices;
            _coreGoodsServices = coreGoodsServices;
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
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            var business = _businessServices.Query();

            var dict2 = _sysDictionaryServices.QueryByClause(p => p.dictCode == "goodStatus");
            var dictData2 = new List<SysDictionaryData>();
            if (dict2 != null)
            {
                dictData2 = _sysDictionaryDataServices.QueryListByClause(p => p.dictId == dict2.id);
            }

            jm.data = new
            {
                business,
                dictData2
            };
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
            var where = PredicateBuilder.True<CoreGoods>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreGoods, object>> orderEx;
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
            var text = Request.Form["text"].FirstOrDefault();
            if (!string.IsNullOrEmpty(text)) @where = @where.And(p => p.goodName.Contains(text));
            var goodstatus = Request.Form["goodstatus"].FirstOrDefault().ObjectToInt(0);
            if (goodstatus > 0) @where = @where.And(p => p.status == goodstatus);
            //商家
            var businessid = Request.Form["businessid"].FirstOrDefault().ObjectToInt(0);
            if (businessid > 0) @where = @where.And(p => p.businessId == businessid);
            //创建时间 datetime
            var createTime = Request.Form["createTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(createTime))
            {
                if (createTime.Contains("到"))
                {
                    var dts = createTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.createTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.createTime < dtEnd);
                }
                else
                {
                    var dt = createTime.ObjectToDate();
                    where = where.And(p => p.createTime > dt);
                }
            }
            //获取数据
            var list = await _coreGoodsServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return Json(jm);
        }
        #endregion

        #region 创建数据============================================================

        // POST: Api/SysRole/GetCreate
        /// <summary>
        ///     创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public async Task<AdminUiCallBack> GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
           var busi =   _businessServices.QueryListByClause(p => p.status == true, p => p.id, OrderByType.Desc, true);
            var dict2 = await _sysDictionaryServices.QueryByClauseAsync(p => p.dictCode == "goodStatus");
            var dictData2 = new List<SysDictionaryData>();
            if (dict2 != null)
            {
                dictData2 = await _sysDictionaryDataServices.QueryListByClauseAsync(p => p.dictId == dict2.id);
            }
            jm.data = new
            {
                busi,
                dictData2
            };

            return jm;
        }

        #endregion

        #region 创建提交============================================================

        // POST: Api/SysRole/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<JsonResult> DoCreate([FromBody] CoreGoods entity)
        {
            var aurl = HttpRequestExtensions.GetAbsoluteUri(Request);
            var jm = new AdminUiCallBack();
            entity.createTime = DateTime.Now;
            entity.absUrl = aurl + entity.url;
            entity.goodNo = RandomNumber.GetRandomProduct();
            entity.businessName = _businessServices.QueryByClause(p => p.id == entity.businessId).businessName;
            var bl = await _coreGoodsServices.InsertAsync(entity) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;

            return new JsonResult(jm);
        }

        #endregion

        #region 编辑数据============================================================
        // POST: Api/CoreCmsArticleType/GetEdit
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async  Task<AdminUiCallBack> GetEdit([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var busi = _businessServices.QueryListByClause(p => p.status == true, p => p.id, OrderByType.Desc, true);

            //获取商品分类
            var model = await _coreGoodsServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            var dict2 = await _sysDictionaryServices.QueryByClauseAsync(p => p.dictCode == "goodStatus");
            var dictData2 = new List<SysDictionaryData>();
            if (dict2 != null)
            {
                dictData2 = await _sysDictionaryDataServices.QueryListByClauseAsync(p => p.dictId == dict2.id);
            }

            jm.code = 0;
            jm.data = new
            {
                model,
                dictData2,
                busi
            };

            return jm;
        }
        #endregion

        #region 编辑提交============================================================
        // POST: Admins/CoreCmsArticleType/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<JsonResult> DoEdit([FromBody] CoreGoods entity)
        {
            var jm = new AdminUiCallBack();

            var absurl = HttpRequestExtensions.GetAbsoluteUri(Request);

            var oldModel = await _coreGoodsServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            //事物处理过程开始
            oldModel.id = entity.id;
            oldModel.goodName = entity.goodName;
            oldModel.businessId = entity.businessId;
            oldModel.businessName = _businessServices.QueryByClause(p=>p.id== entity.businessId).businessName;
            oldModel.status = entity.status;
            oldModel.unitPrice = entity.unitPrice;
            oldModel.url = entity.url;
            oldModel.absUrl = absurl+ entity.url;
            oldModel.stock = entity.stock;
            oldModel.avilableTime = entity.avilableTime;
            //事物处理过程结束
            var bl = await _coreGoodsServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
        }
        #endregion

        #region 删除数据============================================================
        // POST: Api/CoreCmsArticleType/DoDelete/10
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("删除数据")]
        public async Task<JsonResult> DoDelete([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreGoodsServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return new JsonResult(jm);
            }
            //执行删除
            var bl = await _coreGoodsServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return new JsonResult(jm);
        }
        #endregion

    }


    public static class HttpRequestExtensions
    {
        public static string GetAbsoluteUri(this HttpRequest request)
        {
            return new StringBuilder()
             .Append(request.Scheme)
             .Append("://")
             .Append(request.Host)
             //.Append(request.PathBase)
             //.Append(request.Path)
             //.Append(request.QueryString)
             .ToString();
        }
    }
}
