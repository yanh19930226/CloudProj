using Core.Net.Data;
using Core.Net.Entity.Model.Shops;
using Core.Net.Service.Shops;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.Shops
{
    public class CoreCmsShipServices : BaseServices<CoreCmsShip>, ICoreCmsShipServices
    {
        private readonly IBaseRepository<CoreCmsShip> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsShipServices(IBaseRepository<CoreCmsShip> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
