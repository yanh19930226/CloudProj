using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yande.CloudTool.Api.Models.Response
{
    /// <summary>
    /// TransactionResponse
    /// </summary>
    public class TransactionResponse:BaseResponse
    {
        /// <summary>
        /// 编号 从最后一笔消费往前算的位置 1就是最后一笔 2就是倒数第二笔
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 消费时间(格式：yyyyMMddHHmmss)
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 卡号10进制（实体卡号或虚拟卡号）
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 消费额 10.00元
        /// </summary>
        public string Consume { get; set; }
        /// <summary>
        /// 余额100.00元
        /// </summary>
        public string Balance { get; set; }
        /// <summary>
        /// 消费类型 （消费 充值 退款,因错退还）
        /// </summary>
        public string Class { get; set; }
    }
}
