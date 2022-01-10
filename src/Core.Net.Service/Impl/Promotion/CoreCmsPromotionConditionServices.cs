using Core.Net.Data;
using Core.Net.Entity.Model.Promotion;
using Core.Net.Service.Promotion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.Promotion
{
    public class CoreCmsPromotionConditionServices : BaseServices<CoreCmsPromotionCondition>, ICoreCmsPromotionConditionServices
    {
        private readonly IBaseRepository<CoreCmsPromotionCondition> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsPromotionConditionServices(IBaseRepository<CoreCmsPromotionCondition> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
