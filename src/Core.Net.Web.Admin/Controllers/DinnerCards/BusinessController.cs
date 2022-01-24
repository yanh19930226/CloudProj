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
using Core.Net.Util.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Net.Web.Admin.Controllers.DinnerCards
{
    /// <summary>
    ///     商家表
    /// </summary>
    [Description("商家表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class BusinessController : Controller
    {
        private readonly IBusinessOrderServices _businessOrderServices;
        private readonly IBusinessServices _businessServices;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISysDictionaryServices _sysDictionaryServices;
        private readonly ISysDictionaryDataServices _sysDictionaryDataServices;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="coreCmsNoticeServices"></param>
        public BusinessController(IWebHostEnvironment webHostEnvironment,
            IBusinessOrderServices businessOrderServices,
             ISysDictionaryServices sysDictionaryServices,
      ISysDictionaryDataServices sysDictionaryDataServices,
        IBusinessServices businessServices)
        {
            _businessOrderServices = businessOrderServices;
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

            var where = PredicateBuilder.True<Business>();
            //标题 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) @where = @where.And(p => p.businessName.Contains(name));
            //获取数据
            var list = _businessServices.QueryListByClause(where).OrderByDescending(p => p.createTime).ToList();
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

            //返回数据
            var dict = _sysDictionaryServices.QueryByClause(p => p.dictCode == "payMode");
            var dictData = new List<SysDictionaryData>();
            if (dict != null)
            {
                dictData = _sysDictionaryDataServices.QueryListByClause(p => p.dictId == dict.id);
            }
            jm.data = new
            {
                dictData,
            };
            return new JsonResult(jm);
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
            var dict = _sysDictionaryServices.QueryByClause(p => p.dictCode == "payMode");
            var dictData = new List<SysDictionaryData>();
            if (dict != null)
            {
                dictData = _sysDictionaryDataServices.QueryListByClause(p => p.dictId == dict.id);
            }
            //返回数据
            var jm = new AdminUiCallBack { code = 0, data = new { dictData } };
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
        public async Task<JsonResult> DoCreate([FromBody] Business entity)
        {
            var jm = new AdminUiCallBack();
            entity.createTime = DateTime.Now;
            var bl = await _businessServices.InsertAsync(entity) > 0;
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

            //返回数据
            var dict = _sysDictionaryServices.QueryByClause(p => p.dictCode == "payMode");
            var dictData = new List<SysDictionaryData>();
            if (dict != null)
            {
                dictData = _sysDictionaryDataServices.QueryListByClause(p => p.dictId == dict.id);
            }
            var model = await _businessServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            jm.code = 0;
            jm.data = new
            {
                model,
                dictData
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
        public async Task<JsonResult> DoEdit([FromBody] Business entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _businessServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            //事物处理过程开始
            oldModel.id = entity.id;
            oldModel.businessName = entity.businessName;
            oldModel.sort = entity.sort;
            oldModel.status = entity.status;

            //事物处理过程结束
            var bl = await _businessServices.UpdateAsync(oldModel);
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
           
            var bl = await _businessServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return new JsonResult(jm);
        }
        #endregion

        #region 设置是否启用============================================================

        // POST: Api/CoreCmsBrand/DoSetisShow/10
        /// <summary>
        ///     设置是否启用
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否启用")]
        public async Task<AdminUiCallBack> DoSetisStatus([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _businessServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.status = entity.data;
            oldModel.createTime = DateTime.Now;
            var bl = await _businessServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion

        #region 商家提现============================================================

        // POST: Api/CoreCmsUser/GetEditBalance
        /// <summary>
        ///     商家提现
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("商家提现")]
        public async Task<JsonResult> GetCash([FromBody] FMIntId entity)
        {
            //返回数据
            var jm = new AdminUiCallBack();
            var model = await _businessServices.QueryByIdAsync(entity.id);
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

        #region 商家提现提交============================================================

        // POST: Api/CoreCmsUser/DoEditBalance
        /// <summary>
        /// 商家提现提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("商家提现提交")]
        public async Task<JsonResult> DoCash([FromBody] FMUpdateDecimalDataByIntId entity)
        {
            var jm = new AdminUiCallBack();
            var model = await _businessServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            model.money = model.money-entity.data;
            var business = _businessServices.QueryByClause(p => p.id == entity.id);

            BusinessOrder businessOrder = new BusinessOrder();
            businessOrder.businessName = business.businessName;
            businessOrder.businessid = business.id;
            businessOrder.userid = business.id;
            businessOrder.ordertype =(int)BusinessOrderTypeEnum.Cash;
            businessOrder.userName = business.businessName;
            businessOrder.before = business.money;
            businessOrder.after = business.money - entity.data;
            businessOrder.change = entity.data;
            businessOrder.createtime = DateTime.Now;
            businessOrder.orderNo = "商家提现";
            await _businessOrderServices.InsertAsync(businessOrder);
            //修改商家金额记录表
            var bl = await _businessServices.UpdateAsync(model);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            return new JsonResult(jm);
        }

        #endregion
    }
}
