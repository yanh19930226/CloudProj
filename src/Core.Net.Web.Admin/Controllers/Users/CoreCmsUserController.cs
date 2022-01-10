using Core.Net.Configuration;
using Core.Net.Entity.Dtos;
using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.Model.Users;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Users;
using Core.Net.Util.Extensions;
using Core.Net.Util.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Net.Web.Admin.Controllers.Users
{
    /// <summary>
    ///     用户表
    /// </summary>
    [Description("用户表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CoreCmsUserController : Controller
    {
        private readonly ICoreCmsUserGradeServices _coreCmsUserGradeServices;
        private readonly ICoreCmsUserServices _coreCmsUserServices;
        private readonly IWebHostEnvironment _webHostEnvironment;


        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="coreCmsUserServices"></param>
        /// <param name="coreCmsUserGradeServices"></param>
        public CoreCmsUserController(
            IWebHostEnvironment webHostEnvironment
            , ICoreCmsUserServices coreCmsUserServices
            , ICoreCmsUserGradeServices coreCmsUserGradeServices
        )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsUserServices = coreCmsUserServices;
            _coreCmsUserGradeServices = coreCmsUserGradeServices;
        }

        #region 首页数据============================================================

        // POST: Api/CoreCmsUser/GetIndex
        /// <summary>
        ///     首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public async Task<JsonResult> GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            var sexTypes = EnumHelper.EnumToList<GlobalEnumVars.UserSexTypes>();
            var userStatus = EnumHelper.EnumToList<GlobalEnumVars.UserStatus>();
            var userAccountTypes = EnumHelper.EnumToList<GlobalEnumVars.UserAccountTypes>();
            var userGrade = await _coreCmsUserGradeServices.QueryAsync();
            jm.data = new
            {
                sexTypes,
                userStatus,
                userGrade,
                userAccountTypes
            };
            return Json(jm);
        }

        #endregion

        #region 获取列表============================================================

        // POST: Api/CoreCmsUser/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsUser>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsUser, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "userName":
                    orderEx = p => p.userName;
                    break;
                case "passWord":
                    orderEx = p => p.passWord;
                    break;
                case "mobile":
                    orderEx = p => p.mobile;
                    break;
                case "sex":
                    orderEx = p => p.sex;
                    break;
                case "birthday":
                    orderEx = p => p.birthday;
                    break;
                case "avatarImage":
                    orderEx = p => p.avatarImage;
                    break;
                case "nickName":
                    orderEx = p => p.nickName;
                    break;
                case "balance":
                    orderEx = p => p.balance;
                    break;
                case "point":
                    orderEx = p => p.point;
                    break;
                case "grade":
                    orderEx = p => p.grade;
                    break;
                case "createTime":
                    orderEx = p => p.createTime;
                    break;
                case "updataTime":
                    orderEx = p => p.updataTime;
                    break;
                case "status":
                    orderEx = p => p.status;
                    break;
                case "parentId":
                    orderEx = p => p.parentId;
                    break;
                case "isDelete":
                    orderEx = p => p.isDelete;
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

            //用户名 nvarchar
            var userName = Request.Form["userName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(userName))
            {
                where = where.And(p => p.userName.Contains(userName));
            }
            //手机号 nvarchar
            var mobile = Request.Form["mobile"].FirstOrDefault();
            if (!string.IsNullOrEmpty(mobile))
            {
                where = where.And(p => p.mobile.Contains(mobile));
            }
            //性别[1男2女3未知] int
            var sex = Request.Form["sex"].FirstOrDefault().ObjectToInt(0);
            if (sex > 0)
            {
                where = where.And(p => p.sex == sex);
            }
            //昵称 nvarchar
            var nickName = Request.Form["nickName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(nickName))
            {
                where = where.And(p => p.nickName.Contains(nickName));
            }
            //用户等级 int
            var grade = Request.Form["grade"].FirstOrDefault().ObjectToInt(0);
            if (grade > 0)
            {
                where = where.And(p => p.grade == grade);
            }
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
            var updataTime = Request.Form["updataTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(updataTime))
            {
                var dt = updataTime.ObjectToDate();
                where = where.And(p => p.updataTime > dt);
            }
            //状态[1正常2停用] int
            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0)
            {
                where = where.And(p => p.status == status);
            }
            //推荐人 int
            var parentId = Request.Form["parentId"].FirstOrDefault().ObjectToInt(0);
            if (parentId > 0)
            {
                where = where.And(p => p.parentId == parentId);
            }
            //删除标志 有数据就是删除 bit
            var isDelete = Request.Form["isDelete"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isDelete) && isDelete.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isDelete);
            }
            else if (!string.IsNullOrEmpty(isDelete) && isDelete.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isDelete == false);
            }
            //获取数据
            var list = await _coreCmsUserServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return Json(jm);
        }

        #endregion

        #region 创建数据============================================================

        // POST: Api/CoreCmsUser/GetCreate
        /// <summary>
        ///     创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public async Task<JsonResult> GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            var userGrade = await _coreCmsUserGradeServices.QueryAsync();
            jm.data = userGrade;

            return new JsonResult(jm);
        }

        #endregion

        #region 创建提交============================================================

        // POST: Api/CoreCmsUser/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<JsonResult> DoCreate([FromBody] CoreCmsUser entity)
        {
            var jm = new AdminUiCallBack();

            if (string.IsNullOrEmpty(entity.mobile))
            {
                jm.msg = "请输入用户手机号";
                return new JsonResult(jm);
            }

            var isHava = await _coreCmsUserServices.ExistsAsync(p => p.mobile == entity.mobile);
            if (isHava)
            {
                jm.msg = "已存在此手机号码";
                return new JsonResult(jm);
            }

            entity.createTime = DateTime.Now;
            entity.passWord = CommonHelper.Md5For32(entity.passWord);
            entity.parentId = 0;

            var bl = await _coreCmsUserServices.InsertAsync(entity) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;


            return new JsonResult(jm);
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/CoreCmsUser/GetEdit
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

            var model = await _coreCmsUserServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            jm.code = 0;
            var sexTypes = EnumHelper.EnumToList<GlobalEnumVars.UserSexTypes>();
            var userStatus = EnumHelper.EnumToList<GlobalEnumVars.UserStatus>();
            var userGrade = await _coreCmsUserGradeServices.QueryAsync();

            jm.data = new
            {
                model,
                userGrade,
                sexTypes,
                userStatus
            };

            return new JsonResult(jm);
        }

        #endregion

        #region 编辑提交============================================================

        // POST: Admins/CoreCmsUser/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<JsonResult> DoEdit([FromBody] CoreCmsUser entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsUserServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            if (entity.mobile != oldModel.mobile)
            {
                var isHava = await _coreCmsUserServices.ExistsAsync(p => p.mobile == entity.mobile);
                if (isHava)
                {
                    jm.msg = "已存在此手机号码";
                    return new JsonResult(jm);
                }
            }

            //事物处理过程开始

            if (!string.IsNullOrEmpty(entity.passWord)) oldModel.passWord = CommonHelper.Md5For32(entity.passWord);
            oldModel.mobile = entity.mobile;
            oldModel.sex = entity.sex;
            oldModel.birthday = entity.birthday;
            oldModel.avatarImage = entity.avatarImage;
            oldModel.nickName = entity.nickName;
            oldModel.grade = entity.grade;
            oldModel.updataTime = DateTime.Now;
            oldModel.status = entity.status;
            //事物处理过程结束
            var bl = await _coreCmsUserServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
        }

        #endregion

        #region 设置删除标志============================================================

        // POST: Api/CoreCmsUser/DoSetisDelete/10
        /// <summary>
        ///     设置删除标志 有数据就是删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置删除标志")]
        public async Task<JsonResult> DoSetisDelete([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsUserServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            oldModel.isDelete = entity.data;

            var bl = await _coreCmsUserServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
        }

        #endregion

        #region 修改余额============================================================

        // POST: Api/CoreCmsUser/GetEditBalance
        /// <summary>
        ///     修改余额
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("修改余额")]
        public async Task<JsonResult> GetEditBalance([FromBody] FMIntId entity)
        {
            //返回数据
            var jm = new AdminUiCallBack();

            var model = await _coreCmsUserServices.QueryByIdAsync(entity.id);
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

        #region 修改余额提交============================================================

        // POST: Api/CoreCmsUser/DoEditBalance
        /// <summary>
        ///     修改余额提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("修改余额提交")]
        public async Task<JsonResult> DoEditBalance([FromBody] FMUpdateDecimalDataByIntId entity)
        {
            var jm = await _coreCmsUserServices.UpdateBalance(entity.id, entity.data);
            return new JsonResult(jm);
        }

        #endregion

        #region 修改积分============================================================

        // POST: Api/CoreCmsUser/GetEditPoint
        /// <summary>
        ///     修改积分
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("修改积分")]
        public async Task<JsonResult> GetEditPoint([FromBody] FMIntId entity)
        {
            //返回数据
            var jm = new AdminUiCallBack();

            var model = await _coreCmsUserServices.QueryByIdAsync(entity.id);
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

        #region 修改积分提交============================================================

        // POST: Api/CoreCmsUser/DoEditPoint
        /// <summary>
        ///     修改积分提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("修改积分提交")]
        public async Task<JsonResult> DoEditPoint([FromBody] FMUpdateUserPoint entity)
        {
            var jm = await _coreCmsUserServices.UpdatePiont(entity);
            return new JsonResult(jm);
        }

        #endregion
    }
}
