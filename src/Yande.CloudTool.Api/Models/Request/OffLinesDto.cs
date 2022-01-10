using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yande.CloudTool.Api.Models.Request
{
    public class OffLinesDto
    {
        public int DeviceNumber { get; set; }
        public string Order { get; set; }
        public int PayType { get; set; }
        public int CardMode { get; set; }
        public string Time { get; set; }
        public string CardNo { get; set; }
        public string Money { get; set; }
    }
}
