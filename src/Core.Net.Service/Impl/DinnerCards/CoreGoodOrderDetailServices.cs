using Core.Net.Data;
using Core.Net.Entity.Model.DinnerCard;
using Core.Net.Service.DinnerCards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.DinnerCards
{
    public class CoreGoodOrderDetailServices : BaseServices<CoreGoodOrderDetail>, ICoreGoodOrderDetailServices
    {
        private readonly IBaseRepository<CoreGoodOrderDetail> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreGoodOrderDetailServices(IBaseRepository<CoreGoodOrderDetail> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
