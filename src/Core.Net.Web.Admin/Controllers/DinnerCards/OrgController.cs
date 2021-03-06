using Core.Net.Configuration;
using Core.Net.Entity.Dtos;
using Core.Net.Entity.Model.DinnerCard;
using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.Model.Systems;
using Core.Net.Entity.ViewModels;
using Core.Net.Filter;
using Core.Net.Service.DinnerCards;
using Core.Net.Service.Systems;
using Core.Net.Util.Extensions;
using Core.Net.Util.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Net.Web.Admin.Controllers.DinnerCards
{
    /// <summary>
    /// 部门表
    /// </summary>
    [Description("部门表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class OrgController : Controller
    {

        private readonly IBusinessServices _businessServices;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISysDictionaryServices _sysDictionaryServices;
        private readonly ISysDictionaryDataServices _sysDictionaryDataServices;
        private readonly ISysOrganizationServices _sysOrganizationServices;
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="coreCmsNoticeServices"></param>
        public OrgController(IWebHostEnvironment webHostEnvironment,
             ISysDictionaryServices sysDictionaryServices,
      ISysDictionaryDataServices sysDictionaryDataServices,
      ISysOrganizationServices sysOrganizationServices,
        IBusinessServices businessServices)
        {
            _sysOrganizationServices = sysOrganizationServices;
            _sysDictionaryServices = sysDictionaryServices;
            _sysDictionaryDataServices = sysDictionaryDataServices;
            _webHostEnvironment = webHostEnvironment;
            _businessServices = businessServices;
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
            var where = PredicateBuilder.True<SysOrganization>();
            //标题 nvarchar
            var organizationName = Request.Form["organizationName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(organizationName)) @where = @where.And(p => p.organizationName.Contains(organizationName));
            //获取数据
            var list = _sysOrganizationServices.QueryListByClause(where).ToList();
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.Count;
            jm.msg = "数据调用成功!";
            return Json(jm);
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
            return Json(jm);
        }
        #endregion

        #region 创建数据============================================================
        // POST: Api/CoreCmsArticleType/GetCreate
        /// <summary>
        /// 创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public AdminUiCallBack GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            var dict = _sysDictionaryServices.QueryByClause(p => p.dictCode == "organization_type");
            var dictData = new List<SysDictionaryData>();
            if (dict != null)
            {
                dictData = _sysDictionaryDataServices.QueryListByClause(p => p.dictId == dict.id);
            }
            jm.data = new
            {
                dictData
            };

            return jm;
        }
        #endregion

        #region 创建提交============================================================
        // POST: Api/CoreCmsArticleType/DoCreate
        /// <summary>
        /// 创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<JsonResult> DoCreate([FromBody] SysOrganization entity)
        {
            var jm = new AdminUiCallBack();
            entity.createTime = DateTime.Now;
            var bl = await _sysOrganizationServices.InsertAsync(entity) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = (bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure);

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
        public async Task<JsonResult> GetEdit([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _sysOrganizationServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            jm.code = 0;
            jm.data = new
            {
                model
            };

            return new JsonResult(jm);
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
        public async Task<JsonResult> DoEdit([FromBody] SysOrganization entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _sysOrganizationServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            //事物处理过程开始
            oldModel.id = entity.id;
            oldModel.organizationName = entity.organizationName;
            //事物处理过程结束
            var bl = await _sysOrganizationServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
        }
        #endregion

        #region 删除数据============================================================
        // POST: Api/CoreCmsArticleType/DoDelete/10
        /// <summary>
        /// 单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<JsonResult> DoDelete([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await _sysOrganizationServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return new JsonResult(jm);
        }
        #endregion

    }
}
