using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yande.CloudTool.Api.Models.Response
{
    public class ConsumTransactionsResponse : BaseResponse
    {
        /// <summary>
        /// 人员姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 10进制卡序列号（实体卡号或虚拟卡号）
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 现金余额（允许两位小数）
        /// </summary>
        public string Money { get; set; }
        /// <summary>
        /// 补贴余额（允许两位小数）
        /// </summary>
        public string Subsidy { get; set; }
        /// <summary>
        /// 剩余次数
        /// </summary>
        public string Times { get; set; }
        /// <summary>
        /// 赠送余额（允许两位小数）
        /// </summary>
        public string Integral { get; set; }
        /// <summary>
        /// 入场时间(固定值””
        /// </summary>
        public string InTime { get; set; }
        /// <summary>
        /// 出场时间(固定值””
        /// </summary>
        public string OutTime { get; set; }
        /// <summary>
        /// 累计使用时间：天时分秒
        /// </summary>
        public string CumulativeTime { get; set; }
        /// <summary>
        /// 实际扣费金额(信息查询模式下返回”0”
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        ///语音段（用于安卓系统设备）
        /// </summary>
        public string VoiceID { get; set; }
        /// <summary>
        /// 自定义显示文本（Status为1时传入，屏幕显示此值，内容内使用\r\n换行，最多支持4行，每行不超过8个汉字）
        /// </summary>
        public string Text { get; set; }
    }
}
