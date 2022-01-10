using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Entity.ViewModels
{
    /// <summary>
    ///     日志实体
    /// </summary>
    public class LogInfo
    {
        public DateTime Datetime { get; set; }
        public string Content { get; set; }
        public string IP { get; set; }
        public string LogColor { get; set; }
        public int Import { get; set; } = 0;
    }
    public class ApiWeek
    {
        public string week { get; set; }
        public string url { get; set; }
        public int count { get; set; }
    }

    public class ApiDate
    {
        public string date { get; set; }
        public int count { get; set; }
    }

    public class RequestApiWeekView
    {
        public List<string> columns { get; set; }
        public string rows { get; set; }
    }

    public class AccessApiDateView
    {
        public string[] columns { get; set; }
        public List<ApiDate> rows { get; set; }
    }

    public class RequestInfo
    {
        public string Ip { get; set; }
        public string Url { get; set; }
        public string Datetime { get; set; }
        public string Date { get; set; }
        public string Week { get; set; }
    }
}
