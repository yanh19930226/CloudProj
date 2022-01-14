using Core.Net.Data;
using Core.Net.Entity.Model.DinnerCard;
using Core.Net.Service.DinnerCards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.DinnerCards
{
    public class CoreOrderServices : BaseServices<CoreOrder>, ICoreOrderServices
    {
        private readonly IBaseRepository<CoreOrder> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreOrderServices(IBaseRepository<CoreOrder> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }



        //public async Task<IPageList<CoreCmsNoticeViewModel>> GetNotice(Expression<Func<CoreCmsNotice, bool>> predicate, Expression<Func<CoreCmsNotice, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1, int pageSize = 20, bool blUseNoLock = false)
        //{

        //    var dict = _sysDictionaryServices.QueryByClause(p => p.dictCode == "noticeType");
        //    var dictData = new List<SysDictionaryData>();
        //    if (dict != null)
        //    {
        //        dictData = _sysDictionaryDataServices.QueryListByClause(p => p.dictId == dict.id);
        //    }

        //    RefAsync<int> totalCount = 0;
        //    var page = blUseNoLock
        //        ? await _sqlSugarClient.Queryable<CoreCmsNotice>().OrderByIF(orderByExpression != null, orderByExpression, orderByType)
        //            .WhereIF(predicate != null, predicate).Where(SqlWith.NoLock)
        //            .Select(p => new CoreCmsNoticeViewModel
        //            {
        //                id = p.id,
        //                title = p.title,
        //                sort = p.sort,
        //                contentBody = p.contentBody,
        //                createTime = p.createTime,
        //                isDel = p.isDel,
        //                noticeType = p.noticeType,
        //            }).ToPageListAsync(pageIndex, pageSize, totalCount)
        //        : await _sqlSugarClient.Queryable<CoreCmsNotice>().OrderByIF(orderByExpression != null, orderByExpression, orderByType)
        //            .WhereIF(predicate != null, predicate)
        //             .Select(p => new CoreCmsNoticeViewModel
        //             {
        //                 id = p.id,
        //                 title = p.title,
        //                 sort = p.sort,
        //                 contentBody = p.contentBody,
        //                 createTime = p.createTime,
        //                 isDel = p.isDel,
        //                 noticeType = p.noticeType,


        //             }).ToPageListAsync(pageIndex, pageSize, totalCount);


        //    var list = new PageList<CoreCmsNoticeViewModel>(page, pageIndex, pageSize, totalCount);
        //    foreach (var item in list)
        //    {

        //        item.NoticeTypeName = dictData.Where(p => p.dictDataCode == item.noticeType.ToString()).FirstOrDefault().dictDataName;
        //    }
        //    return list;
        //}
    }
}
