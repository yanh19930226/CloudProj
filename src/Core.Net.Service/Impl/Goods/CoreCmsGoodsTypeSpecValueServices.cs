using Core.Net.Data;
using Core.Net.Entity.Model.Goods;
using Core.Net.Service.Goods;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.Goods
{
    public class CoreCmsGoodsTypeSpecValueServices :BaseServices<CoreCmsGoodsTypeSpecValue>, ICoreCmsGoodsTypeSpecValueServices
    {
        private readonly IBaseRepository<CoreCmsGoodsTypeSpecValue> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsGoodsTypeSpecValueServices(IBaseRepository<CoreCmsGoodsTypeSpecValue> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
