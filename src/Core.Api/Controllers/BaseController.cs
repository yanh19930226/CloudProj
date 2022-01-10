using Core.Net.Util.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class BaseController : Controller
    {
        public object SysUserId;
        public object Role;
        public object IdentityId;
        public object OrganizationId;
        public object Name;
        public object NameIdentifier;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string jwtStr = HttpContext.Request.Headers["Authorization"].ObjectToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(jwtStr) == false)
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);

                jwtToken.Payload.TryGetValue("SysUserId", out SysUserId);
                jwtToken.Payload.TryGetValue("OrganizationId", out OrganizationId);
                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out Role);
                jwtToken.Payload.TryGetValue(ClaimTypes.Name, out Name);
                jwtToken.Payload.TryGetValue(ClaimTypes.NameIdentifier, out NameIdentifier);
                jwtToken.Payload.TryGetValue("IdentityId", out IdentityId);

                SysUserId = SysUserId.ToString();
                Role = Role.ToString();
                Name = Name.ToString();
                IdentityId = IdentityId.ToString();
                OrganizationId = OrganizationId.ToString();
                NameIdentifier = NameIdentifier.ToString();
            }
        }
    }
}
