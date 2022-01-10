using Core.Net.Configuration;
using Core.Net.Entity.Dtos;
using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.Model.Goods;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Goods;
using Core.Net.Util.Extensions;
using Core.Net.Util.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Net.Web.Admin.Controllers.Goods
{

    /// <summary>
    /// 商品参数表
    /// </summary>
    [Description("商品参数表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CoreCmsGoodsParamsController : Controller
    {
        private readonly ICoreCmsGoodsParamsServices _coreCmsGoodsParamsServices;
        //private readonly ICoreCmsGoodsTypeParamsServices _coreCmsGoodsTypeParamsServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="coreCmsGoodsParamsServices"></param>
        /// <param name="coreCmsGoodsTypeParamsServices"></param>
        public CoreCmsGoodsParamsController(IWebHostEnvironment webHostEnvironment,
            ICoreCmsGoodsParamsServices coreCmsGoodsParamsServices
            /*, ICoreCmsGoodsTypeParamsServices coreCmsGoodsTypeParamsServices*/)
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsGoodsParamsServices = coreCmsGoodsParamsServices;
            //_coreCmsGoodsTypeParamsServices = coreCmsGoodsTypeParamsServices;
        }

        #region 首页数据============================================================

        // POST: Api/CoreCmsGoodsParams/GetIndex
        /// <summary>
        ///     首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public JsonResult GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            var goodsParamTypes = EnumHelper.EnumToList<GlobalEnumVars.GoodsParamTypes>();
            jm.data = new
            {
                goodsParamTypes
            };
            return new JsonResult(jm);
        }

        #endregion

        #region 获取列表============================================================

        // POST: Api/CoreCmsGoodsParams/GetPageList
        /// <summary>
        ///     获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<JsonResult> GetPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsGoodsParams>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsGoodsParams, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "name":
                    orderEx = p => p.name;
                    break;
                case "value":
                    orderEx = p => p.value;
                    break;
                case "type":
                    orderEx = p => p.type;
                    break;
                case "createTime":
                    orderEx = p => p.createTime;
                    break;
                case "updateTime":
                    orderEx = p => p.updateTime;
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

            //序列 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //参数名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) @where = @where.And(p => p.name.Contains(name));
            //参数值 nvarchar
            var value = Request.Form["value"].FirstOrDefault();
            if (!string.IsNullOrEmpty(value)) @where = @where.And(p => p.value.Contains(value));
            //参数类型 nvarchar
            var type = Request.Form["type"].FirstOrDefault();
            if (!string.IsNullOrEmpty(type)) @where = @where.And(p => p.type.Contains(type));
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
            //更新时间 datetime
            var updateTime = Request.Form["updateTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(updateTime))
            {
                if (updateTime.Contains("到"))
                {
                    var dts = updateTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.updateTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.updateTime < dtEnd);
                }
                else
                {
                    var dt = updateTime.ObjectToDate();
                    where = where.And(p => p.updateTime > dt);
                }
            }

            //获取数据
            var list = await _coreCmsGoodsParamsServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }

        #endregion

        #region 创建数据============================================================

        // POST: Api/CoreCmsGoodsParams/GetCreate
        /// <summary>
        ///     创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public JsonResult GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            var goodsParamTypes = EnumHelper.EnumToList<GlobalEnumVars.GoodsParamTypes>();
            jm.data = new
            {
                goodsParamTypes
            };
            return new JsonResult(jm);
        }

        #endregion

        #region 创建提交============================================================

        // POST: Api/CoreCmsGoodsParams/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<JsonResult> DoCreate([FromBody] CoreCmsGoodsParams entity)
        {
            var jm = new AdminUiCallBack();

            entity.createTime = DateTime.Now;
            var bl = await _coreCmsGoodsParamsServices.InsertAsync(entity) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;

            return new JsonResult(jm);
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/CoreCmsGoodsParams/GetEdit
        /// <summary>
        ///     编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<JsonResult> GetEdit([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsGoodsParamsServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            jm.code = 0;
            var goodsParamTypes = EnumHelper.EnumToList<GlobalEnumVars.GoodsParamTypes>();
            jm.data = new
            {
                model,
                goodsParamTypes
            };

            return new JsonResult(jm);
        }

        #endregion

        #region 编辑提交============================================================

        // POST: Admins/CoreCmsGoodsParams/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<JsonResult> DoEdit([FromBody] CoreCmsGoodsParams entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsGoodsParamsServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            //事物处理过程开始
            oldModel.name = entity.name;
            oldModel.value = entity.value;
            oldModel.type = entity.type;
            oldModel.updateTime = DateTime.Now;

            //事物处理过程结束
            var bl = await _coreCmsGoodsParamsServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/CoreCmsGoodsParams/DoDelete/10
        /// <summary>
        ///     单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<JsonResult> DoDelete([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsGoodsParamsServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return new JsonResult(jm);
            }

            var bl = await _coreCmsGoodsParamsServices.DeleteByIdAsync(entity.id);
            //if (bl) await _coreCmsGoodsTypeParamsServices.DeleteAsync(p => p.paramsId == entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return new JsonResult(jm);
        }

        #endregion
    }
}
