using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models.Dtos
{
    /// <summary>
    ///  根据where查询条件和order排序获取列表
    /// </summary>
    public class FMPage
    {
        /// <summary>
        ///     当前页码
        /// </summary>
        public int page { get; set; } = 1;

        /// <summary>
        ///     每页数据量
        /// </summary>
        public int limit { get; set; } = 10;
    }
}
