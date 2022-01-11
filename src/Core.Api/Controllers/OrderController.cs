using Core.Api.Models.Dtos;
using Core.Net.Data;
using Core.Net.Entity.Model.DinnerCard;
using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.DinnerCards;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Controllers
{
    /// <summary>
    /// 订单控制器
    /// </summary>
    public class OrderController : BaseController
    {
        private readonly ICoreOrderServices _coreOrderServices;
        private readonly ICoreGoodOrderDetailServices _coreGoodOrderDetailServices;
        /// <summary>
        /// OrderController
        /// </summary>
        /// <param name="coreOrderServices"></param>
        public OrderController(ICoreOrderServices coreOrderServices, ICoreGoodOrderDetailServices coreGoodOrderDetailServices)
        {
            _coreOrderServices = coreOrderServices;
            _coreGoodOrderDetailServices = coreGoodOrderDetailServices;
        }

        #region 创建订单 ============================================================
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="createOrderDto">创建订单参数</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto )
        {
            var jm = new CallBackResult<bool>();
            CoreOrder coreOrder = new CoreOrder();
            coreOrder.oraganizationId = Convert.ToInt32(OrganizationId);
            coreOrder.roleId = Convert.ToInt32(SysUserId);
            coreOrder.sysUserId = Convert.ToInt32(SysUserId);
            coreOrder.orderNo = Guid.NewGuid().ToString("N");
            coreOrder.orderType = 1000;
            coreOrder.status = 1000;
            coreOrder.unitPrice = createOrderDto.unitPrice;
            coreOrder.totalPrice = createOrderDto.totalPrice;
            coreOrder.goodName = createOrderDto.goodName;
            coreOrder.goodNo = createOrderDto.goodNo;
            coreOrder.goodUrl = createOrderDto.goodUrl;
            coreOrder.goodNumber = createOrderDto.goodNumber;
            coreOrder.payType = 2;
            coreOrder.userName = createOrderDto.userName;
            coreOrder.telePhone = createOrderDto.telePhone;
            coreOrder.createTime = DateTime.Now;
            await _coreOrderServices.InsertAsync(coreOrder);
            jm.Success("创建成功");
            return Ok(jm);
        }
        #endregion


        #region 个人订单分页列表 ============================================================
        /// <summary>
        /// 个人订单分页列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetMyPageOrder([FromBody] OrderSearchDto orderSearchDto)
        {
            int sysUserId = Convert.ToInt32(SysUserId);
            var jm = new PageCallBackResult<IPageList<CoreOrder>>();

            var where = PredicateBuilder.True<CoreOrder>();
            where = where.And(p => p.sysUserId == sysUserId);

            if (!string.IsNullOrEmpty(orderSearchDto.Text))
            {
                where = where.And(p => p.goodName.Contains(orderSearchDto.Text));
            }

            var list = await _coreOrderServices.QueryPageAsync(where, p => p.createTime, OrderByType.Desc, orderSearchDto.page, orderSearchDto.limit);
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

            var order = await _coreOrderServices.QueryByClauseAsync(p => p.id == orderId);

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
            var order = await _coreOrderServices.QueryByClauseAsync(p => p.id == orderId);

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

            var order = await _coreOrderServices.QueryByClauseAsync(p => p.id == orderId);

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

            var orderlist = await _coreOrderServices.QueryListByClauseAsync(where);

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

            var orderlist = await _coreOrderServices.QueryListByClauseAsync(where);
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
