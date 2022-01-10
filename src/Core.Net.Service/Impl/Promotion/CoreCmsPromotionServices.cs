using Core.Net.Data;
using Core.Net.Entity.Model.Promotion;
using Core.Net.Service.Promotion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.Promotion
{
    public class CoreCmsPromotionServices : BaseServices<CoreCmsPromotion>, ICoreCmsPromotionServices
    {
        private readonly IBaseRepository<CoreCmsPromotion> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsPromotionServices(IBaseRepository<CoreCmsPromotion> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
