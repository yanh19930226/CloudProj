using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yande.CloudTool.Api.Models.Request
{
    public class ConsumTransactionDto
    {
        /// <summary>
        /// 消费序号(年月日时分秒+2个字节序号(0~65535))，用于识别上传数据不重复
        /// </summary>
        public string Order { get; set; }
        /// <summary>
        /// 10进制卡序列号（实体卡号或虚拟卡号）
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 卡模式（0：实体卡 1：电子卡）
        /// </summary>
        public int CardMode { get; set; }
        /// <summary>
        /// 交易模式（0:刷卡扣费 1：现金充值2：余额查询 3:钱包转账）
        /// </summary>
        public int Mode { get; set; }
        /// <summary>
        /// 扣费类型（0:手动 1：菜单 2：定值 3：取餐 4：计时 5：计次）
        /// </summary>
        public int PayType { get; set; }
        /// <summary>
        /// 消费金额(Mode为2时，此字段传入值可能为0)
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// 消费菜品列表
        /// </summary>
        public List<Menu> Menus { get; set; }
    }

    public class Menu
    {
        public string MenuID { get; set; }
        public string Count { get; set; }
    }
}
