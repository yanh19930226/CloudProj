using Core.Net.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Net.Util.Helper
{
    public static class CommonHelper
    {

        #region 获取现在是星期几
        /// <summary>
        /// 获取现在是星期几
        /// </summary>
        /// <returns></returns>
        public static string GetWeek()
        {
            string week = string.Empty;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    week = "周一";
                    break;
                case DayOfWeek.Tuesday:
                    week = "周二";
                    break;
                case DayOfWeek.Wednesday:
                    week = "周三";
                    break;
                case DayOfWeek.Thursday:
                    week = "周四";
                    break;
                case DayOfWeek.Friday:
                    week = "周五";
                    break;
                case DayOfWeek.Saturday:
                    week = "周六";
                    break;
                case DayOfWeek.Sunday:
                    week = "周日";
                    break;
                default:
                    week = "N/A";
                    break;
            }
            return week;
        }

        #endregion

        #region 获取32位md5加密
        /// <summary>
        /// 通过创建哈希字符串适用于任何 MD5 哈希函数 （在任何平台） 上创建 32 个字符的十六进制格式哈希字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns>32位md5加密字符串</returns>
        public static string Md5For32(string source)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                string hash = sBuilder.ToString();
                return hash.ToUpper();
            }
        }
        #endregion

        #region String数组转Int数组
        public static int[] StringArrAyToIntArray(string[] str)
        {
            try
            {
                int[] iNums = Array.ConvertAll<string, int>(str, s => int.Parse(s));
                return iNums;
            }
            catch
            {
                return new int[0];
            }
        }
        #endregion

        #region string 转int数组

        public static int[] StringToIntArray(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return new int[0];
                if (str.EndsWith(","))
                {
                    str = str.Remove(str.Length - 1, 1);
                }
                var idstrarr = str.Split(',');
                var idintarr = new int[idstrarr.Length];

                for (int i = 0; i < idstrarr.Length; i++)
                {
                    idintarr[i] = Convert.ToInt32(idstrarr[i]);
                }
                return idintarr;
            }
            catch
            {
                return new int[0];
            }
        }
        #endregion

        #region 判断字符串是否为手机号码
        /// <summary>
        /// 判断字符串是否为手机号码
        /// </summary>
        /// <param name="mobilePhoneNumber"></param>
        /// <returns></returns>
        public static bool IsMobile(string mobilePhoneNumber)
        {
            if (mobilePhoneNumber.Length < 11)
            {
                return false;
            }

            //电信手机号码正则
            string dianxin = @"^1[345789][01379]\d{8}$";
            Regex regexDx = new Regex(dianxin);
            //联通手机号码正则
            string liantong = @"^1[345678][01256]\d{8}$";
            Regex regexLt = new Regex(liantong);
            //移动手机号码正则
            string yidong = @"^1[345789][0123456789]\d{8}$";
            Regex regexYd = new Regex(yidong);
            if (regexDx.IsMatch(mobilePhoneNumber) || regexLt.IsMatch(mobilePhoneNumber) || regexYd.IsMatch(mobilePhoneNumber))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 从字典中取单个数据
        /// <summary>
        /// 从字典中取单个数据
        /// </summary>
        /// <param name="configs"></param>
        /// <param name="skey"></param>
        /// <returns></returns>
        public static string GetConfigDictionary(Dictionary<string, DictionaryKeyValues> configs, string skey)
        {
            configs.TryGetValue(skey, out var di);
            return di?.sValue;
        }

        #endregion
    }


    public static class RandomNumber
    {
        public static object _lock = new object();
        public static int count = 1;

        public static string GetRandomProduct()
        {
            lock (_lock)
            {
                if (count >= 10000)
                {
                    count = 1;
                }
                var number = "P" + DateTime.Now.ToString("yyMMddHHmmss") + count.ToString("0000");
                count++;
                return number;
            }
        }


        public static string GetRandomOrder()
        {
            lock (_lock)
            {
                return "O" + DateTime.Now.Ticks;

            }
        }

        public static string GetRandomBusiness()
        {
            lock (_lock)
            {
                Random ran = new Random();
                return "B" + DateTime.Now.ToString("yyyyMMddHHmmss") + ran.Next(1000, 9999).ToString();
            }
        }


        public static string GetRandomSort()
        {
            lock (_lock)
            {
                Random ran = new Random();
                return "S" + DateTime.Now.ToString("yyyyMMddHHmmss") + ran.Next(1000, 9999).ToString();
            }
        }
    }
}
