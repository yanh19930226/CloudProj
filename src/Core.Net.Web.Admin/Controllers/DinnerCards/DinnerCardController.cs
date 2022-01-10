using Core.Net.Configuration;
using Core.Net.Entity.Dtos;
using Core.Net.Entity.Model.DinnerCard;
using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.DinnerCards;
using Core.Net.Util.Extensions;
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
    /// 餐卡
    /// </summary>
    [Description("餐卡")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DinnerCardController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDinnerCardServices _dinnerCardServices;

        ///  <summary>
        ///  构造函数
        /// </summary>
        ///   <param name="webHostEnvironment"></param>
        /// <param name="coreCmsArticleTypeServices"></param>
        ///  <param name="coreCmsArticleServices"></param>
        public DinnerCardController(
            IWebHostEnvironment webHostEnvironment,
            IDinnerCardServices dinnerCardServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _dinnerCardServices = dinnerCardServices;
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
            return new JsonResult(jm);
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
            var where = PredicateBuilder.True<DinnerCard>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<DinnerCard, object>> orderEx;
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

            //角色id int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
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
            var list = await _dinnerCardServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
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
        public async Task<JsonResult> GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            var categories = await _dinnerCardServices.QueryAsync();
            return new JsonResult(jm);
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
        public async Task<JsonResult> DoCreate([FromBody] DinnerCard entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await _dinnerCardServices.InsertAsync(entity) > 0;
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

            var model = await _dinnerCardServices.QueryByIdAsync(entity.id);
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
        // POST: Admins/CoreCmsArticleType/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<JsonResult> DoEdit([FromBody] DinnerCard entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _dinnerCardServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            //事物处理过程开始
            //oldModel.id = entity.id;
            //oldModel.name = entity.name;
            //oldModel.parentId = entity.parentId;
            //oldModel.sort = entity.sort;

            //事物处理过程结束
            var bl = await _dinnerCardServices.UpdateAsync(oldModel);
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

            var model = await _dinnerCardServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return new JsonResult(jm);
            }
            //执行删除
            var bl = await _dinnerCardServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return new JsonResult(jm);
        }
        #endregion

        #region 绑定============================================================
        // POST: Api/CoreCmsArticleType/DoDelete/10
        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("绑定")]
        public async Task<JsonResult> Binding([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            //var model = await _coreCmsArticleTypeServices.QueryByIdAsync(entity.id);
            //if (model == null)
            //{
            //    jm.msg = GlobalConstVars.DataisNo;
            //    return new JsonResult(jm);
            //}
            ////执行删除
            //var bl = await _coreCmsArticleTypeServices.DeleteByIdAsync(entity.id);
            //jm.code = bl ? 0 : 1;
            //jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return new JsonResult(jm);
        }
        #endregion

        #region 充值============================================================
        // POST: Api/CoreCmsArticleType/DoDelete/10
        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("充值")]
        public async Task<JsonResult> ReCharge([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            //var model = await _coreCmsArticleTypeServices.QueryByIdAsync(entity.id);
            //if (model == null)
            //{
            //    jm.msg = GlobalConstVars.DataisNo;
            //    return new JsonResult(jm);
            //}
            ////执行删除
            //var bl = await _coreCmsArticleTypeServices.DeleteByIdAsync(entity.id);
            //jm.code = bl ? 0 : 1;
            //jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return new JsonResult(jm);
        }
        #endregion

        #region 冻结============================================================
        // POST: Api/CoreCmsArticleType/DoDelete/10
        /// <summary>
        ///冻结
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("冻结")]
        public async Task<JsonResult> Frozen ([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            //var model = await _coreCmsArticleTypeServices.QueryByIdAsync(entity.id);
            //if (model == null)
            //{
            //    jm.msg = GlobalConstVars.DataisNo;
            //    return new JsonResult(jm);
            //}

            ////如果有子级不能删除
            //if (await _coreCmsArticleTypeServices.ExistsAsync(p => p.parentId == entity.id))
            //{
            //    jm.msg = GlobalConstVars.DeleteIsHaveChildren;
            //    return new JsonResult(jm);
            //}
            ////如果类别有关联文章不能删除
            //if (await _coreCmsArticleServices.ExistsAsync(p => p.typeId == entity.id))
            //{
            //    jm.msg = "栏目下有文章禁止删除";
            //    return new JsonResult(jm);
            //}
            ////执行删除
            //var bl = await _coreCmsArticleTypeServices.DeleteByIdAsync(entity.id);
            //jm.code = bl ? 0 : 1;
            //jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return new JsonResult(jm);
        }
        #endregion
    }
}
