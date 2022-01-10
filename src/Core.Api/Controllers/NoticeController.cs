using Core.Api.Models.Dtos;
using Core.Net.Data;
using Core.Net.Entity.Model.Expression;
using Core.Net.Entity.Model.Shops;
using Core.Net.Entity.Model.Systems;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Shops;
using Core.Net.Service.Systems;
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
    /// 公告控制器
    /// </summary>
    public class NoticeController : BaseController
    {
        private readonly ICoreCmsNoticeServices _coreCmsNoticeServices;
        private readonly ISysDictionaryServices _sysDictionaryServices;
        private readonly ISysDictionaryDataServices _sysDictionaryDataServices;


        /// <summary>
        /// GoodController
        /// </summary>
        /// <param name="coreCmsNoticeServices"></param>
        public NoticeController(ICoreCmsNoticeServices coreCmsNoticeServices, ISysDictionaryServices sysDictionaryServices, ISysDictionaryDataServices sysDictionaryDataServices)
        {
            _coreCmsNoticeServices = coreCmsNoticeServices;
            _sysDictionaryServices = sysDictionaryServices;
            _sysDictionaryDataServices = sysDictionaryDataServices;
        }
        #region 公告分页列表 ============================================================
        /// <summary>
        /// 公告分页列表
        /// </summary>
        /// <param name="noticeSearchDto">搜索参数</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetNoticePageList([FromBody] NoticeSearchDto noticeSearchDto)
        {
            var jm = new PageCallBackResult<IPageList<CoreCmsNoticeViewModel>>();

            var where = PredicateBuilder.True<CoreCmsNotice>();

            if (!string.IsNullOrEmpty(noticeSearchDto.noticeText))
            {
                where = where.And(p => p.title.Contains(noticeSearchDto.noticeText));
            }
            var list = await _coreCmsNoticeServices.GetNotice(where, p => p.createTime, OrderByType.Desc, noticeSearchDto.page, noticeSearchDto.limit);
          
            jm.Count = list.TotalCount;

            jm.Success(list, "数据调用成功");

            return Ok(jm);
        }
        #endregion
    }
}
