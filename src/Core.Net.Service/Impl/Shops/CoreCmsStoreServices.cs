using Core.Net.Data;
using Core.Net.Entity.Model.Shops;
using Core.Net.Service.Shops;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.Shops
{
    public class CoreCmsStoreServices : BaseServices<CoreCmsStore>, ICoreCmsStoreServices
    {
        private readonly IBaseRepository<CoreCmsStore> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsStoreServices(IBaseRepository<CoreCmsStore> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
