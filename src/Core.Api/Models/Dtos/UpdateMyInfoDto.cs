using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models.Dtos
{
    /// <summary>
    /// 修改信息
    /// </summary>
    public class UpdateMyInfoDto
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string IdCardNo { get; set; }
    }
}
