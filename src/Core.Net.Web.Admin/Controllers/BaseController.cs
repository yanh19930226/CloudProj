using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Core.Net.Configuration.GlobalConstVars;

namespace Core.Net.Web.Admin.Controllers
{
    [Authorize(policy: Permissions.Name)]
    public class BaseController : Controller
    {
    }
}
