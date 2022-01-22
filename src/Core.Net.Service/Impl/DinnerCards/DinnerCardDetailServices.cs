using Core.Net.Data;
using Core.Net.Entity.Model.DinnerCard;
using Core.Net.Service.DinnerCards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.DinnerCards
{

    public class DinnerCardDetailServices : BaseServices<DinnerCardDetail>, IDinnerCardDetailServices
    {
        private readonly IBaseRepository<DinnerCardDetail> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public DinnerCardDetailServices(IBaseRepository<DinnerCardDetail> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
