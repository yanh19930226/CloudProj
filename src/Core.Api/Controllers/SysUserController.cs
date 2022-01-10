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

        #region 个人订单分页列表 ============================================================
        /// <summary>
        /// 个人订单分页列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetMyPageOrder([FromBody] OrderSearchDto orderSearchDto )
        {
            int sysUserId = Convert.ToInt32(SysUserId);
            var jm = new PageCallBackResult<IPageList<CoreOrder>>();

            var where = PredicateBuilder.True<CoreOrder>();
            where = where.And(p => p.sysUserId == sysUserId);

            if (!string.IsNullOrEmpty(orderSearchDto.Text))
            {
                where = where.And(p => p.goodName.Contains(orderSearchDto.Text));
            }

            var list = await _coreGoodsOrderServices.QueryPageAsync(where, p => p.createTime, OrderByType.Desc, orderSearchDto.page, orderSearchDto.limit);
            jm.Count = list.TotalCount;

            jm.Success(list, "数据调用成功");
            return Ok(jm);
        }
        #endregion

        #region 订单详细 ============================================================
        /// <summary>
        /// 订单详细
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{orderId}")]
        public async Task<IActionResult> GetMyOrderDetail(int orderId)
        {
            var jm = new CallBackResult<CoreOrder>();

            var order = await _coreGoodsOrderServices.QueryByClauseAsync(p => p.id == orderId);

            jm.Success(order, "数据调用成功");

            return Ok(jm);
        }

        #endregion

        #region 完成订单
        /// <summary>
        /// 完成订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{orderId}")]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            var jm = new CallBackResult<bool>();
            var order = await _coreGoodsOrderServices.QueryByClauseAsync(p => p.id == orderId);

            if (order == null)
            {
                jm.Failed("订单不存在");
            }
            else
            {
                order.status = 1000;
                order.completeTime = DateTime.Now;
                jm.Success(true, "订单完成");
            }
            return Ok(jm);
        }
        #endregion

        #region 取消订单
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var jm = new CallBackResult<bool>();

            var order = await _coreGoodsOrderServices.QueryByClauseAsync(p => p.id == orderId);

            if (order == null)
            {
                jm.Failed("订单不存在");
            }
            else
            {
                order.status = 2000;
                jm.Success(true, "订单取消");
            }
            return Ok(jm);
        }
        #endregion

        #region 管理员订单查询
        /// <summary>
        /// 管理员订单查询
        /// </summary>
        /// <param name="allOrderDto">搜索参数</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AllOrder([FromBody] AllOrderDto allOrderDto)
        {
            var jm = new CallBackResult<OrderListInfoDto>();
            var where = PredicateBuilder.True<CoreOrder>();

            //机构部门
            if (!string.IsNullOrEmpty(allOrderDto.organizationId))
            {
                var orgId = Convert.ToInt32(allOrderDto.organizationId);
                where = where.And(p => p.oraganizationId == orgId);
            }
            //岗位角色
            if (!string.IsNullOrEmpty(allOrderDto.roleId))
            {
                var rid = Convert.ToInt32(allOrderDto.roleId);
                where = where.And(p => p.roleId == rid);
            }
            //员工信息
            if (!string.IsNullOrEmpty(allOrderDto.userText))
            {
                where = where.And(p => p.userName.Contains(allOrderDto.userText));
            }
            //订单编号
            if (!string.IsNullOrEmpty(allOrderDto.orderNo))
            {
                where = where.And(p => p.orderNo.Contains(allOrderDto.orderNo));
            }
            //订单类型
            if (!string.IsNullOrEmpty(allOrderDto.orderType))
            {
                var orderType = Convert.ToInt32(allOrderDto.orderType);
                where = where.And(p => p.orderType == orderType);
            }
            //开始时间
            if (!string.IsNullOrEmpty(allOrderDto.startTime))
            {
                var startTime = Convert.ToDateTime(allOrderDto.startTime);
                where = where.And(p => p.createTime >= startTime);
            }
            //结束时间
            if (!string.IsNullOrEmpty(allOrderDto.endTime))
            {
                var endTime = Convert.ToDateTime(allOrderDto.endTime);
                where = where.And(p => p.createTime <= endTime);
            }

            var orderlist = await _coreGoodsOrderServices.QueryListByClauseAsync(where);
            OrderListInfoDto orderListInfoDto = new OrderListInfoDto();

            if (orderlist!=null&& orderlist.Count>0)
            {
                orderListInfoDto.orders = orderlist;
                orderListInfoDto.OrderTotalFeet = orderlist.Sum(p => p.totalPrice);
                orderListInfoDto.OrderCount = orderlist.Count;
            }

            jm.Success(orderListInfoDto, "数据调用成功");

            return Ok(jm);
        }
        #endregion

        #region 商品统计查询
        /// <summary>
        /// 商品统计查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GoodStatic([FromBody] AllGoodDto allOrderDto)
        {
            var jm = new CallBackResult<OrderListInfoDto>();
            var where = PredicateBuilder.True<CoreOrder>();

            ////机构部门
            //if (!string.IsNullOrEmpty(allOrderDto.organizationId))
            //{
            //    var orgId = Convert.ToInt32(allOrderDto.organizationId);
            //    where = where.And(p => p.oraganizationId == orgId);
            //}
            ////岗位角色
            //if (!string.IsNullOrEmpty(allOrderDto.roleId))
            //{
            //    var rid = Convert.ToInt32(allOrderDto.roleId);
            //    where = where.And(p => p.roleId == rid);
            //}
            ////员工信息
            //if (!string.IsNullOrEmpty(allOrderDto.userText))
            //{
            //    where = where.And(p => p.userName.Contains(allOrderDto.userText));
            //}
            ////订单编号
            //if (!string.IsNullOrEmpty(allOrderDto.orderNo))
            //{
            //    where = where.And(p => p.orderNo.Contains(allOrderDto.orderNo));
            //}
            ////订单类型
            //if (!string.IsNullOrEmpty(allOrderDto.orderType))
            //{
            //    var orderType = Convert.ToInt32(allOrderDto.orderType);
            //    where = where.And(p => p.orderType == orderType);
            //}
            ////开始时间
            //if (!string.IsNullOrEmpty(allOrderDto.startTime))
            //{
            //    var startTime = Convert.ToDateTime(allOrderDto.startTime);
            //    where = where.And(p => p.createTime >= startTime);
            //}
            ////结束时间
            //if (!string.IsNullOrEmpty(allOrderDto.endTime))
            //{
            //    var endTime = Convert.ToDateTime(allOrderDto.endTime);
            //    where = where.And(p => p.createTime <= endTime);
            //}

            //var orderlist = await _coreGoodsOrderServices.QueryListByClauseAsync(where);
            //OrderListInfoDto orderListInfoDto = new OrderListInfoDto();

            //if (orderlist != null && orderlist.Count > 0)
            //{
            //    orderListInfoDto.orders = orderlist;
            //    orderListInfoDto.OrderTotalFeet = orderlist.Sum(p => p.totalPrice);
            //    orderListInfoDto.OrderCount = orderlist.Count;
            //}

            //jm.Success(orderListInfoDto, "数据调用成功");

            return Ok(jm);
        }
        #endregion

        #region 订单统计查询
        /// <summary>
        /// 订单统计查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> OrderStatic([FromBody] AllOrderDto allOrderDto)
        {
            var jm = new CallBackResult<OrderListInfoDto>();
            var where = PredicateBuilder.True<CoreOrder>();

            //机构部门
            if (!string.IsNullOrEmpty(allOrderDto.organizationId))
            {
                var orgId = Convert.ToInt32(allOrderDto.organizationId);
                where = where.And(p => p.oraganizationId == orgId);
            }
            //岗位角色
            if (!string.IsNullOrEmpty(allOrderDto.roleId))
            {
                var rid = Convert.ToInt32(allOrderDto.roleId);
                where = where.And(p => p.roleId == rid);
            }
            //员工信息
            if (!string.IsNullOrEmpty(allOrderDto.userText))
            {
                where = where.And(p => p.userName.Contains(allOrderDto.userText));
            }
            //订单编号
            if (!string.IsNullOrEmpty(allOrderDto.orderNo))
            {
                where = where.And(p => p.orderNo.Contains(allOrderDto.orderNo));
            }
            //订单类型
            if (!string.IsNullOrEmpty(allOrderDto.orderType))
            {
                var orderType = Convert.ToInt32(allOrderDto.orderType);
                where = where.And(p => p.orderType == orderType);
            }
            //开始时间
            if (!string.IsNullOrEmpty(allOrderDto.startTime))
            {
                var startTime = Convert.ToDateTime(allOrderDto.startTime);
                where = where.And(p => p.createTime >= startTime);
            }
            //结束时间
            if (!string.IsNullOrEmpty(allOrderDto.endTime))
            {
                var endTime = Convert.ToDateTime(allOrderDto.endTime);
                where = where.And(p => p.createTime <= endTime);
            }

            var orderlist = await _coreGoodsOrderServices.QueryListByClauseAsync(where);
            OrderListInfoDto orderListInfoDto = new OrderListInfoDto();

            if (orderlist != null && orderlist.Count > 0)
            {
                orderListInfoDto.orders = orderlist;
                orderListInfoDto.OrderTotalFeet = orderlist.Sum(p => p.totalPrice);
                orderListInfoDto.OrderCount = orderlist.Count;
            }

            jm.Success(orderListInfoDto, "数据调用成功");

            return Ok(jm);
        }
        #endregion
    }
}
