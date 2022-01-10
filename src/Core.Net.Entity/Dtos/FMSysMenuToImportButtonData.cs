using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Entity.Dtos
{
    public class FMSysMenuToImportButtonData
    {
        public int menuId { get; set; }
        public string controllerName { get; set; }
        public string actionName { get; set; }
        public string description { get; set; }
    }


    public class FMSysMenuToImportButton
    {
        public List<FMSysMenuToImportButtonData> data { get; set; }
    }
}
