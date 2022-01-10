using Core.Net.Entity.Model.Goods;
using Core.Net.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Goods
{
    public interface ICoreCmsGoodsCategoryServices : IBaseServices<CoreCmsGoodsCategory>
    {
        /// <summary>
        ///     事务重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
         new Task<AdminUiCallBack> InsertAddAsync(CoreCmsGoodsCategory entity);

        /// <summary>
        ///     重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> UpdateEditAsync(CoreCmsGoodsCategory entity);

        /// <summary>
        ///     重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> DeletedByIdAsync(object id);

    }
}
