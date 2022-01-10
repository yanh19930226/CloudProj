using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Net.Entity.ViewModels
{
    /// <summary>
    ///     店员视图表
    /// </summary>
    public class StoreClerkDto
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "")]
        public int id { get; set; }

        /// <summary>
        /// </summary>
        [Display(Name = "")]
        public int storeId { get; set; }

        /// <summary>
        /// </summary>
        [Display(Name = "")]
        public int userId { get; set; }

        /// <summary>
        /// </summary>
        [Display(Name = "")]
        public bool isDel { get; set; }

        /// <summary>
        /// </summary>
        [Display(Name = "")]
        public DateTime createTime { get; set; }

        /// <summary>
        /// </summary>
        [Display(Name = "")]
        public DateTime? updateTime { get; set; }

        /// <summary>
        /// </summary>
        [Display(Name = "")]
        public string storeName { get; set; }

        /// <summary>
        /// </summary>
        [Display(Name = "")]
        public string nickName { get; set; }

        /// <summary>
        /// </summary>
        [Display(Name = "")]
        public string mobile { get; set; }

        /// <summary>
        /// </summary>
        [Display(Name = "")]
        public string avatarImage { get; set; }
    }
}
