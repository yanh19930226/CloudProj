using Core.Net.Data;
using Core.Net.Entity.Model.Users;
using Core.Net.Service.Users;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.Users
{
    public class CoreCmsUserPointLogServices : BaseServices<CoreCmsUserPointLog>, ICoreCmsUserPointLogServices
    {
        private readonly IBaseRepository<CoreCmsUserPointLog> _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlSugarClient _sqlSugarClient;
        public CoreCmsUserPointLogServices(IBaseRepository<CoreCmsUserPointLog> dal, IUnitOfWork unitOfWork, SqlSugarClient sqlSugarClient)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _sqlSugarClient = sqlSugarClient;
        }
    }
}
