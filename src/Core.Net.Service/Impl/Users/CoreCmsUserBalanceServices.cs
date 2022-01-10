using Core.Net.Data;
using Core.Net.Entity.Model.Users;
using Core.Net.Service.Users;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.Users
{
    public class CoreCmsUserBalanceServices : BaseServices<CoreCmsUserBalance>, ICoreCmsUserBalanceServices
    {
        private readonly IBaseRepository<CoreCmsUserBalance> _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlSugarClient _sqlSugarClient;
        public CoreCmsUserBalanceServices(IBaseRepository<CoreCmsUserBalance> dal, IUnitOfWork unitOfWork, SqlSugarClient sqlSugarClient)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _sqlSugarClient = sqlSugarClient;
        }
    }
}
