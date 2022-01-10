using Core.Net.Data;
using Core.Net.Entity.Model.Systems;
using Core.Net.Service.Systems;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Impl.Systems
{
    public class SysRoleMenuServices : BaseServices<SysRoleMenu>, ISysRoleMenuServices
    {
        private readonly IBaseRepository<SysRoleMenu> _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlSugarClient _sqlSugarClient;
        public SysRoleMenuServices(IBaseRepository<SysRoleMenu> dal, IUnitOfWork unitOfWork, SqlSugarClient sqlSugarClient)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _sqlSugarClient = sqlSugarClient;
        }

        public async Task<List<SysRoleMenu>> RoleModuleMaps()
        {
            return await _dal.QueryMuchAsync<SysRoleMenu, SysMenu, SysRole, SysRoleMenu>(
               (rmp, m, r) => new object[]
               {
                    JoinType.Left, rmp.menuId == m.id,
                    JoinType.Left, rmp.roleId == r.id
               },
               (rmp, m, r) => new SysRoleMenu
               {
                   role = r,
                   menu = m
               },
               (rmp, m, r) => m.deleted == false && r.deleted == false
           );
        }
    }
}
