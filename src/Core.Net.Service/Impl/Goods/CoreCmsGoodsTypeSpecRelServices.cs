using Core.Net.Data;
using Core.Net.Entity.Model.Goods;
using Core.Net.Service.Goods;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Service.Impl.Goods
{
    public class CoreCmsGoodsTypeSpecRelServices : BaseServices<CoreCmsGoodsTypeSpecRel>, ICoreCmsGoodsTypeSpecRelServices
    {
        private readonly IBaseRepository<CoreCmsGoodsTypeSpecRel> _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlSugarClient _sqlSugarClient;

        public CoreCmsGoodsTypeSpecRelServices(IBaseRepository<CoreCmsGoodsTypeSpecRel> dal, IUnitOfWork unitOfWork, SqlSugarClient sqlSugarClient)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _sqlSugarClient = sqlSugarClient;
        }
    }
}
