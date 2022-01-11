using Core.Api.Models.Dtos;
using Core.Net.Data;
using Core.Net.Entity.Model.DinnerCard;
using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.Model.Systems;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.DinnerCards;
using Core.Net.Service.Systems;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Controllers
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public class SysUserController : BaseController
    {
        private readonly ISysUserServices _sysUserServices;
        private readonly ISysRoleServices _sysRoleServices;
        private readonly ISysOrganizationServices _sysOrganizationServices;
        private readonly ICoreOrderServices _coreGoodsOrderServices;
        private readonly ICoreGoodOrderDetailServices _coreGoodOrderDetailServices;

        /// <summary>
        /// SysUserController
        /// </summary>
        /// <param name="sysUserServices"></param>
        /// <param name="coreGoodsOrderServices"></param>
        /// <param name="coreGoodOrderDetailServices"></param>
        /// <param name="sysRoleServices"></param>
        /// <param name="sysOrganizationServices"></param>
        public SysUserController(ISysUserServices sysUserServices, ICoreOrderServices coreGoodsOrderServices, ICoreGoodOrderDetailServices coreGoodOrderDetailServices, ISysRoleServices sysRoleServices, ISysOrganizationServices sysOrganizationServices)
        {
            _sysUserServices = sysUserServices;
            _coreGoodsOrderServices = coreGoodsOrderServices;
            _sysOrganizationServices = sysOrganizationServices;
            _sysRoleServices = sysRoleServices;
            _coreGoodOrderDetailServices = coreGoodOrderDetailServices;
        }

        #region 个人信息 ============================================================
        /// <summary>
        /// 个人信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMyInfo()
        {
            MyInfoDto myInfoDto = new MyInfoDto();
            int sysUserId = Convert.ToInt32(SysUserId);
            var jm = new CallBackResult<MyInfoDto>();
            var model = await _sysUserServices.QueryByClauseAsync(p => p.id == sysUserId);
            if (model == null)
            {
                jm.Failed("数据调用失败");
                return Ok(jm);
            }
            else
            {
                var roleName="测试";
                myInfoDto.id = model.id;
                myInfoDto.userName = model.userName;
                myInfoDto.organizationName = _sysOrganizationServices.QueryByClause(p=>p.id== model.organizationId).organizationName;
                myInfoDto.roles = roleName;
                myInfoDto.phone = model.phone;
                myInfoDto.createTime = model.createTime;
                myInfoDto.idCardNo = model.idCardNo;
                myInfoDto.balance = model.balance;
            }
            jm.Success(myInfoDto, "数据调用成功");
            return Ok(jm);
        }
        #endregion

        #region 修改个人信息 ============================================================
        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateMyInfo([FromBody] UpdateMyInfoDto updateMyInfoDto)
        {
            int sysUserId = Convert.ToInt32(SysUserId);
            var jm = new CallBackResult<bool>();
            var model = await _sysUserServices.QueryByClauseAsync(p => p.id == sysUserId);
            if (model == null)
            {
                jm.Failed("数据调用失败");
                return Ok(jm);
            }
            else
            {
                model.userName = updateMyInfoDto.UserName;
                model.idCardNo = updateMyInfoDto.IdCardNo;
                model.phone = updateMyInfoDto.Phone;
                await _sysUserServices.UpdateAsync(model);
                jm.Success("更新成功");
                return Ok(jm);
            }
        }
        #endregion

        
    }
}
