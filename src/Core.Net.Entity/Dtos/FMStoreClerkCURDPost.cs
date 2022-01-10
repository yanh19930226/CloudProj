using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Entity.Dtos
{
    public class FMStoreClerkCURDPost
    {
        public int id { get; set; } = 0;

        public int storeId { get; set; }
        public string phone { get; set; }
    }
}
