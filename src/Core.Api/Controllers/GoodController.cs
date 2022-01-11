using Core.Api.Models.Dtos;
using Core.Net.Data;
using Core.Net.Entity.Model.DinnerCard;
using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.DinnerCards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Controllers
{
    /// <summary>
    /// 商品控制器
    /// </summary>
    public class GoodController : BaseController
    {
        private readonly ICoreGoodsServices _coreGoodsServices;
        private readonly IBusinessServices _businessServices;
        /// <summary>
        /// GoodController
        /// </summary>
        /// <param name="coreGoodsServices"></param>
        public GoodController(ICoreGoodsServices coreGoodsServices,IBusinessServices businessServices)
        {
            _coreGoodsServices = coreGoodsServices;
            _businessServices = businessServices;
        }

        #region 商家分页列表 ============================================================
        /// <summary>
        /// 商家分页列表
        /// </summary>
        /// <param name="businessSearchDto">搜索参数</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetBusinessPageList([FromBody] BusinessSearchDto businessSearchDto)
        {

            var jm = new PageCallBackResult<IPageList<Business>>();

            var where = PredicateBuilder.True<Business>();


            if (!string.IsNullOrEmpty(businessSearchDto.businessText))
            {
                where = where.And(p => p.businessName.Contains(businessSearchDto.businessText));
            }
            var list = await _businessServices.QueryPageAsync(where, p => p.createTime, OrderByType.Desc, businessSearchDto.page, businessSearchDto.limit);

            jm.Count = list.TotalCount;

            jm.Success(list, "数据调用成功");

            return Ok(jm);
        }
        #endregion

        #region 商品列表 ============================================================
        /// <summary>
        /// 商品分页列表
        /// </summary>
        /// <param name="goodSearchDto">搜索参数</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetGoodPageList([FromBody] GoodSearchDto goodSearchDto)
        {

            var jm = new PageCallBackResult<IPageList<CoreGoods>>();

            var where = PredicateBuilder.True<CoreGoods>();

            if (!string.IsNullOrEmpty(goodSearchDto.goodText))
            {
                where = where.And(p => p.goodName.Contains(goodSearchDto.goodText));
            }
            var list = await _coreGoodsServices.QueryPageAsync(where, p => p.createTime, OrderByType.Desc, goodSearchDto.page, goodSearchDto.limit);

            jm.Count = list.TotalCount;

            jm.Success(list, "数据调用成功");

            return Ok(jm);
        }
        #endregion
    }
}
