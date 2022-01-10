using Core.Net.Entity.Model.Systems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.Service.Systems
{
    public interface ISysUserServices : IBaseServices<SysUser>
    {
        Task<string> GetUserRoleNameStr(string loginName, string loginPwd);
    }
}
