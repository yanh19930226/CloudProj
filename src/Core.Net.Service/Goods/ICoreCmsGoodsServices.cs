using Core.Net.Entity.Dtos;
using Core.Net.Entity.Model.Goods;
using Core.Net.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Goods
{
    public interface ICoreCmsGoodsServices : IBaseServices<CoreCmsGoods>
    {

        /// <summary>
        ///     设置标签
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> DoSetLabel(FmSetLabel entity);

        /// <summary>
        ///     取消标签
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> DoDeleteLabel(FmSetLabel entity);

        /// <summary>
        ///     批量上架
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> DoBatchMarketableUp(int[] ids);

        /// <summary>
        ///     批量下架
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> DoBatchMarketableDown(int[] ids);
    }
}
