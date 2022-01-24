using Core.Net.Entity.Model.DinnerCard;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.DinnerCards;
using Core.Net.Service.Systems;
using Core.Net.Util.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yande.CloudTool.Api.Models.Request;
using Yande.CloudTool.Api.Models.Response;

namespace Yande.CloudTool.Api.Controllers
{
    /// <summary>
    /// CloudCostToolController
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [OpenApiTag("消费机接口", Description = "消费机接口")]
    public class CloudCostToolController : Controller
    {
        private readonly ILogger<CloudCostToolController> _logger;

        private readonly ICoreOrderServices _coreOrderServices;
        private readonly IDinnerCardServices _dinnerCardServices;
        private readonly ISysUserServices _sysUserServices;

        /// <summary>
        /// CloudCostToolController
        /// </summary>
        /// <param name="logger"></param>
        public CloudCostToolController(ILogger<CloudCostToolController> logger, ICoreOrderServices coreOrderServices, IDinnerCardServices dinnerCardServices, ISysUserServices sysUserServices)
        {
            _logger = logger;
            _coreOrderServices = coreOrderServices;
            _dinnerCardServices = dinnerCardServices;
            _sysUserServices = sysUserServices;
        }
        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Test()
        {
            var jm = new CallBackResult();
            jm.Success("测试成功");
            return Ok(jm);
        }

        /// <summary>
        ///获取服务器时间接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/hxz/v1/ServerTime")]
        public IActionResult ServerTime()
        {
            ServerTimeResponse serverTimeResponse = new ServerTimeResponse();
            serverTimeResponse.Msg = "Success";
            serverTimeResponse.Status = 1;
            serverTimeResponse.Time = DateTime.Now.ToString("yyyyMMddHHmmss") + CommonHelper.GetWeek();
            return Ok(serverTimeResponse);
        }

        /// <summary>
        /// 用户刷卡综合接口
        /// </summary>
        /// <param name="consumTransactionDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/hxz/v1/ConsumTransactions")]
        public async Task<IActionResult> ConsumTransactions([FromBody] ConsumTransactionDto consumTransactionDto)
        {
            _logger.LogError(JsonConvert.SerializeObject(consumTransactionDto));

            ConsumTransactionsResponse resp = new ConsumTransactionsResponse();

            try
            {
                //1.根据卡号查询用户相关信息
                var user = _sysUserServices.QueryByClause(p=>p.cardNo== consumTransactionDto.CardNo);


                //var dinnerCard = _dinnerCardServices.QueryByClause(p => p.cardNo == consumTransactionDto.CardNo);
                //var sysUserId = dinnerCard.sysUserId;
                ////用户信息
                //var user = await _sysUserServices.QueryByClauseAsync(p => p.id == sysUserId);

                //刷卡扣费
                if (consumTransactionDto.Mode==0)
                {
                    //根据订单类型创建订单
                    CoreOrder coreOrder = new CoreOrder();
                    coreOrder.organizationId = Convert.ToInt32(user.organizationId);
                    coreOrder.roleId = 1;
                    coreOrder.sysUserId = Convert.ToInt32(user.id);
                    coreOrder.organizationName = user.organizationName;
                    //卡号
                    coreOrder.cardNo = consumTransactionDto.CardNo;
                    coreOrder.costOrderNo = consumTransactionDto.Order;
                    coreOrder.orderNo = RandomNumber.GetRandomOrder();
                    coreOrder.orderType = consumTransactionDto.Mode;

                    coreOrder.status = 1;
                    //消费金额
                    coreOrder.amount = Convert.ToDecimal(consumTransactionDto.Amount);
                    //变动前
                    coreOrder.totalPrice = Convert.ToDecimal(user.balance);
                    //余额
                    coreOrder.balance = Convert.ToDecimal(user.balance) - Convert.ToDecimal(consumTransactionDto.Amount);
                    coreOrder.createTime = DateTime.Now;
                    coreOrder.payType = Convert.ToInt32(consumTransactionDto.PayType);

                    #region 商品信息
                    // coreOrder.goodName
                    //coreOrder.goodNo
                    //  coreOrder.goodUrl
                    //    coreOrder.goodNumber
                    //    coreOrder.unitPrice
                    #endregion

                    #region 用户信息
                    coreOrder.userName = user.userName;
                    coreOrder.telePhone = user.phone;
                    #endregion
                    user.balance = coreOrder.balance;
                     _sysUserServices.Update(user);
                    await _coreOrderServices.InsertAsync(coreOrder);
                    //3.扣减卡上金额并返回数据
                    resp.Status = 1;
                    resp.Msg = "Success";
                    resp.Name = user.userName;
                    resp.CardNo = consumTransactionDto.CardNo;
                    resp.Money = coreOrder.balance.ToString();
                    resp.Subsidy = "";
                    resp.Times = "0";
                    resp.Integral = "0";
                    resp.InTime = "";
                    resp.OutTime = "";
                    resp.CumulativeTime = "";
                    resp.Amount = consumTransactionDto.Amount;
                    resp.VoiceID = "null";
                    resp.Text = "";

                }
                //余额查询
                if (consumTransactionDto.Mode == 2)
                {
                    //3.扣减卡上金额并返回数据
                    resp.Status = 1;
                    resp.Msg = "Success";
                    resp.Name = user.userName;
                    resp.CardNo = consumTransactionDto.CardNo;
                    resp.Money = user.balance.ToString();
                    resp.Subsidy = "10000.00";
                    resp.Times = "0";
                    resp.Integral = "0";
                    resp.InTime = "";
                    resp.OutTime = "";
                    resp.CumulativeTime = "";
                    resp.Amount = consumTransactionDto.Amount;
                    resp.VoiceID = "null";
                    resp.Text = "";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Ok(resp);
        }

        /// <summary>
        ///消费记录查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/hxz/v1/transaction")]
        public async Task<IActionResult> Transaction([FromBody] TransactionDto transactionDto)
        {
            TransactionResponse resp = new TransactionResponse();

            _logger.LogError(JsonConvert.SerializeObject(transactionDto));

            try
            {
                var lastOrder =  _coreOrderServices.QueryListByClause(p => true).OrderByDescending(p=>p.createTime).Skip(transactionDto.Number - 1).Take(1).FirstOrDefault();

                if (lastOrder!=null)
                {
                    resp.Status = 1;
                    resp.Msg = "Success";
                    resp.Name = lastOrder.userName;
                    resp.CardNo = lastOrder.cardNo;
                    resp.Number = transactionDto.Number.ToString();
                    resp.Time = lastOrder.createTime.ToString("yyyyMMddHHmmss");
                    resp.Consume = lastOrder.amount.ToString();
                    resp.Balance = lastOrder.balance.ToString();
                    resp.Class = "消费";
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "订单数据不存在";
                    resp.Name = "";
                    resp.CardNo = ""; 
                    resp.Number = transactionDto.Number.ToString();
                    resp.Time =DateTime.Now.ToString("yyyyMMddHHmmss");
                    resp.Consume ="";
                    resp.Balance = "";
                    resp.Class = "消费";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Ok(resp);
        }

        /// <summary>
        ///终端查询日统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/hxz/v1/Counts")]
        public async Task<IActionResult> Counts()
        {
            CountsResponse resp = new CountsResponse();
            try
            {
                resp.Status = 1;

                resp.Msg = "Success";

                var start = DateTime.Now.AddDays(-1);
                 
                var orderList = await _coreOrderServices.QueryListByClauseAsync(p => p.createTime>= start);

                var dateTotalMoney = orderList.Sum(p => p.totalPrice);

                var dateTotalTime = orderList.Count;

                resp.Text = $"日总金额：{dateTotalMoney};日总次数：{dateTotalTime}";
            }
            catch (Exception ex)
            {
                resp.Status = 0;

                resp.Msg = "Failed";

                resp.Text = "";

                _logger.LogError(ex.Message);
            }

            return Ok(resp);
        }

        /// <summary>
        ///菜品查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/hxz/v1/menus")]
        public async Task<IActionResult> Menus([FromBody] MenusDto menusDto)
        {
            var resp = new MenusResponse();
            resp.Status = 1;
            resp.Msg = "Success";
            resp.Name = "testMenu";
            resp.Price = "9.999";
            return Ok(resp);
        }

        /// <summary>
        ///记账扣费流水上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/hxz/v1/OffLines")]
        public async Task<IActionResult> OffLines([FromBody] OffLinesDto offLinesDto)
        {
            var resp = new OffLinesResponse();
            resp.Status = 1;
            resp.Msg = "Success";
            resp.Order = "11111";
            return Ok(resp);
        }

        #region MyRegion




        ///// <summary>
        /////付款码（二维码）支付
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("/hxz/v1/QRCodeTransaction")]
        //public async Task<IActionResult> QRCodeTransaction([FromBody] QRCodeTransactionDto qRCodeTransactionDto)
        //{
        //    var jm = new CallBackResult<QRCodeTransactionResponse>();
        //    QRCodeTransactionRequestModel qRCodeTransactionRequestModel = new QRCodeTransactionRequestModel()
        //    {

        //    };
        //    QRCodeTransactionRequest req = new QRCodeTransactionRequest(qRCodeTransactionRequestModel);
        //    var res = _cloudCostToolClient.Post(req);
        //    jm.Success(res, "调用成功");
        //    return Ok(jm);
        //}

        ///// <summary>
        /////二维码支付结果查询
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("/hxz/v1/TransactionInquiry")]
        //public async Task<IActionResult> TransactionInquiry([FromBody] TransactionInquiryDto transactionInquiryDto)
        //{
        //    var jm = new CallBackResult<TransactionInquiryResponse>();
        //    TransactionInquiryRequestModel TransactionInquiryRequestModel = new TransactionInquiryRequestModel()
        //    {

        //    };
        //    TransactionInquiryRequest req = new TransactionInquiryRequest(TransactionInquiryRequestModel);
        //    var res = _cloudCostToolClient.Post(req);
        //    jm.Success(res, "调用成功");
        //    return Ok(jm);
        //}




        #endregion
    }
}
