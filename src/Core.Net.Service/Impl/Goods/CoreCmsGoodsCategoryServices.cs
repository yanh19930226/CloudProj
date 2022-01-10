using Core.Net.Configuration;
using Core.Net.Data;
using Core.Net.Entity.Model.Goods;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Goods;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Impl.Goods
{
    public class CoreCmsGoodsCategoryServices: BaseServices<CoreCmsGoodsCategory>, ICoreCmsGoodsCategoryServices
    {
        private readonly IBaseRepository<CoreCmsGoodsCategory> _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlSugarClient _sqlSugarClient;

        public CoreCmsGoodsCategoryServices(IBaseRepository<CoreCmsGoodsCategory> dal, IUnitOfWork unitOfWork, SqlSugarClient sqlSugarClient)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _sqlSugarClient = sqlSugarClient;
        }

        public async Task<AdminUiCallBack> DeletedByIdAsync(object id)
        {
            var jm = new AdminUiCallBack();

            var bl = await _sqlSugarClient.Deleteable<CoreCmsGoodsCategory>(id).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            //if (bl)
            //{
            //    await UpdateCaChe();
            //}

            return jm;
        }

        public async Task<AdminUiCallBack> InsertAddAsync(CoreCmsGoodsCategory entity)
        {
            var jm = new AdminUiCallBack();
            var bl = await _sqlSugarClient.Insertable(entity).ExecuteReturnIdentityAsync() > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;
            //if (bl)
            //{
            //    await UpdateCaChe();
            //}
            return jm;
        }

        public async Task<AdminUiCallBack> UpdateEditAsync(CoreCmsGoodsCategory entity)
        {
            var jm = new AdminUiCallBack();

            //事物处理过程结束
            var bl = await _sqlSugarClient.Updateable(entity).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            //if (bl)
            //{
            //    await UpdateCaChe();
            //}

            return jm;
        }
    }
}
