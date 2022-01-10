﻿using Core.Net.Configuration;
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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SysRoleController : Controller
    {
        private readonly ISysRoleServices _sysRoleServices;
        private readonly ISysMenuServices _sysMenuServices;
        private readonly ISysRoleMenuServices _sysRoleMenuServices;

        /// <summary>
        ///  构造函数
        /// </summary>
        public SysRoleController(ISysRoleServices sysRoleServices, ISysMenuServices sysMenuServices, ISysRoleMenuServices sysRoleMenuServices)
        {
            _sysRoleServices = sysRoleServices;
            _sysMenuServices = sysMenuServices;
            _sysRoleMenuServices = sysRoleMenuServices;
        }

        #region 首页数据============================================================

        // POST: Api/SysRole/GetIndex
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

        // POST: Api/SysRole/GetPageList
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
            var where = PredicateBuilder.True<SysRole>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<SysRole, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "roleName":
                    orderEx = p => p.roleName;
                    break;
                case "roleCode":
                    orderEx = p => p.roleCode;
                    break;
                case "comments":
                    orderEx = p => p.comments;
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

            //角色id int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //角色名称 nvarchar
            var roleName = Request.Form["roleName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(roleName)) @where = @where.And(p => p.roleName.Contains(roleName));
            //角色标识 nvarchar
            var roleCode = Request.Form["roleCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(roleCode)) @where = @where.And(p => p.roleCode.Contains(roleCode));
            //备注 nvarchar
            var comments = Request.Form["comments"].FirstOrDefault();
            if (!string.IsNullOrEmpty(comments)) @where = @where.And(p => p.comments.Contains(comments));
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
            var list = await _sysRoleServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return Json(jm);
        }

        #endregion

        #region 创建数据============================================================

        // POST: Api/SysRole/GetCreate
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

        // POST: Api/SysRole/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<JsonResult> DoCreate([FromBody] SysRole entity)
        {
            var jm = new AdminUiCallBack();

            entity.createTime = DateTime.Now;

            var bl = await _sysRoleServices.InsertAsync(entity) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;

            return new JsonResult(jm);
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/SysRole/GetEdit
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

            var model = await _sysRoleServices.QueryByIdAsync(entity.id);
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

        // POST: Api/SysRole/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<JsonResult> DoEdit([FromBody] SysRole entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _sysRoleServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            //事物处理过程开始
            oldModel.roleName = entity.roleName;
            oldModel.roleCode = entity.roleCode;
            oldModel.comments = entity.comments;
            oldModel.updateTime = DateTime.Now;

            //事物处理过程结束
            var bl = await _sysRoleServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/SysRole/DoDelete/10
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

            var model = await _sysRoleServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return new JsonResult(jm);
            }

            var bl = await _sysRoleServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return new JsonResult(jm);
        }

        #endregion

        #region 批量删除============================================================

        // POST: Api/SysRole/DoBatchDelete/10,11,20
        /// <summary>
        ///     批量删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量删除")]
        public async Task<JsonResult> DoBatchDelete([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await _sysRoleServices.DeleteByIdsAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return Json(jm);
        }

        #endregion

        #region 获取菜单=====================================================================

        // POST: Api/SysRole/GetSysMenu
        /// <summary>
        ///     获取菜单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("获取菜单")]
        public async Task<JsonResult> GetSysMenu([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();
            //获取所有菜单
            var model = await _sysMenuServices.QueryListByClauseAsync(p => p.deleted == false && p.hide == false,
                p => p.sortNumber, OrderByType.Asc);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return Json(jm);
            }

            //获取角色的权限
            var roleMenus = await _sysRoleMenuServices.QueryListByClauseAsync(p => p.roleId == entity.id);
            var list = new List<SysMenuTreeDto>();
            if (model.Any())
                model.ForEach(p =>
                {
                    list.Add(new SysMenuTreeDto
                    {
                        authority = p.authority,
                        @checked = roleMenus.Exists(m => m.menuId == p.id),
                        children = null,
                        component = p.component,
                        createTime = p.createTime,
                        deleted = p.deleted,
                        hide = p.hide,
                        iconColor = p.iconColor,
                        menuName = p.menuName,
                        menuIcon = p.menuIcon,
                        menuType = p.menuType,
                        id = p.id,
                        open = true,
                        parentId = p.parentId,
                        parentName = "",
                        path = p.path,
                        sortNumber = p.sortNumber,
                        target = p.target,
                        updateTime = p.updateTime
                    });
                });
            jm.code = 0;
            jm.data = list;
            jm.otherData = new
            {
                entity,
                roleMenus
            };

            return Json(jm);
        }

        #endregion

        #region 设置权限=====================================================================

        // POST: Api/SysRole/DoSetSysMenu
        /// <summary>
        ///     设置权限
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置权限")]
        public async Task<JsonResult> DoSetSysMenu([FromBody] FMIntIdByListIntData entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _sysRoleServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return Json(jm);
            }
            //把角色原来的权限全部删除
            await _sysRoleMenuServices.DeleteAsync(p => p.roleId == oldModel.id);
            if (entity.data.Any())
            {
                var list = new List<SysRoleMenu>();
                entity.data.ForEach(p =>
                {
                    list.Add(new SysRoleMenu
                    {
                        createTime = DateTime.Now,
                        menuId = p,
                        roleId = oldModel.id
                    });
                });
                await _sysRoleMenuServices.InsertAsync(list);
            }

            jm.code = 0;
            jm.msg = "权限设置成功";

            return Json(jm);
        }

        #endregion
    }
}
