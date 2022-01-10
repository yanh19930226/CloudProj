using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Entity.Dtos
{
    /// <summary>
    ///     用户登录验证实体
    /// </summary>
    public class FMLogin
    {
        public string userName { get; set; }
        public string password { get; set; }
    }
}
