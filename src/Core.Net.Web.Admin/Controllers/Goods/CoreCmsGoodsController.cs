using Core.Net.Configuration;
using Core.Net.Entity.Dtos;
using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.Model.Goods;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Common;
using Core.Net.Service.Goods;
using Core.Net.Service.Shops;
using Core.Net.Util.Extensions;
using Core.Net.Util.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Net.Web.Admin.Controllers.Goods
{
    /// <summary>
    ///  商品表
    /// </summary>
    [Description("商品表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
   
    public class CoreCmsGoodsController : ControllerBase
    {
        private readonly IBrandServices _brandServices;
        //private readonly ICoreCmsGoodsCategoryExtendServices _categoryExtendServices;
        private readonly ICoreCmsGoodsCategoryServices _coreCmsGoodsCategoryServices;
        private readonly ICoreCmsGoodsServices _coreCmsGoodsServices;
        //private readonly ICoreCmsGoodsGradeServices _goodsGradeServices;
        private readonly ICoreCmsGoodsParamsServices _goodsParamsServices;
        private readonly ICoreCmsGoodsTypeServices _goodTypeServices;
        private readonly ICoreCmsLabelServices _labelServices;
        //private readonly ICoreCmsProductsServices _productsServices;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly ICoreCmsGoodsTypeParamsServices _typeParamsServices;
        private readonly ICoreCmsGoodsTypeSpecRelServices _typeSpecRelServices;
        private readonly ICoreCmsGoodsTypeSpecServices _typeSpecServices;
        private readonly ICoreCmsGoodsTypeSpecValueServices _typeSpecValueServices;
        //private readonly ICoreCmsUserGradeServices _userGradeServices;
        //private readonly ICoreCmsProductsDistributionServices _productsDistributionServices;
        private readonly IWebHostEnvironment _webHostEnvironment;


        /// <summary>
        ///     构造函数
        /// </summary>
        public CoreCmsGoodsController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsGoodsServices coreCmsGoodsServices
            , ICoreCmsSettingServices settingServices
            , IBrandServices brandServices
            , ICoreCmsGoodsTypeServices goodTypeServices
            , ICoreCmsGoodsCategoryServices coreCmsGoodsCategoryServices
            //, ICoreCmsUserGradeServices userGradeServices
            , ICoreCmsGoodsTypeParamsServices typeParamsServices
            , ICoreCmsGoodsParamsServices goodsParamsServices
            , ICoreCmsGoodsTypeSpecRelServices typeSpecRelServices
            , ICoreCmsGoodsTypeSpecServices typeSpecServices
            , ICoreCmsGoodsTypeSpecValueServices typeSpecValueServices
            //, ICoreCmsGoodsGradeServices goodsGradeServices
            //, ICoreCmsProductsServices productsServices
            //, ICoreCmsGoodsCategoryExtendServices categoryExtendServices
            , ICoreCmsLabelServices labelServices
           /* , ICoreCmsProductsDistributionServices productsDistributionServices*/)
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsGoodsServices = coreCmsGoodsServices;
            _settingServices = settingServices;
            _brandServices = brandServices;
            _goodTypeServices = goodTypeServices;
            _coreCmsGoodsCategoryServices = coreCmsGoodsCategoryServices;
            //_userGradeServices = userGradeServices;
            _typeParamsServices = typeParamsServices;
            _goodsParamsServices = goodsParamsServices;
            _typeSpecRelServices = typeSpecRelServices;
            _typeSpecServices = typeSpecServices;
            _typeSpecValueServices = typeSpecValueServices;
            //_goodsGradeServices = goodsGradeServices;
            //_productsServices = productsServices;
            //_categoryExtendServices = categoryExtendServices;
            _labelServices = labelServices;
            //_productsDistributionServices = productsDistributionServices;
        }

        #region 首页数据============================================================

        // POST: Api/CoreCmsGoods/GetIndex
        /// <summary>
        ///     首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public async Task<JsonResult> GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            var totalGoods = await _coreCmsGoodsServices.GetCountAsync(p => p.id > 0 && p.isDel == false);
            var totalMarketableUp = await _coreCmsGoodsServices.GetCountAsync(p => p.isMarketable && p.isDel == false);
            var totalMarketableDown =
                await _coreCmsGoodsServices.GetCountAsync(p => p.isMarketable == false && p.isDel == false);

            //获取库存

            var totalWarn = 0;

            //var allConfigs = await _settingServices.GetConfigDictionaries();
            //var kc = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.GoodsStocksWarn);
            //if (kc != null)
            //{
            //    var kcNumer = kc.ObjectToInt();
            //    totalWarn = await _coreCmsGoodsServices.GetCountAsync(p =>
            //        p.stock <= kcNumer && p.isDel == false && p.isMarketable);
            //}
            //else
            //{
            //    totalWarn = await _coreCmsGoodsServices.GetCountAsync(p =>
            //        p.stock <= 0 && p.isDel == false && p.isMarketable);
            //}

            //获取商品分类
            var categories = await _coreCmsGoodsCategoryServices.QueryListByClauseAsync(p => p.isShow, p => p.sort,
                OrderByType.Asc);
            //获取商品类别
            var types = await _goodTypeServices.QueryAsync();
            //获取品牌
            var brands = await _brandServices.QueryAsync();

            //获取商品分销方式
            var productsDistributionType = EnumHelper.EnumToList<GlobalEnumVars.ProductsDistributionType>();

            jm.data = new
            {
                totalGoods,
                totalMarketableUp,
                totalMarketableDown,
                totalWarn,
                categories = GoodsHelper.GetTree(categories),
                categoriesAll = categories,
                types,
                brands,
                productsDistributionType
            };

            return new JsonResult(jm);
        }

        #endregion

        #region 获取列表============================================================

        // POST: Api/CoreCmsGoods/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsGoods>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsGoods, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "bn":
                    orderEx = p => p.bn;
                    break;
                case "name":
                    orderEx = p => p.name;
                    break;
                case "brief":
                    orderEx = p => p.brief;
                    break;
                case "price":
                    orderEx = p => p.price;
                    break;
                case "costprice":
                    orderEx = p => p.costprice;
                    break;
                case "mktprice":
                    orderEx = p => p.mktprice;
                    break;
                case "images":
                    orderEx = p => p.images;
                    break;
                case "goodsCategoryId":
                    orderEx = p => p.goodsCategoryId;
                    break;
                case "goodsTypeId":
                    orderEx = p => p.goodsTypeId;
                    break;
                case "brandId":
                    orderEx = p => p.brandId;
                    break;
                case "isNomalVirtual":
                    orderEx = p => p.isNomalVirtual;
                    break;
                case "isMarketable":
                    orderEx = p => p.isMarketable;
                    break;
                case "stock":
                    orderEx = p => p.stock;  //这里的
                    break;
                case "freezeStock":
                    orderEx = p => p.freezeStock; //这里的
                    break;
                case "weight":
                    orderEx = p => p.weight; //这里的
                    break;
                case "unit":
                    orderEx = p => p.unit;
                    break;
                case "intro":
                    orderEx = p => p.intro;
                    break;
                case "spesDesc":
                    orderEx = p => p.spesDesc;
                    break;
                case "parameters":
                    orderEx = p => p.parameters;
                    break;
                case "commentsCount":
                    orderEx = p => p.commentsCount;
                    break;
                case "viewCount":
                    orderEx = p => p.viewCount;
                    break;
                case "buyCount":
                    orderEx = p => p.buyCount;
                    break;
                case "uptime":
                    orderEx = p => p.uptime;
                    break;
                case "downtime":
                    orderEx = p => p.downtime;
                    break;
                case "sort":
                    orderEx = p => p.sort;
                    break;
                case "labelIds":
                    orderEx = p => p.labelIds;
                    break;
                case "newSpec":
                    orderEx = p => p.newSpec;
                    break;
                case "createTime":
                    orderEx = p => p.createTime;
                    break;
                case "updateTime":
                    orderEx = p => p.updateTime;
                    break;
                case "isRecommend":
                    orderEx = p => p.isRecommend;
                    break;
                case "isHot":
                    orderEx = p => p.isHot;
                    break;
                case "isDel":
                    orderEx = p => p.isDel;
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

            //商品编码 nvarchar
            var bn = Request.Form["bn"].FirstOrDefault();
            if (!string.IsNullOrEmpty(bn)) @where = @where.And(p => p.bn.Contains(bn));
            //商品名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) @where = @where.And(p => p.name.Contains(name));
            //商品分类ID 关联category.id int
            var selectTreeSelectNodeId = Request.Form["selectTree_select_nodeId"].FirstOrDefault().ObjectToInt(0);
            if (selectTreeSelectNodeId > 0) @where = @where.And(p => p.goodsCategoryId == selectTreeSelectNodeId);
            //商品分类ID 关联category.id int
            var goodsCategoryId = Request.Form["goodsCategoryId"].FirstOrDefault().ObjectToInt(0);
            if (goodsCategoryId > 0) @where = @where.And(p => p.goodsCategoryId == goodsCategoryId);
            //商品类别ID 关联goods_type.id int
            var goodsTypeId = Request.Form["goodsTypeId"].FirstOrDefault().ObjectToInt(0);
            if (goodsTypeId > 0) @where = @where.And(p => p.goodsTypeId == goodsTypeId);
            //品牌ID 关联brand.id int
            var brandId = Request.Form["brandId"].FirstOrDefault().ObjectToInt(0);
            if (brandId > 0) @where = @where.And(p => p.brandId == brandId);
            //是否上架 bit
            var isMarketable = Request.Form["isMarketable"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isMarketable) && isMarketable.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isMarketable);
            else if (!string.IsNullOrEmpty(isMarketable) && isMarketable.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isMarketable == false);
            //是否推荐 bit
            var isRecommend = Request.Form["isRecommend"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isRecommend) && isRecommend.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isRecommend);
            else if (!string.IsNullOrEmpty(isRecommend) && isRecommend.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isRecommend == false);
            //是否热门 bit
            var isHot = Request.Form["isHot"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isHot) && isHot.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isHot);
            else if (!string.IsNullOrEmpty(isHot) && isHot.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isHot == false);
            ////是否报警
            //var warn = Request.Form["warn"].FirstOrDefault();
            //if (!string.IsNullOrEmpty(warn) && warn.ToLowerInvariant() == "true")
            //{
            //    //获取库存
            //    var allConfigs = await _settingServices.GetConfigDictionaries();
            //    var kc = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.GoodsStocksWarn);
            //    if (kc != null)
            //    {
            //        var kcNumer = kc.ObjectToInt();
            //        where = where.And(p => p.stock <= kcNumer && p.isDel == false && p.isMarketable);
            //    }
            //    else
            //    {
            //        where = where.And(p => p.stock <= 0 && p.isDel == false && p.isMarketable);
            //    }
            //}

            where = where.And(p => p.isDel == false);

            //获取数据
            var list = await _coreCmsGoodsServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);

            if (list != null && list.Any())
            {
                var labels = await _labelServices.QueryAsync();

                foreach (var item in list)
                    if (!string.IsNullOrEmpty(item.labelIds))
                    {
                        var ids = CommonHelper.StringToIntArray(item.labelIds);
                        item.labels = labels.Where(p => ids.Contains(p.id)).ToList();
                    }
            }


            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }

        #endregion

        #region 设置是否虚拟商品============================================================

        // POST: Api/CoreCmsGoods/DoSetisNomalVirtual/10
        /// <summary>
        ///     设置是否虚拟商品
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否虚拟商品")]
        public async Task<JsonResult> DoSetisNomalVirtual([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsGoodsServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            oldModel.isNomalVirtual = entity.data;

            var bl = await _coreCmsGoodsServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
        }

        #endregion

        #region 设置是否上架============================================================

        // POST: Api/CoreCmsGoods/DoSetisMarketable/10
        /// <summary>
        ///     设置是否上架
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否上架")]
        public async Task<JsonResult> DoSetisMarketable([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsGoodsServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            oldModel.isMarketable = entity.data;

            var bl = await _coreCmsGoodsServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
        }

        #endregion

        #region 设置是否推荐============================================================

        // POST: Api/CoreCmsGoods/DoSetisRecommend/10
        /// <summary>
        ///     设置是否推荐
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否推荐")]
        public async Task<JsonResult> DoSetisRecommend([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsGoodsServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            oldModel.isRecommend = entity.data;

            var bl = await _coreCmsGoodsServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
        }

        #endregion

        #region 设置是否热门============================================================

        // POST: Api/CoreCmsGoods/DoSetisHot/10
        /// <summary>
        ///     设置是否热门
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否热门")]
        public async Task<JsonResult> DoSetisHot([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsGoodsServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            oldModel.isHot = entity.data;

            var bl = await _coreCmsGoodsServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
        }

        #endregion

        #region 设置是否删除============================================================

        // POST: Api/CoreCmsGoods/DoSetisDel/10
        /// <summary>
        ///     设置是否删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否删除")]
        public async Task<JsonResult> DoSetisDel([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsGoodsServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            oldModel.isDel = entity.data;

            var bl = await _coreCmsGoodsServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
        }

        #endregion

        #region 设置标签============================================================

        // POST: Api/CoreCmsGoods/GetLabel
        /// <summary>
        ///     设置标签
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置标签")]
        public async Task<JsonResult> GetLabel([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();
            var model = await _labelServices.QueryAsync();
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            jm.code = 0;
            jm.data = new
            {
                labels = model,
                ids = entity
            };

            return new JsonResult(jm);
        }

        // POST: Admins/CoreCmsGoods/DoSetLabel
        /// <summary>
        ///     设置标签提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置标签提交")]
        public async Task<JsonResult> DoSetLabel([FromBody] FmSetLabel entity)
        {
            var jm = await _coreCmsGoodsServices.DoSetLabel(entity);
            return new JsonResult(jm);
        }

        #endregion

        #region 去除标签============================================================

        // POST: Api/CoreCmsGoods/GetDeleteLabel
        /// <summary>
        ///     去除标签
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("去除标签")]
        public async Task<JsonResult> GetDeleteLabel([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsGoodsServices.QueryByIDsAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            var labelIds = model.Select(p => p.labelIds).ToList();
            var r = new List<int>();
            labelIds.ForEach(p =>
            {
                var arr = CommonHelper.StringToIntArray(p);
                r.AddRange(arr);
            });

            var labels = _labelServices.QueryListByClause(p => r.Contains(p.id));

            jm.code = 0;
            jm.data = new
            {
                labels,
                ids = entity
            };


            return new JsonResult(jm);
        }

        // POST: Admins/CoreCmsGoods/DoDeleteLabel
        /// <summary>
        ///     去除标签提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("去除标签提交")]
        public async Task<JsonResult> DoDeleteLabel([FromBody] FmSetLabel entity)
        {
            var jm = await _coreCmsGoodsServices.DoDeleteLabel(entity);
            return new JsonResult(jm);
        }

        #endregion

        #region 批量上架============================================================

        // POST: Api/CoreCmsGoods/DoBatchMarketableUp/10,11,20
        /// <summary>
        ///     批量上架
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量上架")]
        public async Task<JsonResult> DoBatchMarketableUp([FromBody] FMArrayIntIds entity)
        {
            var jm = await _coreCmsGoodsServices.DoBatchMarketableUp(entity.id);
            return new JsonResult(jm);
        }

        #endregion

        #region 批量下架============================================================

        // POST: Api/CoreCmsGoods/DoBatchMarketableDown/10,11,20
        /// <summary>
        ///     批量下架
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量下架")]
        public async Task<JsonResult> DoBatchMarketableDown([FromBody] FMArrayIntIds entity)
        {
            var jm = await _coreCmsGoodsServices.DoBatchMarketableDown(entity.id);
            return new JsonResult(jm);
        }

        #endregion

        #region 批量删除============================================================

        // POST: Api/CoreCmsGoods/DoBatchDelete/10,11,20
        /// <summary>
        ///     批量删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量删除")]
        public async Task<JsonResult> DoBatchDelete([FromBody] FMArrayIntIds entity)
        {
            var jm = await _coreCmsGoodsServices.DeleteByIdsAsync(entity.id);
            return new JsonResult(jm);
        }

        #endregion

        #region 批量修改价格===========================================================

        //// POST: Api/CoreCmsGoods/GetBatchModifyPrice
        ///// <summary>
        /////     批量修改价格
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Description("批量修改价格")]
        //public JsonResult GetBatchModifyPrice([FromBody] FMArrayIntIds entity)
        //{
        //    var jm = new AdminUiCallBack();

        //    var priceType = EnumHelper.EnumToList<GlobalEnumVars.PriceType>();
        //    var userGrade = _userGradeServices.Query();
        //    if (userGrade.Any())
        //        userGrade.ForEach(p =>
        //        {
        //            priceType.Add(new EnumEntity
        //            {
        //                description = p.title,
        //                title = "grade_price_" + p.id,
        //                value = 10000 + p.id
        //            });
        //        });
        //    jm.code = 0;
        //    jm.data = new
        //    {
        //        entity,
        //        priceType
        //    };

        //    return new JsonResult(jm);
        //}


        //// POST: Api/CoreCmsGoods/DoBatchModifyPrice
        ///// <summary>
        /////     批量修改价格提交
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Description("批量修改价格提交")]
        //public async Task<JsonResult> DoBatchModifyPrice([FromBody] FmBatchModifyPrice entity)
        //{
        //    var jm = await _coreCmsGoodsServices.DoBatchModifyPrice(entity);
        //    return new JsonResult(jm);
        //}

        #endregion
    }
}
