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
        public IActionResult OrderType()
        {
            var jm = new CallBackResult<List<KV>>();
            List<KV> kVs = new List<KV>();
            kVs.Add(new KV() { 
            Key="",
            Value="全部"
            });
            kVs.Add(new KV()
            {
                Key = "0",
                Value = "刷卡扣费"
            });
            kVs.Add(new KV()
            {
                Key = "1",
                Value = "现金充值"
            });
            kVs.Add(new KV()
            {
                Key = "2",
                Value = "余额查询"
            });

            kVs.Add(new KV()
            {
                Key = "3",
                Value = "钱包转账"
            });

            kVs.Add(new KV()
            {
                Key = "4",
                Value = "商品订单"
            });
            kVs.Add(new KV()
            {
                Key = "5",
                Value = "公司福利"
            });
            //返回数据
            jm.Success(kVs, "数据调用成功");
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
            var jm = new CallBackResult<List<KV>>();
            List<KV> kVs = new List<KV>();
            kVs.Add(new KV()
            {
                Key = "",
                Value = "全部"
            });
            kVs.Add(new KV()
            {
                Key = "0",
                Value = "未付款"
            });
            kVs.Add(new KV()
            {
                Key = "1",
                Value = "已取消"
            });
            kVs.Add(new KV()
            {
                Key = "2",
                Value = "已付款"
            });

            kVs.Add(new KV()
            {
                Key = "3",
                Value = "已退款"
            });

            kVs.Add(new KV()
            {
                Key = "4",
                Value = "已发货"
            });
            kVs.Add(new KV()
            {
                Key = "5",
                Value = "已完成"
            });
            //返回数据
            jm.Success(kVs, "数据调用成功");
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
            var jm = new CallBackResult<List<KV>>();
            List<KV> kVs = new List<KV>();
            kVs.Add(new KV()
            {
                Key = "",
                Value = "全部"
            });
            kVs.Add(new KV()
            {
                Key = "3",
                Value = "停用"
            });
            kVs.Add(new KV()
            {
                Key = "1",
                Value = "待上架"
            });
            kVs.Add(new KV()
            {
                Key = "2",
                Value = "在售"
            });
            //返回数据
            jm.Success(kVs, "数据调用成功");
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
            var jm = new CallBackResult<List<KV>>();
            List<KV> kVs = new List<KV>();

            var where = PredicateBuilder.True<SysOrganization>();

            var organizations = await _sysOrganizationServices.QueryListByClauseAsync(where);
            foreach (var item in organizations)
            {
                KV kV = new KV();
                kV.Key = item.id.ToString();
                kV.Value = item.organizationName.ToString();
                kVs.Add(kV);
            }
            jm.Success(kVs, "数据调用成功");

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

            var jm = new CallBackResult<List<KV>>();
            List<KV> kVs = new List<KV>();
            //var jm = new CallBackResult<List<SysRole>>();
            var where = PredicateBuilder.True<SysRole>();

            var roleList = await _sysRoleServices.QueryListByClauseAsync(where);

            foreach (var item in roleList)
            {
                KV kV = new KV();
                kV.Key = item.id.ToString();
                kV.Value = item.roleName.ToString();
                kVs.Add(kV);
            }
            jm.Success(kVs, "数据调用成功");

            return Ok(jm);
        } 
        #endregion
    }

    public class KV
    {

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
