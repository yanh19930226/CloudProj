using Core.Api.Models;
using Core.Api.Models.Configs;
using Core.Net.Entity.Dtos;
using Core.Net.Entity.ViewModels;
using Core.Net.Service.Systems;
using Core.Net.Util.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.Api.Controllers
{
    /// <summary>
    /// 登入控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : Controller
    {

        private readonly ISysUserServices _sysUserServices;
        private IOptions<JwtConfig> _options;

        /// <summary>
        /// 登入接口
        /// </summary>
        /// <param name="sysUserServices"></param>
        public LoginController(ISysUserServices sysUserServices, IOptions<JwtConfig> options)
        {
            _sysUserServices = sysUserServices;
            _options = options;
        }

        /// <summary>
        /// 获取JWT的授权
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> GetJwtToken([FromBody] FMLogin model)
        {
            var jm = new CallBackResult<TokenModel>();

            if (string.IsNullOrEmpty(model.userName))
            {
                jm.Failed("用户名不能为空");
                return Ok(jm);
            }

            if (string.IsNullOrEmpty(model.password))
            {
                jm.Failed("密码不能为空");
                return Ok(jm);
            }

            var user = await _sysUserServices.QueryByClauseAsync(p => p.userName == model.userName || p.phone == model.userName && p.passWord == CommonHelper.Md5For32(model.password));

            if (user == null)
            {
                jm.Failed("用户名或者密码不正确");
                return Ok(jm);
            }
            var claims = new Claim[]
               {
                   new Claim("SysUserId", user.id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, user.id.ToString()),
                     new Claim("OrganizationId",user.organizationId==null?"":user.organizationId.ToString()),
                    new Claim("IdentityId", user.userName==null?"":user.userName),
                    new Claim(ClaimTypes.Role,"测试"),
                    new Claim(ClaimTypes.Name,user.userName==null?"":user.userName),
                    new Claim(ClaimTypes.NameIdentifier,user.userName==null?"":user.userName),
                     new Claim("LastLoginTime", DateTime.Now.ToString()),
               };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_options.Value.SecretKey));
            var expires = DateTime.Now.AddDays(3);
            var token = new JwtSecurityToken(
                        issuer: _options.Value.Issuer,
                        audience: _options.Value.Audience,
                        claims: claims,
                        notBefore: DateTime.Now,
                        expires: expires,
                        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);  //生成Token

            var data = new TokenModel() { Token = jwtToken, Expires = expires.ToFileTimeUtc() };

            jm.Success(data, "登入成功");

            return Ok(jm);
        }

        /// <summary>
        /// 请求刷新Token（以旧换新）
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> RefreshToken()
        {
            var jm = new CallBackResult<string>();

            return Json(jm);
        }
    }
}
