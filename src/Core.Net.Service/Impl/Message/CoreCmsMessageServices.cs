using Core.Net.Data;
using Core.Net.Entity.Model;
using Core.Net.Service.Message;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Impl.Message
{
    public class CoreCmsMessageServices : ICoreCmsMessageServices
    {
        private readonly IBaseRepository<CoreCmsMessage> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsMessageServices(IBaseRepository<CoreCmsMessage> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            _unitOfWork = unitOfWork;
        }

        public async Task<CoreCmsMessage> QueryByIdAsync(object objId)
        {
            return await _dal.QueryByIdAsync(objId);
        }
    }
}
