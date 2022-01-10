using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yande.CloudTool.Api.Models.Request
{
    /// <summary>
    /// TransactionDto
    /// </summary>
    public class TransactionDto
    {
        /// <summary>
        /// 从最后一笔消费往前算的位置 1就是最后一笔 2就是倒数第二笔
        /// </summary>
        public int Number { get; set; }
    }
}
