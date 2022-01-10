using Core.Net.Entity.Dtos;
using Core.Net.Entity.Model.Goods;
using Core.Net.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Goods
{
    public interface ICoreCmsGoodsTypeServices : IBaseServices<CoreCmsGoodsType>
    {
        /// <summary>
        ///     更新参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> UpdateParametersAsync(FMUpdateArrayIntDataByIntId entity);

        /// <summary>
        ///     更新属性
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> UpdateTypesAsync(FMUpdateArrayIntDataByIntId entity);
        /// <summary>
        ///     事务重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> InsertAsync(FmGoodsTypeInsert entity);


        /// <summary>
        ///     重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> DeleteByIdAsync(int id);
    }
}
