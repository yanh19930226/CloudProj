using Core.Net.Configuration;
using Core.Net.Entity.Dtos;
using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.Model.Systems;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Systems;
using Core.Net.Util.Extensions;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Net.Web.Admin.Controllers.Systems
{
    /// <summary>
    ///  数据字典表
    /// </summary>
    [Description("数据字典表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SysDictionaryController : Controller
    {
        private readonly ISysDictionaryDataServices _sysDictionaryDataServices;
        private readonly ISysDictionaryServices _sysDictionaryServices;
        /// <summary>
        ///  构造函数
        /// </summary>
        public SysDictionaryController(ISysDictionaryServices sysDictionaryServices, ISysDictionaryDataServices sysDictionaryDataServices)
        {
            _sysDictionaryServices = sysDictionaryServices;
            _sysDictionaryDataServices = sysDictionaryDataServices;
        }

        #region 首页数据============================================================

        // POST: Api/SysDictionary/GetIndex
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
            return Json(jm);
        }

        #endregion

        #region 获取列表============================================================

        // POST: Api/SysDictionary/GetPageList
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
            var where = PredicateBuilder.True<SysDictionary>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<SysDictionary, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "dictCode":
                    orderEx = p => p.dictCode;
                    break;
                case "dictName":
                    orderEx = p => p.dictName;
                    break;
                case "comments":
                    orderEx = p => p.comments;
                    break;
                case "sortNumber":
                    orderEx = p => p.sortNumber;
                    break;
                case "deleted":
                    orderEx = p => p.deleted;
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

            //字典id int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //字典标识 nvarchar
            var dictCode = Request.Form["dictCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(dictCode)) @where = @where.And(p => p.dictCode.Contains(dictCode));
            //字典名称 nvarchar
            var dictName = Request.Form["dictName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(dictName)) @where = @where.And(p => p.dictName.Contains(dictName));
            //备注 nvarchar
            var comments = Request.Form["comments"].FirstOrDefault();
            if (!string.IsNullOrEmpty(comments)) @where = @where.And(p => p.comments.Contains(comments));
            //排序号 int
            var sortNumber = Request.Form["sortNumber"].FirstOrDefault().ObjectToInt(0);
            if (sortNumber > 0) @where = @where.And(p => p.sortNumber == sortNumber);
            //是否删除,0否,1是 bit
            var deleted = Request.Form["deleted"].FirstOrDefault();
            if (!string.IsNullOrEmpty(deleted) && deleted.ToLowerInvariant() == "true")
                @where = @where.And(p => p.deleted);
            else if (!string.IsNullOrEmpty(deleted) && deleted.ToLowerInvariant() == "false")
                @where = @where.And(p => p.deleted == false);
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

            //修改时间 datetime
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
            var list = await _sysDictionaryServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return Json(jm);
        }

        #endregion

        #region 创建数据============================================================

        // POST: Api/SysDictionary/GetCreate
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
            return new JsonResult(jm);
        }

        #endregion

        #region 创建提交============================================================

        // POST: Api/SysDictionary/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<JsonResult> DoCreate([FromBody] SysDictionary entity)
        {
            var jm = new AdminUiCallBack();

            entity.createTime = DateTime.Now;

            var bl = await _sysDictionaryServices.InsertAsync(entity) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;

            return new JsonResult(jm);
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/SysDictionary/GetEdit
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

            var model = await _sysDictionaryServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            jm.code = 0;
            jm.data = model;

            return new JsonResult(jm);
        }

        #endregion

        #region 编辑提交============================================================

        // POST: Api/SysDictionary/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<JsonResult> DoEdit([FromBody] SysDictionary entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _sysDictionaryServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            //事物处理过程开始
            oldModel.dictCode = entity.dictCode;
            oldModel.dictName = entity.dictName;
            oldModel.comments = entity.comments;
            oldModel.sortNumber = entity.sortNumber;
            oldModel.updateTime = DateTime.Now;

            //事物处理过程结束
            var bl = await _sysDictionaryServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/SysDictionary/DoDelete/10
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

            var model = await _sysDictionaryServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return new JsonResult(jm);
            }

            //判断当前字典是否有字典项有就不能删除
            if (await _sysDictionaryDataServices.ExistsAsync(p => p.dictId == model.id))
            {
                jm.msg = GlobalConstVars.DeleteIsHaveChildren;
                return new JsonResult(jm);
            }

            var bl = await _sysDictionaryServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return new JsonResult(jm);

        }

        #endregion
    }
}
