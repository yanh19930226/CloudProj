using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.Dtos
{
    public class FMIntId
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [Required(ErrorMessage = "请输入要提交的序列参数")]
        public int id { get; set; }

        public object data { get; set; } = null;
    }
    public class FMIntIdByListIntData
    {
        public int id { get; set; }
        public List<int> data { get; set; } = null;
    }
    public class FMArrayIntIds
    {
        public int[] id { get; set; }
        public object data { get; set; } = null;
    }


    public class FMArrayStrIds
    {
        public string[] orderNo { get; set; }
        public object data { get; set; } = null;
    }

}
