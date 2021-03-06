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
using Core.Net.Service.Systems;
using Core.Net.Util.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Core.Net.Web.Admin.Controllers.DinnerCards
{

    /// <summary>
    /// 订单表
    /// </summary>
    [Description("订单表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CardOrderController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDinnerCardServices _dinnerCardServices;
        private readonly ICoreOrderServices _coreOrderServices;
        private readonly ISysRoleServices _sysRoleServices;
        private readonly ISysOrganizationServices _sysOrganizationServices;
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
        public CardOrderController(
            IWebHostEnvironment webHostEnvironment,
            IDinnerCardServices dinnerCardServices,
            ISysDictionaryServices sysDictionaryServices,
            IBusinessServices businessServices,
             ICoreOrderServices coreOrderServices,
            ISysRoleServices sysRoleServices,
            ISysDictionaryDataServices sysDictionaryDataServices,
            ISysOrganizationServices sysOrganizationServices,
            ICoreGoodsServices coreGoodsServices
            )
        {

            _webHostEnvironment = webHostEnvironment;
            _businessServices = businessServices;
            _sysRoleServices = sysRoleServices;
            _sysOrganizationServices = sysOrganizationServices;
            _sysDictionaryServices = sysDictionaryServices;
            _sysDictionaryDataServices = sysDictionaryDataServices;
            _dinnerCardServices = dinnerCardServices;
            _coreGoodsServices = coreGoodsServices;
            _coreOrderServices = coreOrderServices;
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
            //订单类型
            var dict = _sysDictionaryServices.QueryByClause(p => p.dictCode == "orderType");
            var dictData = new List<SysDictionaryData>();
            if (dict != null)
            {
                dictData = _sysDictionaryDataServices.QueryListByClause(p => p.dictId == dict.id);
            }
            //机构
            List<KV> orgkVs = new List<KV>();
            var organizations =  _sysOrganizationServices.Query();
            foreach (var item in organizations)
            {
                KV kV = new KV();
                kV.Key = item.id.ToString();
                kV.Value = item.organizationName.ToString();
                orgkVs.Add(kV);
            }

            ////角色
            //List<KV> rolekVs = new List<KV>();
            //var roleList =  _sysRoleServices.Query();
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

            where = where.And(p => p.orderType != (int)OrderTypeEnum.GoodOrder);

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
         
            var orderType = Request.Form["orderType"].FirstOrDefault().ObjectToInt(0);
            if (orderType > 0) @where = @where.And(p => p.orderType == orderType);

            var organizationId = Request.Form["organizationId"].FirstOrDefault().ObjectToInt(0);
            if (organizationId > 0) @where = @where.And(p => p.organizationId == organizationId);

            //var roleId = Request.Form["roleId"].FirstOrDefault().ObjectToInt(0);
            //if (orderType > 0) @where = @where.And(p => p.roleId == roleId);


            var text = Request.Form["text"].FirstOrDefault();
            if (!string.IsNullOrEmpty(text)) @where = @where.And(p => p.telePhone.Contains(text)|| p.orderNo.Contains(text)|| p.costOrderNo.Contains(text)|| p.userName.Contains(text));

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

            var model = await _coreOrderServices.QueryByIdAsync(entity.id, false);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            jm.code = 0;

            jm.data = new
            {
                model,
            };

            return new JsonResult(jm);
        }
        #endregion
    }

    public class KV
    {

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
