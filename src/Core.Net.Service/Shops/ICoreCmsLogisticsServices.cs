using Core.Net.Entity.Model.Shops;
using Core.Net.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Shops
{
    public interface ICoreCmsLogisticsServices : IBaseServices<CoreCmsLogistics>
    {
        /// <summary>
        ///     通过接口
        /// </summary>
        Task<AdminUiCallBack> DoUpdateCompany();
    }
}
