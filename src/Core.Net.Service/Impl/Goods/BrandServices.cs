using Core.Net.Data;
using Core.Net.Entity.Model.Goods;
using Core.Net.Service.Goods;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.Goods
{
    public class BrandServices : BaseServices<Brand>, IBrandServices
    {
        private readonly IBaseRepository<Brand> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public BrandServices(IBaseRepository<Brand> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
