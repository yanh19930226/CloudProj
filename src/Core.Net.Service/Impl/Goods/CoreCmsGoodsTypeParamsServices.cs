using Core.Net.Data;
using Core.Net.Entity.Model.Goods;
using Core.Net.Service.Goods;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.Goods
{
    public class CoreCmsGoodsTypeParamsServices : BaseServices<CoreCmsGoodsTypeParams>, ICoreCmsGoodsTypeParamsServices
    {
        private readonly IBaseRepository<CoreCmsGoodsTypeParams> _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlSugarClient _sqlSugarClient;

        public CoreCmsGoodsTypeParamsServices(IBaseRepository<CoreCmsGoodsTypeParams> dal, IUnitOfWork unitOfWork, SqlSugarClient sqlSugarClient)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _sqlSugarClient = sqlSugarClient;
        }
    }
}
