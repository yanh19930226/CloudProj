using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Entity.ViewModels
{
    /// <summary>
    ///     枚举实体
    /// </summary>
    public class EnumEntity
    {
        /// <summary>
        ///     枚举的描述
        /// </summary>
        public string description { set; get; }

        /// <summary>
        ///     枚举名称
        /// </summary>
        public string title { set; get; }

        /// <summary>
        ///     枚举对象的值
        /// </summary>
        public int value { set; get; }
    }
}
