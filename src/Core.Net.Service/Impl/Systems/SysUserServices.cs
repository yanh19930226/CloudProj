using Core.Net.Data;
using Core.Net.Entity.Model.Systems;
using Core.Net.Service.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Impl.Systems
{
    public class SysUserServices : BaseServices<SysUser>, ISysUserServices
    {
        private readonly IBaseRepository<SysUser> _dal;
        private readonly IBaseRepository<SysRole> _sysRoleRepository;
        private readonly IBaseRepository<SysUserRole> _sysUserRoleRepository;
        private readonly IUnitOfWork _unitOfWork;
        public SysUserServices(IBaseRepository<SysUser> dal, IUnitOfWork unitOfWork, IBaseRepository<SysRole> sysRoleRepository, IBaseRepository<SysUserRole> sysUserRoleRepository)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _sysRoleRepository = sysRoleRepository;
            _sysUserRoleRepository = sysUserRoleRepository;
        }

        /// <summary>
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        public async Task<string> GetUserRoleNameStr(string loginName, string loginPwd)
        {
            var roleName = "";
            //用户是否存在
            var user = await QueryByClauseAsync(a => a.userName == loginName && a.passWord == loginPwd);
            //获取所有角色
            var roleList = await _sysRoleRepository.QueryListByClauseAsync(a => a.deleted == false);
            if (user != null)
            {
                //用户存在获取用户所有角色
                var userRoles = await _sysUserRoleRepository.QueryListByClauseAsync(ur => ur.userId == user.id);
                if (userRoles.Count > 0)
                {
                    //获取所有用户角色的角色名生成Claim
                    var arr = userRoles.Select(ur => ur.roleId).ToList();
                    var roles = roleList.Where(d => arr.Contains(d.id));
                    roleName = string.Join(',', roles.Select(r => r.roleCode).ToArray());
                }
            }
            return roleName;
        }
    }
}
