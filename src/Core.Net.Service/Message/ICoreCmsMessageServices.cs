using Core.Net.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Message
{
    public interface ICoreCmsMessageServices
    {
        Task<CoreCmsMessage> QueryByIdAsync(object objId);
    }
}
