using Core.Net.Entity.Dtos;
using Core.Net.Entity.Model.Goods;
using Core.Net.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Goods
{
    public interface ICoreCmsGoodsTypeSpecServices : IBaseServices<CoreCmsGoodsTypeSpec>
    {

        /// <summary>
        ///     重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> InsertAsync(FmGoodsTypeSpecInsert entity);

        /// <summary>
        ///     重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> UpdateAsync(FmGoodsTypeSpecUpdate entity);

        /// <summary>
        ///     重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> DeleteByIdAsync(object id);
    }
}
