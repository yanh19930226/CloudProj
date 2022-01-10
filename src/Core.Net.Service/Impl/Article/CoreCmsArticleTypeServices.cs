using Core.Net.Data;
using Core.Net.Entity.Model.Article;
using Core.Net.Service.Article;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.Article
{
    public class CoreCmsArticleTypeServices : BaseServices<CoreCmsArticleType>, ICoreCmsArticleTypeServices
    {
        private readonly IBaseRepository<CoreCmsArticleType> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsArticleTypeServices(IBaseRepository<CoreCmsArticleType> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
