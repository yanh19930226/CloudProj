using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yande.CloudTool.Api.Models.Response
{
    /// <summary>
    /// MenusResponse
    /// </summary>
    public class MenusResponse:BaseResponse
    {
        public string Name { get; set; }
        public string Price { get; set; }

    }
}
