using Core.Net.Data;
using Core.Net.Data.Impl;
using Core.Net.Entity.Model.Shops;
using Core.Net.Entity.Model.Users;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Shops;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Impl.Shops
{
    public class CoreCmsClerkServices : BaseServices<CoreCmsClerk>, ICoreCmsClerkServices
    {
        private readonly IBaseRepository<CoreCmsClerk> _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlSugarClient _sqlSugarClient;
        public CoreCmsClerkServices(IBaseRepository<CoreCmsClerk> dal, IUnitOfWork unitOfWork, SqlSugarClient sqlSugarClient)
        {
            _dal = dal;
            base.BaseDal = dal;
            _sqlSugarClient = sqlSugarClient;
            _unitOfWork = unitOfWork;
        }

        #region 获取门店关联用户分页数据
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
        public async Task<IPageList<StoreClerkDto>> QueryStoreClerkDtoPageAsync(Expression<Func<StoreClerkDto, bool>> predicate,
            Expression<Func<StoreClerkDto, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<StoreClerkDto> page;
            if (blUseNoLock)
            {
                page = await _sqlSugarClient.Queryable<CoreCmsClerk, CoreCmsStore, CoreCmsUser>((p, sst, ccu) => new JoinQueryInfos(
                        JoinType.Left, p.storeId == sst.id,
                        JoinType.Left, p.userId == ccu.id
                        ))
                    .Select((p, sst, ccu) => new StoreClerkDto
                    {
                        id = p.id,
                        storeId = p.storeId,
                        userId = p.userId,
                        isDel = p.isDel,
                        createTime = p.createTime,
                        updateTime = p.updateTime,
                        storeName = sst.storeName,
                        nickName = ccu.nickName,
                        mobile = ccu.mobile,
                        avatarImage = ccu.avatarImage,
                    }).With(SqlWith.NoLock)
                .MergeTable()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await _sqlSugarClient.Queryable<CoreCmsClerk, CoreCmsStore, CoreCmsUser>((p, sst, ccu) => new JoinQueryInfos(
                        JoinType.Left, p.storeId == sst.id,
                        JoinType.Left, p.userId == ccu.id
                    ))
                    .Select((p, sst, ccu) => new StoreClerkDto
                    {
                        id = p.id,
                        storeId = p.storeId,
                        userId = p.userId,
                        isDel = p.isDel,
                        createTime = p.createTime,
                        updateTime = p.updateTime,
                        storeName = sst.storeName,
                        nickName = ccu.nickName,
                        mobile = ccu.mobile,
                        avatarImage = ccu.avatarImage,
                    })
                    .MergeTable()
                    .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                    .WhereIF(predicate != null, predicate)
                    .ToPageListAsync(pageIndex, pageSize, totalCount);
            }

            var list = new PageList<StoreClerkDto>(page, pageIndex, pageSize, totalCount);

            return list;
        }

        #endregion
    }
}
