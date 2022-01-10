using Core.Net.Data;
using Core.Net.Entity.Model.Advert;
using Core.Net.Service.Advert;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.Advert
{
    public class CoreCmsAdvertisementServices : BaseServices<CoreCmsAdvertisement>, ICoreCmsAdvertisementServices
    {
        private readonly IBaseRepository<CoreCmsAdvertisement> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsAdvertisementServices(IBaseRepository<CoreCmsAdvertisement> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
