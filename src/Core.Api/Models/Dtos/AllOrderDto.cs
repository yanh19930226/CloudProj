using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models.Dtos
{
    /// <summary>
    /// 订单记录查询条件
    /// </summary>
    public class AllOrderDto
    {
        /// <summary>
        /// 机构部门Id
        /// </summary>
        public string organizationId { get; set; }
        /// <summary>
        /// 岗位角色Id
       /// </summary>
       public string roleId { get; set; }
        /// <summary>
        /// 员工姓名/身份证/手机
        /// </summary>
        public string userText { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string orderNo { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public string orderType { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string startTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string endTime { get; set; }
    }
}
