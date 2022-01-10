using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.Model.Systems;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Systems;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Controllers
{
    /// <summary>
    /// 基础信息
    /// </summary>
    public class BasicController : BaseController
    {
        private readonly ISysUserServices _sysUserServices;
        private readonly ISysRoleServices _sysRoleServices;
        private readonly ISysOrganizationServices _sysOrganizationServices;

        /// <summary>
        /// BasicController
        /// </summary>
        /// <param name="sysUserServices"></param>
        /// <param name="sysRoleServices"></param>
        /// <param name="sysOrganizationServices"></param>
        public BasicController(ISysUserServices sysUserServices,ISysRoleServices sysRoleServices, ISysOrganizationServices sysOrganizationServices)
        {
            _sysUserServices = sysUserServices;
            _sysOrganizationServices = sysOrganizationServices;
            _sysRoleServices = sysRoleServices;
        }

        #region 订单类型
        /// <summary>
        /// 订单类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public  IActionResult OrderType()
        {
            var jm = new CallBackResult<Dictionary<string, string>>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("", "全部");
            dic.Add("1", "食堂消费");
            dic.Add("2", "商品订单");
            dic.Add("3", "公司福利");
            dic.Add("4", "餐卡充值");
            //返回数据
            jm.Success(dic, "数据调用成功");
            return Ok(jm);
        }
        #endregion

        #region 订单状态
        /// <summary>
        /// 订单状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public  IActionResult OrderStatus()
        {
            var jm = new CallBackResult<Dictionary<string, string>>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("", "全部");
            dic.Add("1000", "完成");
            dic.Add("2000", "取消");
            dic.Add("3000", "退款");
            //返回数据
            jm.Success(dic, "数据调用成功");
            return Ok(jm);
        }
        #endregion

        #region 商品状态
        /// <summary>
        /// 商品状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GoodStatus()
        {
            var jm = new CallBackResult<Dictionary<string, string>>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("", "全部");
            dic.Add("1", "上架");
            dic.Add("2", "下架");
            //返回数据
            jm.Success(dic, "数据调用成功");
            return Ok(jm);
        }
        #endregion

        #region 获取部门机构
        /// <summary>
        /// 获取部门机构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetOrganizations()
        {
            var jm = new CallBackResult<List<SysOrganization>>();

            var where = PredicateBuilder.True<SysOrganization>();

            var organizations = await _sysOrganizationServices.QueryListByClauseAsync(where);

            jm.Success(organizations, "数据调用成功");

            return Ok(jm);
        } 
        #endregion

        #region 获取角色
        /// <summary>
        /// 获取角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var jm = new CallBackResult<List<SysRole>>();
            var where = PredicateBuilder.True<SysRole>();

            var roleList = await _sysRoleServices.QueryListByClauseAsync(where);

            jm.Success(roleList, "数据调用成功");

            return Ok(jm);
        } 
        #endregion
    }
}
