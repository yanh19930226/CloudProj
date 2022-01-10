using Core.Net.Data;
using Core.Net.Entity.Model.DinnerCard;
using Core.Net.Service.DinnerCards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.DinnerCards
{
    public class CoreOrderServices : BaseServices<CoreOrder>, ICoreOrderServices
    {
        private readonly IBaseRepository<CoreOrder> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreOrderServices(IBaseRepository<CoreOrder> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
