using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models.Configs
{
    /// <summary>
    /// JwtConfig
    /// </summary>
    public class JwtConfig
    {
        /// <summary>
        /// SecretKey
        /// </summary>
        public string SecretKey { get; set; }
        /// <summary>
        /// Issuer
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// Audience
        /// </summary>
        public string Audience { get; set; }
    }
}
