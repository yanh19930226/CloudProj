using Core.Net.Data;
using Core.Net.Entity.Model.Shops;
using Core.Net.Entity.ViewModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Shops
{
    public interface ICoreCmsNoticeServices : IBaseServices<CoreCmsNotice>
    {

        /// <summary>
        /// sqlsugar分组聚合例子
        /// </summary>
        /// <returns></returns>
        Task<IPageList<CoreCmsNoticeViewModel>> GetNotice(Expression<Func<CoreCmsNotice, bool>> predicate, Expression<Func<CoreCmsNotice, object>> orderByExpression, OrderByType orderByType,
            int pageIndex = 1, int pageSize = 20, bool blUseNoLock = false);
    }
}
