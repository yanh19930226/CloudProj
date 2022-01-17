using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Net.Configuration;
using Core.Net.Entity.Dtos;
using Core.Net.Entity.Enums;
using Core.Net.Entity.Model.DinnerCard;
using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.Model.Systems;
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
        private readonly ICoreGoodOrderDetailServices _coreGoodOrderDetailServices;
        private readonly ISysRoleServices _sysRoleServices;
        private readonly ISysOrganizationServices _sysOrganizationServices;
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
             ISysRoleServices sysRoleServices,
              ISysOrganizationServices sysOrganizationServices,
            ISysDictionaryDataServices sysDictionaryDataServices,
             ICoreOrderServices coreOrderServices,
            ICoreCmsGoodsCategoryServices coreCmsGoodsCategoryServices,
            ICoreGoodOrderDetailServices coreGoodOrderDetailServices,
            ICoreGoodsServices coreGoodsServices
            )
        {
            _sysRoleServices = sysRoleServices;
            _sysOrganizationServices = sysOrganizationServices;
            _webHostEnvironment = webHostEnvironment;
            _businessServices = businessServices;
            _sysDictionaryServices = sysDictionaryServices;
            _sysDictionaryDataServices = sysDictionaryDataServices;
            _dinnerCardServices = dinnerCardServices;
            _coreGoodsServices = coreGoodsServices;
            _coreOrderServices = coreOrderServices;
            _coreGoodOrderDetailServices = coreGoodOrderDetailServices;
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
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            var dict = _sysDictionaryServices.QueryByClause(p => p.dictCode == "orderStatus");
            var dictData = new List<SysDictionaryData>();
            if (dict != null)
            {
                dictData = _sysDictionaryDataServices.QueryListByClause(p => p.dictId == dict.id);
            }
            //机构
            List<KV> orgkVs = new List<KV>();
            var organizations = _sysOrganizationServices.Query();
            foreach (var item in organizations)
            {
                KV kV = new KV();
                kV.Key = item.id.ToString();
                kV.Value = item.organizationName.ToString();
                orgkVs.Add(kV);
            }

            ////角色
            //List<KV> rolekVs = new List<KV>();
            //var roleList = _sysRoleServices.Query();
            //foreach (var item in roleList)
            //{
            //    KV kV = new KV();
            //    kV.Key = item.id.ToString();
            //    kV.Value = item.roleName.ToString();
            //    rolekVs.Add(kV);
            //}
            jm.data = new
            {
                dictData,
                orgkVs,
                //rolekVs
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
            var where = PredicateBuilder.True<CoreOrder>();
            where = where.And(p => p.orderType == (int)OrderTypeEnum.GoodOrder);
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

            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0) @where = @where.And(p => p.status == status);

            var organizationId = Request.Form["organizationId"].FirstOrDefault().ObjectToInt(0);
            if (organizationId > 0) @where = @where.And(p => p.organizationId == organizationId);

            var text = Request.Form["text"].FirstOrDefault();
            if (!string.IsNullOrEmpty(text)) @where = @where.And(p => p.orderNo.Contains(text) || p.userName.Contains(text)|| p.telePhone.Contains(text));

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

            var list = await _coreOrderServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);

            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return Json(jm);
        }
        #endregion

        #region 预览数据============================================================
        // POST: Api/CoreCmsAgentGoods/GetDetails/10
        /// <summary>
        /// 预览数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("预览数据")]
        public async Task<JsonResult> GetDetails([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var order = await _coreOrderServices.QueryByClauseAsync(p => p.id == entity.id);
           
            if (order == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            var detail = await _coreGoodOrderDetailServices.QueryListByClauseAsync(p => p.orderNo == order.orderNo);

            jm.code = 0;

            jm.data = new
            {
                order,
                detail
            };
            return new JsonResult(jm);
        }
        #endregion

        #region 取消订单============================================================
        // POST: Api/CoreCmsArticleType/DoDelete/10
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("取消订单")]
        public async Task<JsonResult> DoCancel([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();
            var model = await _coreOrderServices.QueryByClauseAsync(p => p.id == entity.id);
            model.status = (int)OrderStatusEnum.Canceled;
            //事物处理过程结束
            var bl = await _coreOrderServices.UpdateAsync(model);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            return new JsonResult(jm);
        }
        #endregion

        #region 确认收货============================================================
        // POST: Api/CoreCmsArticleType/DoDelete/10
        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("确认收货")]
        public async Task<JsonResult> DoConfirm([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();
            var model = await _coreOrderServices.QueryByClauseAsync(p=>p.id==entity.id);
            model.status = (int)OrderStatusEnum.Complete;
            //事物处理过程结束
            var bl = await _coreOrderServices.UpdateAsync(model);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            return new JsonResult(jm);
        }
        #endregion

        #region 批量确认============================================================

        // POST: Api/CoreCmsGoods/DoBatchDelete/10,11,20
        /// <summary>
        ///     批量确认
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量确认")]
        public async Task<JsonResult> DoBatchConfirm([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            var list = await _coreOrderServices.QueryListByClauseAsync(p => entity.id.Contains(p.id));
            foreach (var item in list)
            {
                item.status = (int)OrderStatusEnum.Complete;
            }
            var bl = await _coreOrderServices.UpdateAsync(list);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? "确认成功" : "确认失败";
            return new JsonResult(jm);
        }

        #endregion

        #region 批量取消============================================================

        // POST: Api/CoreCmsGoods/DoBatchMarketableUp/10,11,20
        /// <summary>
        ///     批量取消
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量取消")]
        public async Task<JsonResult> DoBatchCancel([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            var list =await  _coreOrderServices.QueryListByClauseAsync(p => entity.id.Contains(p.id));
            foreach (var item in list)
            {
                item.status =(int)OrderStatusEnum.Canceled;
            }
            var bl = await _coreOrderServices.UpdateAsync(list);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? "取消成功" : "取消失败";
            return new JsonResult(jm);
        }

        #endregion

    }
}
