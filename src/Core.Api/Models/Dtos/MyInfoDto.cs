using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models.Dtos
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class MyInfoDto
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string idCardNo { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime createTime { get; set; }
        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string organizationName { get; set; } 
        /// <summary>
        /// 岗位角色
        /// </summary>
        public string roles { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public string balance { get; set; } 
    }
}
