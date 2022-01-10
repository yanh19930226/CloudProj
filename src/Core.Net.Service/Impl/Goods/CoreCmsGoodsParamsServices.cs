using Core.Net.Data;
using Core.Net.Entity.Model.Goods;
using Core.Net.Service.Goods;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.Goods
{
    public class CoreCmsGoodsParamsServices : BaseServices<CoreCmsGoodsParams>, ICoreCmsGoodsParamsServices
    {
        private readonly IBaseRepository<CoreCmsGoodsParams> _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlSugarClient _sqlSugarClient;

        public CoreCmsGoodsParamsServices(IBaseRepository<CoreCmsGoodsParams> dal, IUnitOfWork unitOfWork, SqlSugarClient sqlSugarClient)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _sqlSugarClient = sqlSugarClient;
        }
    }
}
