using Core.Net.Data;
using Core.Net.Entity.Model.Systems;
using Core.Net.Service.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.Systems
{
    public class SysOrganizationServices : BaseServices<SysOrganization>, ISysOrganizationServices
    {
        private readonly IBaseRepository<SysOrganization> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public SysOrganizationServices(IBaseRepository<SysOrganization> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
