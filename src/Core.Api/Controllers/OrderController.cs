using Core.Api.Models.Dtos;
using Core.Net.Data;
using Core.Net.Entity.Enums;
using Core.Net.Entity.Model.DinnerCard;
using Core.Net.Entity.Model.Expression;
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
    /// 订单控制器
    /// </summary>
    public class OrderController : BaseController
    {
        private readonly ICoreOrderServices _coreOrderServices;
        private readonly ICoreGoodsServices _coreGoodsServices;
        private readonly ISysUserServices _sysUserServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISysOrganizationServices _sysOrganizationServices;
        private readonly ICoreGoodOrderDetailServices _coreGoodOrderDetailServices;
        /// <summary>
        /// OrderController
        /// </summary>
        /// <param name="coreOrderServices"></param>
        public OrderController(ICoreOrderServices coreOrderServices, ICoreGoodOrderDetailServices coreGoodOrderDetailServices, ISysUserServices sysUserServices, ISysOrganizationServices sysOrganizationServices, ICoreGoodsServices coreGoodsServices, IUnitOfWork unitOfWork)
        {
            _coreOrderServices = coreOrderServices;
            _sysUserServices = sysUserServices;
            _coreGoodsServices = coreGoodsServices;
            _unitOfWork = unitOfWork;
            _sysOrganizationServices = sysOrganizationServices;
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
            try
            {

                _unitOfWork.BeginTran();

                var user = _sysUserServices.QueryByClause(p => p.id == Convert.ToInt32(SysUserId));

                List<CoreGoods> goodlist = new List<CoreGoods>();

                var totalPrice = 0M;

                foreach (var item in createOrderDto.GoodInfos)
                {
                    var uprice = _coreGoodsServices.QueryByClause(p => p.id == item.GoodId).unitPrice;
                    totalPrice += item.Number * uprice;
                    goodlist.Add(_coreGoodsServices.QueryByClause(p => p.id == item.GoodId));
                }

                var org = _sysOrganizationServices.QueryByClause(p => p.id == user.organizationId);

                CoreOrder coreOrder = new CoreOrder();
                coreOrder.oraganizationId = Convert.ToInt32(user.organizationId);
                coreOrder.organizationName = org?.organizationName;
                coreOrder.sysUserId = Convert.ToInt32(user.id);
                coreOrder.userName = user.userName;
                coreOrder.roleId = Convert.ToInt32(user.id);
                coreOrder.roleName = "";
                var orderNo= Guid.NewGuid().ToString("N");
                coreOrder.orderNo = orderNo;
                //订单类型
                coreOrder.orderType = (int)OrderTypeEnum.GoodOrder;
                //订单状态
                coreOrder.status = (int)OrderStatusEnum.UnPayed;
                coreOrder.payType = (int)PayTypeEnum.Other;
                //订单总价
                coreOrder.totalPrice = totalPrice;
                coreOrder.telePhone = user.phone;
                coreOrder.createTime = DateTime.Now;
                await _coreOrderServices.InsertAsync(coreOrder);

                foreach (var item in goodlist)
                {
                    //创建订单详细
                    CoreGoodOrderDetail coreGoodOrderDetail = new CoreGoodOrderDetail();
                    coreGoodOrderDetail.orderNo = orderNo;
                    coreGoodOrderDetail.url = item.url;
                    coreGoodOrderDetail.goodId = item.id;
                    coreGoodOrderDetail.goodNo = item.goodNo;
                    coreGoodOrderDetail.goodName = item.goodName;
                    coreGoodOrderDetail.goodNum = createOrderDto.GoodInfos.Where(p=>p.GoodId== item.id).FirstOrDefault().Number;
                    coreGoodOrderDetail.unitPrice = item.unitPrice;
                    coreGoodOrderDetail.createTime =DateTime.Now;
                    coreGoodOrderDetail.goodNo = orderNo;
                    await _coreGoodOrderDetailServices.InsertAsync(coreGoodOrderDetail);
                }

                _unitOfWork.CommitTran();

                jm.Success("创建成功");
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTran();

                jm.Failed("创建失败");
            }
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

            var type = (int)OrderTypeEnum.GoodOrder;
            where = where.And(p => p.orderType == type);
            if (!string.IsNullOrEmpty(orderSearchDto.orderText))
            {
                where = where.And(p => p.orderNo.Contains(orderSearchDto.orderText));
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
            var jm = new CallBackResult<OrderDetailDto>();

            try
            {
                _unitOfWork.BeginTran();
                OrderDetailDto orderDetailDto = new OrderDetailDto();
                CoreOrderDto coreOrderDto = new CoreOrderDto();
                List<CoreOrderDetailDto> list = new List<CoreOrderDetailDto>();
                var order = await _coreOrderServices.QueryByClauseAsync(p => p.id == orderId);
                coreOrderDto.id = order.id;
                coreOrderDto.orderNo = order.orderNo;
                coreOrderDto.orderType = order.orderType;
                coreOrderDto.status = order.status;
                coreOrderDto.totalPrice = order.totalPrice;
                coreOrderDto.payType = order.payType;
                coreOrderDto.oraganizationId = order.oraganizationId;
                coreOrderDto.roleId = order.roleId;
                coreOrderDto.roleName = order.roleName;
                coreOrderDto.organizationName = order.organizationName;
                coreOrderDto.sysUserId = order.sysUserId;
                coreOrderDto.userName = order.userName;
                coreOrderDto.telePhone = order.telePhone;
                coreOrderDto.createTime = order.createTime;

                var orderdetail = await _coreGoodOrderDetailServices.QueryListByClauseAsync(p => p.orderNo == order.orderNo);

                foreach (var item in orderdetail)
                {
                    CoreOrderDetailDto coreOrderDetail = new CoreOrderDetailDto();
                    coreOrderDetail.id = item.id;
                    coreOrderDetail.url = item.url;
                    coreOrderDetail.orderNo = order.orderNo;
                    coreOrderDetail.goodId = item.goodId;
                    coreOrderDetail.goodName = item.goodName;
                    coreOrderDetail.goodNo = item.goodNo;
                    coreOrderDetail.goodNum = item.goodNum;
                    coreOrderDetail.unitPrice = item.unitPrice;
                    list.Add(coreOrderDetail);
                }
                orderDetailDto.coreOrder = coreOrderDto;
                orderDetailDto.coreOrderDetail = list;
                _unitOfWork.CommitTran();

                jm.Success(orderDetailDto, "数据调用成功");
            }
            catch (Exception)
            {
                _unitOfWork.RollbackTran();
                jm.Failed("失败");
            }
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
                order.status = (int)OrderStatusEnum.Complete;
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
                order.status = (int)OrderStatusEnum.Canceled;
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
