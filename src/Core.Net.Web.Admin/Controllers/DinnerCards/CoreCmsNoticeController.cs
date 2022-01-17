using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Net.Configuration;
using Core.Net.Entity.Dtos;
using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.Model.Shops;
using Core.Net.Entity.Model.Systems;
using Core.Net.Entity.ViewModels;
using Core.Net.Filter;
using Core.Net.Service.Shops;
using Core.Net.Service.Systems;
using Core.Net.Util.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Core.Net.Web.Admin.Controllers.DinnerCards
{
    /// <summary>
    ///     公告表
    /// </summary>
    [Description("公告表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsNoticeController : ControllerBase
    {
        private readonly ICoreCmsNoticeServices _coreCmsNoticeServices;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISysDictionaryServices _sysDictionaryServices;
        private readonly ISysDictionaryDataServices _sysDictionaryDataServices;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="coreCmsNoticeServices"></param>
        public CoreCmsNoticeController(IWebHostEnvironment webHostEnvironment,
            ISysDictionaryServices sysDictionaryServices,
            ISysDictionaryDataServices sysDictionaryDataServices,
            ICoreCmsNoticeServices coreCmsNoticeServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _sysDictionaryServices = sysDictionaryServices;
            _sysDictionaryDataServices = sysDictionaryDataServices;
            _coreCmsNoticeServices = coreCmsNoticeServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsNotice/GetPageList
        /// <summary>
        ///     获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<AdminUiCallBack> GetPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsNotice>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsNotice, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "title":
                    orderEx = p => p.title;
                    break;
                case "contentBody":
                    orderEx = p => p.contentBody;
                    break;
                case "sort":
                    orderEx = p => p.sort;
                    break;
                case "isDel":
                    orderEx = p => p.isDel;
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
            var noticeType = Request.Form["noticeType"].FirstOrDefault().ObjectToInt(0);
            if (noticeType > 0) @where = @where.And(p => p.noticeType == noticeType);
            //公告标题 nvarchar
            var title = Request.Form["title"].FirstOrDefault();
            if (!string.IsNullOrEmpty(title)) @where = @where.And(p => p.title.Contains(title));
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
            var list = await _coreCmsNoticeServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/CoreCmsNotice/GetIndex
        /// <summary>
        ///     首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public AdminUiCallBack GetIndex()
        {

            var jm = new AdminUiCallBack { code = 0 };

            //返回数据
            var dict = _sysDictionaryServices.QueryByClause(p => p.dictCode == "noticeType");
            var dictData = new List<SysDictionaryData>();
            if (dict != null)
            {
                dictData = _sysDictionaryDataServices.QueryListByClause(p => p.dictId == dict.id);
            }

            jm.data = new
            {
                dictData,
            };

            return jm;
        }

        #endregion

        #region 创建数据============================================================

        // POST: Api/CoreCmsNotice/GetCreate
        /// <summary>
        ///     创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public AdminUiCallBack GetCreate()
        {
            //返回数据
            var dict =  _sysDictionaryServices.QueryByClause(p => p.dictCode == "noticeType");
            var dictData = new List<SysDictionaryData>();
            if (dict != null)
            {
                dictData =  _sysDictionaryDataServices.QueryListByClause(p => p.dictId == dict.id);
            }
            //返回数据
            var jm = new AdminUiCallBack { code = 0,data = new {  dictData }};
            return jm;
        }

        #endregion

        #region 创建提交============================================================

        // POST: Api/CoreCmsNotice/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] CoreCmsNotice entity)
        {
            var jm = new AdminUiCallBack();

            entity.createTime = DateTime.Now;
            entity.isDel = false;

            var bl = await _coreCmsNoticeServices.InsertAsync(entity) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;

            return jm;
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/CoreCmsNotice/GetEdit
        /// <summary>
        ///     编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<AdminUiCallBack> GetEdit([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            //返回数据
            var dict = _sysDictionaryServices.QueryByClause(p => p.dictCode == "noticeType");
            var dictData = new List<SysDictionaryData>();
            if (dict != null)
            {
                dictData = _sysDictionaryDataServices.QueryListByClause(p => p.dictId == dict.id);
            }


            var model = await _coreCmsNoticeServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            jm.code = 0;
            jm.data = new {
                dictData,
                model
            };

            return jm;
        }

        #endregion

        #region 编辑提交============================================================

        // POST: Admins/CoreCmsNotice/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsNotice entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsNoticeServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            oldModel.id = entity.id;
            oldModel.noticeType = entity.noticeType;
            oldModel.title = entity.title;
            oldModel.contentBody = entity.contentBody;
            oldModel.sort = entity.sort;
            oldModel.isDel = entity.isDel;
            oldModel.createTime = DateTime.Now;

            //事物处理过程结束
            var bl = await _coreCmsNoticeServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;


            return jm;
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/CoreCmsNotice/DoDelete/10
        /// <summary>
        ///     单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<AdminUiCallBack> DoDelete([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsNoticeServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }

            var bl = await _coreCmsNoticeServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return jm;

        }

        #endregion

        #region 批量删除============================================================

        // POST: Api/CoreCmsNotice/DoBatchDelete/10,11,20
        /// <summary>
        ///     批量删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量删除")]
        public async Task<AdminUiCallBack> DoBatchDelete([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await _coreCmsNoticeServices.DeleteByIdsAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;


            return jm;
        }

        #endregion

        #region 设置软删除============================================================

        // POST: Api/CoreCmsNotice/DoSetisDel/10
        /// <summary>
        ///     设置软删除位  有时间代表已删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置软删除位  有时间代表已删除")]
        public async Task<AdminUiCallBack> DoSetisDel([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsNoticeServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            oldModel.isDel = entity.data;

            var bl = await _coreCmsNoticeServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion
    }
}
