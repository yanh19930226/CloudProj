using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Entity.ViewModels
{
    /// <summary>
    ///     查询货运公司列表返回实体数据
    /// </summary>
    public class ShowApiGetExpressCompanyListResult
    {
        /// <summary>
        ///     错误说明
        /// </summary>
        public string showapi_res_error { get; set; }

        /// <summary>
        ///     状态码
        /// </summary>
        public int showapi_res_code { get; set; }

        /// <summary>
        ///     返回资源序列
        /// </summary>
        public string showapi_res_id { get; set; }

        /// <summary>
        ///     返回资源主体
        /// </summary>
        public ResultBody showapi_res_body { get; set; }
    }

    public class ResultBody
    {
        public int ret_code { get; set; }
        //public bool flag { get; set; }
        //public int page { get; set; }
        //public int showapi_fee_codepage { get; set; }
        //public int allNum { get; set; }
        //public string msg { get; set; }
        //public int maxSize { get; set; }

        public List<expressCompanyList> expressList { get; set; }
    }

    public class expressCompanyList
    {
        public string img_url { get; set; }
        public string com { get; set; }
        public string phone { get; set; }
        public string exp_name { get; set; }
        public string note { get; set; }
        public string url { get; set; }
    }
}
