using Core.Net.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Net.Util.Helper
{
    public class EnumHelper
    {
        /// <summary>
        /// 将枚举转成List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<EnumEntity> EnumToList<T>()
        {
            List<EnumEntity> list = new List<EnumEntity>();

            foreach (var e in Enum.GetValues(typeof(T)))
            {
                EnumEntity m = new EnumEntity();
                object[] objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objArr != null && objArr.Length > 0)
                {
                    DescriptionAttribute da = objArr[0] as DescriptionAttribute;
                    m.description = da.Description;
                }
                m.value = Convert.ToInt32(e);
                m.title = e.ToString();
                list.Add(m);
            }
            return list;
        }
    }
}
