using Core.Net.Data;
using Core.Net.Entity.Model.Systems;
using Core.Net.Service.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.Systems
{
    public class SysNLogRecordsServices : BaseServices<SysNLogRecords>, ISysNLogRecordsServices
    {
        private readonly IBaseRepository<SysNLogRecords> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public SysNLogRecordsServices(IBaseRepository<SysNLogRecords> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
