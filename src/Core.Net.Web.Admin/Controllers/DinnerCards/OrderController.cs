using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Net.Configuration;
using Core.Net.Data;
using Core.Net.Entity.Dtos;
using Core.Net.Entity.Enums;
using Core.Net.Entity.Model.DinnerCard;
using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.Model.Systems;
using Core.Net.Entity.ViewModels;
using Core.Net.Filter;
using Core.Net.Service.DinnerCards;
using Core.Net.Service.Goods;
using Core.Net.Service.Systems;
using Core.Net.Util.Extensions;
using Core.Net.Util.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using SqlSugar;

namespace Core.Net.Web.Admin.Controllers.DinnerCards
{

    /// <summary>
    ///     订单表
    /// </summary>
    [Description("订单表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class OrderController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDinnerCardServices _dinnerCardServices;
        private readonly ICoreOrderServices _coreOrderServices;
        private readonly ICoreGoodOrderDetailServices _coreGoodOrderDetailServices;
        private readonly ISysRoleServices _sysRoleServices;
        private readonly ISysOrganizationServices _sysOrganizationServices;
        private readonly IBusinessServices _businessServices;
        private readonly ICoreGoodsServices _coreGoodsServices;
        private readonly ISysDictionaryServices _sysDictionaryServices;
        private readonly ISysDictionaryDataServices _sysDictionaryDataServices;
        private readonly ICoreCmsGoodsCategoryServices _coreCmsGoodsCategoryServices;
        private readonly IUnitOfWork _unitOfWork;

        ///  <summary>
        ///  构造函数
        /// </summary>
        ///   <param name="webHostEnvironment"></param>
        /// <param name="coreCmsArticleTypeServices"></param>
        ///  <param name="coreCmsArticleServices"></param>
        public OrderController(
            IWebHostEnvironment webHostEnvironment,
            IDinnerCardServices dinnerCardServices,
        ISysDictionaryServices sysDictionaryServices,
            IBusinessServices businessServices,
             ISysRoleServices sysRoleServices,
             IUnitOfWork unitOfWork,
              ISysOrganizationServices sysOrganizationServices,
            ISysDictionaryDataServices sysDictionaryDataServices,
             ICoreOrderServices coreOrderServices,
            ICoreCmsGoodsCategoryServices coreCmsGoodsCategoryServices,
            ICoreGoodOrderDetailServices coreGoodOrderDetailServices,
            ICoreGoodsServices coreGoodsServices
            )
        {
            _unitOfWork = unitOfWork;
            _sysRoleServices = sysRoleServices;
            _sysOrganizationServices = sysOrganizationServices;
            _webHostEnvironment = webHostEnvironment;
            _businessServices = businessServices;
            _sysDictionaryServices = sysDictionaryServices;
            _sysDictionaryDataServices = sysDictionaryDataServices;
            _dinnerCardServices = dinnerCardServices;
            _coreGoodsServices = coreGoodsServices;
            _coreOrderServices = coreOrderServices;
            _coreGoodOrderDetailServices = coreGoodOrderDetailServices;
            _coreCmsGoodsCategoryServices = coreCmsGoodsCategoryServices;
        }
        #region 首页数据============================================================
        // POST: Api/CoreCmsArticleType/GetIndex
        /// <summary>
        /// 首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public JsonResult GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            var dict = _sysDictionaryServices.QueryByClause(p => p.dictCode == "orderStatus");
            var dictData = new List<SysDictionaryData>();
            if (dict != null)
            {
                dictData = _sysDictionaryDataServices.QueryListByClause(p => p.dictId == dict.id);
            }
            //机构
            List<KV> orgkVs = new List<KV>();
            var organizations = _sysOrganizationServices.Query();
            foreach (var item in organizations)
            {
                KV kV = new KV();
                kV.Key = item.id.ToString();
                kV.Value = item.organizationName.ToString();
                orgkVs.Add(kV);
            }
            jm.data = new
            {
                dictData,
                orgkVs,
            };
            return Json(jm);
        }
        #endregion

        #region 获取列表============================================================
        // POST: Api/DinnerCard/GetPageList
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<JsonResult> GetPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreOrder>();
            where = where.And(p => p.orderType == (int)OrderTypeEnum.GoodOrder);
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreOrder, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "createTime":
                    orderEx = p => p.createTime;
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

            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0) @where = @where.And(p => p.status == status);

            var organizationId = Request.Form["organizationId"].FirstOrDefault().ObjectToInt(0);
            if (organizationId > 0) @where = @where.And(p => p.organizationId == organizationId);

            var text = Request.Form["text"].FirstOrDefault();
            if (!string.IsNullOrEmpty(text)) @where = @where.And(p => p.orderNo.Contains(text) || p.userName.Contains(text)|| p.telePhone.Contains(text));

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

            var list = await _coreOrderServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);

            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return Json(jm);
        }
        #endregion

        #region 预览数据============================================================
        // POST: Api/CoreCmsAgentGoods/GetDetails/10
        /// <summary>
        /// 预览数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("预览数据")]
        public async Task<JsonResult> GetDetails([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var order = await _coreOrderServices.QueryByClauseAsync(p => p.id == entity.id);
           
            if (order == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            var detail = await _coreGoodOrderDetailServices.QueryListByClauseAsync(p => p.orderNo == order.orderNo);

            jm.code = 0;

            jm.data = new
            {
                order,
                detail
            };
            return new JsonResult(jm);
        }
        #endregion

        #region 取消订单============================================================
        // POST: Api/CoreCmsArticleType/DoDelete/10
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("取消订单")]
        public async Task<JsonResult> DoCancel([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();
            var model = await _coreOrderServices.QueryByClauseAsync(p => p.id == entity.id);
            model.status = (int)OrderStatusEnum.Canceled;
            //事物处理过程结束
            var bl = await _coreOrderServices.UpdateAsync(model);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            return new JsonResult(jm);
        }
        #endregion

        #region 确认收货============================================================
        // POST: Api/CoreCmsArticleType/DoDelete/10
        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("确认收货")]
        public async Task<JsonResult> DoConfirm([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();
            var model = await _coreOrderServices.QueryByClauseAsync(p=>p.id==entity.id);
            model.status = (int)OrderStatusEnum.Complete;
            //事物处理过程结束
            var bl = await _coreOrderServices.UpdateAsync(model);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            return new JsonResult(jm);
        }
        #endregion

        #region 批量确认============================================================

        // POST: Api/CoreCmsGoods/DoBatchDelete/10,11,20
        /// <summary>
        ///     批量确认
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量确认")]
        public async Task<JsonResult> DoBatchConfirm([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            var list = await _coreOrderServices.QueryListByClauseAsync(p => entity.id.Contains(p.id));
            foreach (var item in list)
            {
                item.status = (int)OrderStatusEnum.Complete;
            }
            var bl = await _coreOrderServices.UpdateAsync(list);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? "确认成功" : "确认失败";
            return new JsonResult(jm);
        }

        #endregion

        #region 批量取消============================================================

        // POST: Api/CoreCmsGoods/DoBatchMarketableUp/10,11,20
        /// <summary>
        ///     批量取消
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量取消")]
        public async Task<JsonResult> DoBatchCancel([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            var list =await  _coreOrderServices.QueryListByClauseAsync(p => entity.id.Contains(p.id));
            foreach (var item in list)
            {
                item.status =(int)OrderStatusEnum.Canceled;
            }
            var bl = await _coreOrderServices.UpdateAsync(list);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? "取消成功" : "取消失败";
            return new JsonResult(jm);
        }

        #endregion

        #region 选择导出============================================================

        // POST: Api/SysLoginRecord/SelectExportExcel/10
        /// <summary>
        ///     选择导出
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("选择导出")]
        public async Task<JsonResult> SelectExcel([FromBody] FMArrayStrIds entity)
        {
            var jm = new AdminUiCallBack();

            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var sheet1 = book.CreateSheet("Sheet1");
           
            //获取list数据
            var listmodel = await _coreGoodOrderDetailServices.QueryListByClauseAsync(p => entity.orderNo.Contains(p.orderNo),
                p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("订单号");
            row1.CreateCell(1).SetCellValue("购买人");
            row1.CreateCell(2).SetCellValue("联系电话");
            row1.CreateCell(3).SetCellValue("商品编号");
            row1.CreateCell(4).SetCellValue("商品名称");
            row1.CreateCell(5).SetCellValue("商品价格");
            row1.CreateCell(6).SetCellValue("订购量");
            row1.CreateCell(7).SetCellValue("实发量");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var order = _coreOrderServices.QueryByClause(p=>p.orderNo== listmodel[i].orderNo);
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].orderNo.ToString());
                rowtemp.CreateCell(1).SetCellValue(order.userName);
                rowtemp.CreateCell(2).SetCellValue(order.telePhone);
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].goodNo);
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].goodName);
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].unitPrice.ToString());
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].goodNum);
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].realNum.ToString());
            }

            // 导出excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-商品订单导出结果.xlsx";
            var filePath = webRootPath + tpath;
            var di = new DirectoryInfo(filePath);
            if (!di.Exists) di.Create();
            var fileHssf = new FileStream(filePath + fileName, FileMode.Create);
            book.Write(fileHssf);
            fileHssf.Close();

            jm.code = 0;
            jm.msg = GlobalConstVars.ExcelExportSuccess;
            jm.data = tpath + fileName;

            return new JsonResult(jm);
        }

        #endregion

        #region 导入
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
                    _unitOfWork.BeginTran();
                    using (var stream = new MemoryStream())
                    {
                        file.CopyToAsync(stream);//取到文件流
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        using (var dt = new Excel().ReadExcel(stream))
                        {
                            for (int row = 0; row < dt.Rows.Count; row++)
                            {
                                if (dt.Rows[row]["订单号"] == null)
                                {
                                    jm.msg = "订单号不能为空";
                                    _unitOfWork.RollbackTran();
                                    return new JsonResult(jm);
                                }
                                if (dt.Rows[row]["商品编号"] == null)
                                {
                                    jm.msg = "商品编号不能为空";
                                    _unitOfWork.RollbackTran();
                                    return new JsonResult(jm);
                                }
                                if (dt.Rows[row]["订购量"] == null)
                                {
                                    jm.msg = "订购量不能为空";
                                    _unitOfWork.RollbackTran();
                                    return new JsonResult(jm);
                                }
                                if (dt.Rows[row]["实发量"] == null)
                                {
                                    jm.msg = "实发量不能为空";
                                    _unitOfWork.RollbackTran();
                                    return new JsonResult(jm);
                                }

                                var orderNo = dt.Rows[row]["订单号"].ToString();
                                var goodNo = dt.Rows[row]["商品编号"].ToString();
                                var orderdetail = _coreGoodOrderDetailServices.QueryByClause(p => p.orderNo == orderNo && p.goodNo == goodNo);
                                var realNum = (int)(Convert.ToInt64(dt.Rows[row]["实发量"].ToString()));
                                if (realNum> orderdetail.goodNum)
                                {
                                    jm.msg = "实发量不能大于订购数量";
                                    _unitOfWork.RollbackTran();
                                    return new JsonResult(jm);
                                }
                                orderdetail.realNum = realNum;
                                _coreGoodOrderDetailServices.Update(orderdetail);
                            }
                        }
                    }
                    _unitOfWork.CommitTran();
                    jm.msg = "导入成功";
                    return new JsonResult(jm);
                }
                catch (Exception ex)
                {
                    _unitOfWork.RollbackTran();
                    return new JsonResult(ex.Message);
                }
            }
            return new JsonResult(jm);
        }
        #endregion

    }
}
