using Core.Net.Data;
using Core.Net.Entity.Model.Systems;
using Core.Net.Service.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.Systems
{
    public class SysDictionaryServices : BaseServices<SysDictionary>, ISysDictionaryServices
    {
        private readonly IBaseRepository<SysDictionary> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public SysDictionaryServices(IBaseRepository<SysDictionary> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
