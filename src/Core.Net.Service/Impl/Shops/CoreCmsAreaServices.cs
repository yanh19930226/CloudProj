using Core.Net.Data;
using Core.Net.Entity.Model.Shops;
using Core.Net.Service.Shops;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.Shops
{
    public class CoreCmsAreaServices : BaseServices<CoreCmsArea>, ICoreCmsAreaServices
    {
        private readonly IBaseRepository<CoreCmsArea> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsAreaServices(IBaseRepository<CoreCmsArea> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
