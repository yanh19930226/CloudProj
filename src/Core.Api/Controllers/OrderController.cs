using Core.Api.Models.Dtos;
using Core.Net.Entity.Model.DinnerCard;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.DinnerCards;
using Microsoft.AspNetCore.Mvc;
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
        /// <summary>
        /// OrderController
        /// </summary>
        /// <param name="coreOrderServices"></param>
        public OrderController(ICoreOrderServices coreOrderServices)
        {
            _coreOrderServices = coreOrderServices;
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
    }
}
