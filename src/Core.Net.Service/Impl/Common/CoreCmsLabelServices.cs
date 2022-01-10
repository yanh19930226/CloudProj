using Core.Net.Data;
using Core.Net.Entity.Model.Common;
using Core.Net.Service.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.Common
{
    public class CoreCmsLabelServices : BaseServices<CoreCmsLabel>, ICoreCmsLabelServices
    {
        private readonly IBaseRepository<CoreCmsLabel> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsLabelServices(IBaseRepository<CoreCmsLabel> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
