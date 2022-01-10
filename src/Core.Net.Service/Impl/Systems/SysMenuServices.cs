using Core.Net.Data;
using Core.Net.Entity.Model.Systems;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Systems;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Impl.Systems
{
    public class SysMenuServices : BaseServices<SysMenu>, ISysMenuServices
    {
        private readonly IBaseRepository<SysMenu> _dal;
        private readonly IUnitOfWork _unitOfWork;
        public SysMenuServices(IBaseRepository<SysMenu> dal, IUnitOfWork unitOfWork)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
