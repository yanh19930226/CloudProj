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
    public interface ICoreCmsClerkServices : IBaseServices<CoreCmsClerk>
    {
        /// <summary>
        ///     获取门店关联用户分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<IPageList<StoreClerkDto>> QueryStoreClerkDtoPageAsync(Expression<Func<StoreClerkDto, bool>> predicate,
            Expression<Func<StoreClerkDto, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);
    }
}
