using Core.Net.Data;
using Core.Net.Entity.Model.Article;
using Core.Net.Service.Article;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.Article
{
    public class CoreCmsArticleServices : BaseServices<CoreCmsArticle>, ICoreCmsArticleServices
    {
        private readonly IBaseRepository<CoreCmsArticle> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsArticleServices(IBaseRepository<CoreCmsArticle> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
