using Core.Net.Data;
using Core.Net.Data.Impl;
using Core.Net.Entity.Model.Shops;
using Core.Net.Entity.Model.Systems;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Shops;
using Core.Net.Service.Systems;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Impl.Shops
{
    public class CoreCmsNoticeServices : BaseServices<CoreCmsNotice>, ICoreCmsNoticeServices
    {
        private readonly IBaseRepository<CoreCmsNotice> _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlSugarClient _sqlSugarClient;
        private readonly ISysDictionaryServices _sysDictionaryServices;
        private readonly ISysDictionaryDataServices _sysDictionaryDataServices;

        public CoreCmsNoticeServices(IBaseRepository<CoreCmsNotice> dal, IUnitOfWork unitOfWork, SqlSugarClient sqlSugarClient, ISysDictionaryServices sysDictionaryServices, ISysDictionaryDataServices sysDictionaryDataServices)
        {
            _dal = dal;
            base.BaseDal = dal;
            _sqlSugarClient = sqlSugarClient;
            _unitOfWork = unitOfWork;
            _sysDictionaryServices = sysDictionaryServices;
            _sysDictionaryDataServices = sysDictionaryDataServices;
        }

        public async Task<IPageList<CoreCmsNoticeViewModel>> GetNotice(Expression<Func<CoreCmsNotice, bool>> predicate, Expression<Func<CoreCmsNotice, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1, int pageSize = 20, bool blUseNoLock = false)
        {

            var dict = _sysDictionaryServices.QueryByClause(p => p.dictCode == "noticeType");
            var dictData = new List<SysDictionaryData>();
            if (dict != null)
            {
                dictData = _sysDictionaryDataServices.QueryListByClause(p => p.dictId == dict.id);
            }

            RefAsync<int> totalCount = 0;
            var page = blUseNoLock
                ? await _sqlSugarClient.Queryable<CoreCmsNotice>().OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                    .WhereIF(predicate != null, predicate).Where(SqlWith.NoLock)
                    .Select(p => new CoreCmsNoticeViewModel
                    {
                        id = p.id,
                        title = p.title,
                        sort = p.sort,
                        contentBody = p.contentBody,
                        createTime = p.createTime,
                        isDel = p.isDel,
                        noticeType = p.noticeType,
                    }).ToPageListAsync(pageIndex, pageSize, totalCount)
                : await _sqlSugarClient.Queryable<CoreCmsNotice>().OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                    .WhereIF(predicate != null, predicate)
                     .Select(p => new CoreCmsNoticeViewModel
                     {
                         id=p.id,
                         title=p.title,
                         sort=p.sort,
                         contentBody=p.contentBody,
                         createTime=p.createTime,
                         isDel=p.isDel,
                         noticeType=p.noticeType,
                       

                     }).ToPageListAsync(pageIndex, pageSize, totalCount);


            var list = new PageList<CoreCmsNoticeViewModel>(page, pageIndex, pageSize, totalCount);
            foreach (var item in list)
            {

                item.NoticeTypeName = dictData.Where(p => p.dictDataCode == item.noticeType.ToString()).FirstOrDefault().dictDataName;
            }
            return list;
        }
    }
}
