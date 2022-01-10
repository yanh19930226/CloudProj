using Core.Net.Data;
using Core.Net.Entity.Model.Users;
using Core.Net.Service.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.Users
{
    public class CoreCmsUserGradeServices : BaseServices<CoreCmsUserGrade>, ICoreCmsUserGradeServices
    {
        private readonly IBaseRepository<CoreCmsUserGrade> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsUserGradeServices(IBaseRepository<CoreCmsUserGrade> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
