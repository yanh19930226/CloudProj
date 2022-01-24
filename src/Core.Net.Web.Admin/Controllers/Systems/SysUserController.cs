using Core.Net.Configuration;
using Core.Net.Entity.Dtos;
using Core.Net.Entity.Enums;
using Core.Net.Entity.Model.DinnerCard;
using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.Model.Systems;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.DinnerCards;
using Core.Net.Service.Systems;
using Core.Net.Util.Extensions;
using Core.Net.Util.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Core.Net.Web.Admin.Controllers.Systems
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SysUserController : BaseController
    {
        private readonly ISysUserServices _sysUserServices;
        private readonly ISysOrganizationServices _sysOrganizationServices;
        private readonly ISysRoleServices _sysRoleServices;
        private readonly ICoreOrderServices _coreOrderServices;
        private readonly ISysUserRoleServices _sysUserRoleServices;
        private readonly IDinnerCardServices _dinnerCardServices;
        private readonly IDinnerCardDetailServices _dinnerCardDetailServices;


        /// <summary>
        ///     构造函数
        /// </summary>
        public SysUserController(ISysUserServices sysUserServices, ISysOrganizationServices sysOrganizationServices, ISysRoleServices sysRoleServices, ISysUserRoleServices sysUserRoleServices, ICoreOrderServices coreOrderServices, IDinnerCardDetailServices dinnerCardDetailServices, IDinnerCardServices dinnerCardServices)
        {
            _sysUserServices = sysUserServices;
            _sysOrganizationServices = sysOrganizationServices;
            _sysRoleServices = sysRoleServices;
            _coreOrderServices = coreOrderServices;
            _sysUserRoleServices = sysUserRoleServices;
            _dinnerCardDetailServices = dinnerCardDetailServices;
            _dinnerCardServices = dinnerCardServices;
        }

        #region 首页数据============================================================

        // POST: Api/SysUser/GetIndex
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

        // POST: Api/SysUser/GetPageList
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
            var where = PredicateBuilder.True<SysUser>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<SysUser, object>> orderEx;
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
                case "avatar":
                    orderEx = p => p.avatar;
                    break;
                case "sex":
                    orderEx = p => p.sex;
                    break;
                case "phone":
                    orderEx = p => p.phone;
                    break;
                case "email":
                    orderEx = p => p.email;
                    break;
                case "emailVerified":
                    orderEx = p => p.emailVerified;
                    break;
                case "trueName":
                    orderEx = p => p.trueName;
                    break;
                case "birthday":
                    orderEx = p => p.birthday;
                    break;
                case "introduction":
                    orderEx = p => p.introduction;
                    break;
                case "organizationId":
                    orderEx = p => p.organizationId;
                    break;
                case "state":
                    orderEx = p => p.state;
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

            //用户id int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //账号 nvarchar
            var userName = Request.Form["userName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(userName)) @where = @where.And(p => p.userName.Contains(userName));
            //密码 nvarchar
            var passWord = Request.Form["passWord"].FirstOrDefault();
            if (!string.IsNullOrEmpty(passWord)) @where = @where.And(p => p.passWord.Contains(passWord));
            //昵称 nvarchar
            var nickName = Request.Form["nickName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(nickName)) @where = @where.And(p => p.nickName.Contains(nickName));
            //头像 nvarchar
            var avatar = Request.Form["avatar"].FirstOrDefault();
            if (!string.IsNullOrEmpty(avatar)) @where = @where.And(p => p.avatar.Contains(avatar));
            //性别 int
            var sex = Request.Form["sex"].FirstOrDefault().ObjectToInt(0);
            if (sex > 0) @where = @where.And(p => p.sex == sex);
            //手机号 nvarchar
            var phone = Request.Form["phone"].FirstOrDefault();
            if (!string.IsNullOrEmpty(phone)) @where = @where.And(p => p.phone.Contains(phone));
            //邮箱 nvarchar
            var email = Request.Form["email"].FirstOrDefault();
            if (!string.IsNullOrEmpty(email)) @where = @where.And(p => p.email.Contains(email));
            //邮箱是否验证 bit
            var emailVerified = Request.Form["emailVerified"].FirstOrDefault();
            if (!string.IsNullOrEmpty(emailVerified) && emailVerified.ToLowerInvariant() == "true")
                @where = @where.And(p => p.emailVerified);
            else if (!string.IsNullOrEmpty(emailVerified) && emailVerified.ToLowerInvariant() == "false")
                @where = @where.And(p => p.emailVerified == false);
            //真实姓名 nvarchar
            var trueName = Request.Form["trueName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(trueName)) @where = @where.And(p => p.trueName.Contains(trueName));
            //个人简介 nvarchar
            var introduction = Request.Form["introduction"].FirstOrDefault();
            if (!string.IsNullOrEmpty(introduction)) @where = @where.And(p => p.introduction.Contains(introduction));
            //机构id int
            var organizationId = Request.Form["organizationId"].FirstOrDefault().ObjectToInt(0);
            if (organizationId > 0)
            {
                //where = where.And(p => p.organizationId == organizationId);
                var o = await _sysOrganizationServices.QueryAsync();
                var ids = new List<int>();
                SysOrganizationHelper.GetOrganizeChildIds(o, organizationId, ref ids);
                if (ids.Any())
                {
                    jm.otherData = ids;
                    where = where.And(p => ids.Contains((int)p.organizationId));
                }
            }

            //状态,0正常,1冻结 int
            var state = Request.Form["state"].FirstOrDefault().ObjectToInt(0);
            if (state > 0) @where = @where.And(p => p.state == state);
            //是否删除,0否,1是 bit
            var deleted = Request.Form["deleted"].FirstOrDefault();
            if (!string.IsNullOrEmpty(deleted) && deleted.ToLowerInvariant() == "true")
                @where = @where.And(p => p.deleted);
            else if (!string.IsNullOrEmpty(deleted) && deleted.ToLowerInvariant() == "false")
                @where = @where.And(p => p.deleted == false);
            //注册时间 datetime
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
            var list = await _sysUserServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";

            if (list.Any())
            {
                var sysRoles = await _sysRoleServices.QueryAsync();
                var sysUserRoles = await _sysUserRoleServices.QueryAsync();

                foreach (var user in list)
                {
                    var roleIds = sysUserRoles.Where(p => p.userId == user.id).Select(p => p.roleId).ToList();
                    if (roleIds.Any()) user.roles = sysRoles.Where(p => roleIds.Contains(p.id)).ToList();
                }
            }

            return Json(jm);
        }

        #endregion

        #region 创建数据============================================================

        // POST: Api/SysUser/GetCreate
        /// <summary>
        ///     创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public async Task<JsonResult> GetCreate()
        {
            //返回数据
            var roles = await _sysRoleServices.QueryListByClauseAsync(p => p.deleted == false);
            var orgs = _sysOrganizationServices.Query();
            var jm = new AdminUiCallBack { code = 0 };
            jm.data = new {roles, orgs };
            return new JsonResult(jm);
        }

        #endregion

        #region 创建提交============================================================

        // POST: Api/SysUser/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<JsonResult> DoCreate([FromBody] SysUser entity)
        {
            var jm = new AdminUiCallBack();

            var haveName = await _sysUserServices.ExistsAsync(p => p.userName == entity.userName);
            if (haveName)
            {
                jm.msg = "账号已经存在";
                return new JsonResult(jm);
            }
            var org = _sysOrganizationServices.QueryByClause(p => p.id == entity.organizationId);

            entity.createTime = DateTime.Now;
            entity.organizationName = org.organizationName;
            entity.balance = 0;
            entity.passWord = CommonHelper.Md5For32(entity.passWord);
            var id = await _sysUserServices.InsertAsync(entity);
            //设置用户角色
            if (id > 0 && !string.IsNullOrEmpty(entity.roleIds))
            {
                var strIds = entity.roleIds.Split(",");
                var ids = CommonHelper.StringArrAyToIntArray(strIds);
                if (ids.Any())
                {
                    var userRoles = new List<SysUserRole>();
                    foreach (var itemRoleId in ids)
                        userRoles.Add(new SysUserRole
                        {
                            createTime = DateTime.Now,
                            roleId = itemRoleId,
                            userId = id
                        });
                    if (userRoles.Any()) await _sysUserRoleServices.InsertAsync(userRoles);
                }
            }

            jm.otherData = entity;
            var bl = id > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;

            return new JsonResult(jm);
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/SysUser/GetEdit
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
            var orgs = _sysOrganizationServices.Query();
            var model = await _sysUserServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            var userRoles = await _sysUserRoleServices.QueryListByClauseAsync(p => p.userId == model.id);
            var roleIds = userRoles.Select(p => p.roleId).ToList();
            var roles = await _sysRoleServices.QueryListByClauseAsync(p => p.deleted == false);

            jm.code = 0;
            jm.data = new
            {
                model,
                roles,
                roleIds,
                orgs
            };

            return new JsonResult(jm);
        }

        #endregion

        #region 编辑提交============================================================

        // POST: Api/SysUser/Edit
        /// <summary>
        ///  编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<JsonResult> DoEdit([FromBody] SysUser entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _sysUserServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            if (oldModel.userName != entity.userName)
            {
                var haveName = await _sysUserServices.ExistsAsync(p => p.userName == entity.userName);
                if (haveName)
                {
                    jm.msg = "账号已经存在";
                    return new JsonResult(jm);
                }
            }
            var org = _sysOrganizationServices.QueryByClause(p => p.id == entity.organizationId);
            //事物处理过程开始
            oldModel.userName = entity.userName;
            oldModel.organizationName = org.organizationName;
            oldModel.organizationId = entity.organizationId > 0 ? entity.organizationId : 0;
            oldModel.nickName = entity.nickName;
            oldModel.sex = entity.sex;
            oldModel.phone = entity.phone;
            oldModel.cardNo = entity.cardNo;
            oldModel.idCardNo = entity.idCardNo;
            oldModel.updateTime = DateTime.Now;

            //事物处理过程结束
            var bl = await _sysUserServices.UpdateAsync(oldModel);
            if (bl)
            {
                await _sysUserRoleServices.DeleteAsync(p => p.userId == oldModel.id);
                if (!string.IsNullOrEmpty(entity.roleIds))
                {
                    var strIds = entity.roleIds.Split(",");
                    var ids = CommonHelper.StringArrAyToIntArray(strIds);
                    if (ids.Any())
                    {
                        var userRoles = new List<SysUserRole>();
                        foreach (var itemRoleId in ids)
                            userRoles.Add(new SysUserRole
                            {
                                createTime = DateTime.Now,
                                roleId = itemRoleId,
                                userId = oldModel.id
                            });
                        if (userRoles.Any()) await _sysUserRoleServices.InsertAsync(userRoles);
                    }
                }
            }

            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/SysUser/DoDelete/10
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

            var model = await _sysUserServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return new JsonResult(jm);
            }

            if (model.id == 1)
            {
                jm.msg = "初始管理员账户禁止删除";
                return new JsonResult(jm);
            }

            var bl = await _sysUserServices.DeleteByIdAsync(entity.id);
            if (bl) await _sysUserRoleServices.DeleteAsync(p => p.userId == model.id);

            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return new JsonResult(jm);
        }

        #endregion

        #region 设置是否锁定============================================================

        // POST: Api/SysUser/DoSetdeleted/10
        /// <summary>
        ///     设置是否锁定,0否,1是
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否锁定,0否,1是")]
        public async Task<JsonResult> DoSetState([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _sysUserServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            oldModel.state = entity.data ? 0 : 1;

            var bl = await _sysUserServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
        }

        #endregion

        #region 修改余额============================================================

        // POST: Api/CoreCmsUser/GetEditBalance
        /// <summary>
        ///  修改余额
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("修改余额")]
        public async Task<JsonResult> GetEditBalance([FromBody] FMIntId entity)
        {
            //返回数据
            var jm = new AdminUiCallBack();

            var model = await _sysUserServices.QueryByIdAsync(entity.id);
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
            var jm = new AdminUiCallBack();
            var model = await _sysUserServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            model.balance = model.balance + entity.data;
            var bl = await _sysUserServices.UpdateAsync(model);
            CoreOrder coreOrder = new CoreOrder();
            coreOrder.organizationId = (int)model.organizationId;
            coreOrder.sysUserId = model.id;
            coreOrder.userName = model.userName;
            coreOrder.organizationName = model.organizationName;
            coreOrder.orderNo = RandomNumber.GetRandomOrder();
            coreOrder.orderType = (int)OrderTypeEnum.MoneyRecharge;
            coreOrder.status = (int)OrderStatusEnum.Complete;
            coreOrder.amount = entity.data;
            coreOrder.balance = Convert.ToDecimal(model.balance);
            coreOrder.createTime = DateTime.Now;
            coreOrder.telePhone = model.phone;
            await _coreOrderServices.InsertAsync(coreOrder);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            return new JsonResult(jm);
        }

        #endregion

        #region 修改密码============================================================

        // POST: Api/CoreCmsUser/GetEditBalance
        /// <summary>
        ///     修改密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("修改余额")]
        public async Task<JsonResult> GetEditPwd([FromBody] FMIntId entity)
        {
            //返回数据
            var jm = new AdminUiCallBack();
            var model = await _sysUserServices.QueryByIdAsync(entity.id);
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

        #region 修改密码提交============================================================

        // POST: Api/CoreCmsUser/DoEditBalance
        /// <summary>
        /// 修改密码提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("修改余额提交")]
        public async Task<JsonResult> DoEditPwd([FromBody] FMUpdateStrDataByIntId entity)
        {
            var jm = new AdminUiCallBack();
            var model = await _sysUserServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            model.passWord = CommonHelper.Md5For32(entity.pwd);
            var bl = await _sysUserServices.UpdateAsync(model);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            return new JsonResult(jm);
        }

        #endregion

        #region 编辑绑定============================================================

        // POST: Api/SysUser/GetEdit
        /// <summary>
        ///  编辑绑定
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑绑定")]
        public async Task<JsonResult> GetBind([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();
            var model = await _sysUserServices.QueryByIdAsync(entity.id);
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

        #region 绑定提交============================================================
        // POST: Api/SysUser/Edit
        /// <summary>
        ///  绑定提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("绑定提交")]
        public async Task<JsonResult> DoBind([FromBody] FMUpdateStrDataByIntId entity)
        {
            var jm = new AdminUiCallBack();
            var oldModel = await _sysUserServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            oldModel.cardNo = entity.pwd;
              //事物处理过程结束
            var bl = await _sysUserServices.UpdateAsync(oldModel);
            if (bl)
            {
                var dnnercard =await  _dinnerCardServices.QueryByClauseAsync(p => p.cardno == oldModel.cardNo);

                if (dnnercard==null)
                {
                    //添加餐卡绑定信息
                    DinnerCard dinnerCard = new DinnerCard();
                    dinnerCard.sysuserid = oldModel.id;
                    dinnerCard.cardno = entity.pwd;
                    dinnerCard.username = oldModel.userName;
                    dinnerCard.orgid = (int)oldModel.organizationId;
                    dinnerCard.orgname = oldModel.organizationName;
                    dinnerCard.telephone = oldModel.phone;
                    dinnerCard.createtime = DateTime.Now;
                    await _dinnerCardServices.InsertAsync(dinnerCard);

                    //添加餐卡记录
                    DinnerCardDetail dinnerCardDetail = new DinnerCardDetail();
                    dinnerCardDetail.cardno = entity.pwd;
                    dinnerCardDetail.username = oldModel.userName;
                    dinnerCardDetail.orgname = oldModel.organizationName;
                    dinnerCardDetail.telephone = oldModel.phone;
                    dinnerCardDetail.action = "绑定";
                    dinnerCardDetail.opuser = "";
                    dinnerCardDetail.optime = DateTime.Now;
                    dinnerCardDetail.createtime = DateTime.Now;
                    await _dinnerCardDetailServices.InsertAsync(dinnerCardDetail);
                }
                else
                {
                    //更新旧卡信息
                    var old = _dinnerCardServices.QueryByClause(p=>p.cardno== oldModel.cardNo);
                    old.orgid =null;
                    old.orgname = "";
                    old.sysuserid = null;
                    old.telephone = "";
                    old.username = "";
                    await _dinnerCardServices.UpdateAsync(old);

                    //添加餐卡记录
                    DinnerCardDetail dinnerCardDetail = new DinnerCardDetail();
                    dinnerCardDetail.cardno = old.cardno;
                    dinnerCardDetail.username = oldModel.userName;
                    dinnerCardDetail.orgname = oldModel.organizationName;
                    dinnerCardDetail.telephone = oldModel.phone;
                    dinnerCardDetail.action = "解绑";
                    dinnerCardDetail.opuser = "";
                    dinnerCardDetail.optime = DateTime.Now;
                    dinnerCardDetail.createtime = DateTime.Now;
                    await _dinnerCardDetailServices.InsertAsync(dinnerCardDetail);


                    dnnercard.sysuserid = oldModel.id;
                    dnnercard.cardno = entity.pwd;
                    dnnercard.username = oldModel.userName;
                    dnnercard.orgid = (int)oldModel.organizationId;
                    dnnercard.orgname = oldModel.organizationName;
                    dnnercard.telephone = oldModel.phone;
                    dnnercard.cardno = entity.pwd;
                    await _dinnerCardServices.UpdateAsync(dnnercard);
                }
            }

            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            return new JsonResult(jm);
        }

        #endregion

        #region 用户导入
        public IActionResult GetImport()
        {
            var jm = new AdminUiCallBack { code = 0 };
            return new JsonResult(jm);
        }

        [HttpPost]
        public IActionResult DoImport(IFormFile excelfile)
        {
            var jm = new AdminUiCallBack { code = 0 };
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files[0];
                try
                {
                    using (var stream = new MemoryStream())
                    {
                        file.CopyToAsync(stream);//取到文件流
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        using (var dt = new Excel().ReadExcel(stream))
                        {
                            for (int row = 0; row < dt.Rows.Count; row++)
                            {
                                SysUser sysUser = new SysUser();

                                if (dt.Rows[row]["姓名"] != null)
                                {
                                    sysUser.userName = dt.Rows[row]["姓名"].ToString();
                                }

                                if (dt.Rows[row]["身份证号"] != null)
                                {
                                    sysUser.idCardNo = dt.Rows[row]["身份证号"].ToString();
                                }

                                if (dt.Rows[row]["手机号"] != null)
                                {
                                    sysUser.phone = dt.Rows[row]["手机号"].ToString();
                                }

                                if (dt.Rows[row]["餐卡卡号"] != null)
                                {
                                    sysUser.cardNo = dt.Rows[row]["餐卡卡号"].ToString();
                                }
                                var name = dt.Rows[row]["部门"].ToString();
                                var org = _sysOrganizationServices.QueryByClause(p => p.organizationName.Contains(name));
                                if (org != null)
                                {
                                    sysUser.organizationId = org.id;
                                    sysUser.organizationName = dt.Rows[row]["部门"].ToString();
                                }
                                else
                                {
                                    var orgg = _sysOrganizationServices.QueryByClause(p => p.organizationName == "默认");
                                    sysUser.organizationId = orgg.id;
                                    sysUser.balance = 0M;
                                    sysUser.organizationName = orgg.organizationName;
                                }
                                sysUser.createTime = DateTime.Now;
                                _sysUserServices.Insert(sysUser);

                            }
                            jm.msg = "导入成功"; 
                            return new JsonResult(jm);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new JsonResult(ex.Message);
                }
            }
            return new JsonResult(jm);
        }
        #endregion

        #region 批量充值
        public IActionResult GetImportCz()
        {
            var jm = new AdminUiCallBack { code = 0 };
            return new JsonResult(jm);
        }

        [HttpPost]
        public IActionResult DoImportCz(IFormFile excelfile)
        {
            var jm = new AdminUiCallBack { code = 0 };
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files[0];
                try
                {
                    using (var stream = new MemoryStream())
                    {
                        file.CopyToAsync(stream);//取到文件流
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        using (var dt = new Excel().ReadExcel(stream))
                        {
                            for (int row = 0; row < dt.Rows.Count; row++)
                            {
                                var phone = dt.Rows[row]["手机号"].ToString();
                                var name = dt.Rows[row]["姓名"].ToString();
                                var user = _sysUserServices.QueryByClause(p => p.phone == phone&&p.userName==name);
                                var cz =Convert.ToDecimal(dt.Rows[row]["金额"].ToString());
                                user.balance = user.balance == null ? 0 : user.balance;
                                user.balance = user.balance + cz;
                                _sysUserServices.Update(user);

                                CoreOrder coreOrder = new CoreOrder();
                                coreOrder.organizationId = (int)user.organizationId;
                                coreOrder.sysUserId = user.id;
                                coreOrder.userName = user.userName;
                                coreOrder.organizationName = user.organizationName;
                                coreOrder.orderNo = RandomNumber.GetRandomOrder();
                                coreOrder.orderType = (int)OrderTypeEnum.MoneyRecharge;
                                coreOrder.status = (int)OrderStatusEnum.Complete;
                                coreOrder.amount = cz;
                                coreOrder.balance = Convert.ToDecimal(user.balance);
                                coreOrder.createTime = DateTime.Now;
                                coreOrder.telePhone = user.phone;
                                 _coreOrderServices.Insert(coreOrder);
                            }
                            jm.msg = "充值成功";
                            return new JsonResult(jm);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new JsonResult(ex.Message);
                }
            }
            return new JsonResult(jm);
        }
        #endregion
    }
}
