using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Entity.Dtos
{
    /// <summary>
    ///     提交设置标签实体
    /// </summary>
    public class FmSetLabel
    {
        /// <summary>
        ///     序列数组
        /// </summary>
        public int[] ids { get; set; }

        public List<labels> labels { get; set; }
    }
    public class labels
    {
        public string text { get; set; }
        public string style { get; set; }
    }
}
