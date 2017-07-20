using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;

namespace Slickflow.WebDemo.Common
{
    /// <summary>
    /// 枚举的帮助类
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 根据枚举定义的项返回Description
        /// </summary>
        /// <param name="value">枚举定义的项</param>
        /// <returns></returns>
        public static string GetDescription(this System.Enum value)
        {
            string rtDescription = string.Empty;
            // Stopwatch watch = new Stopwatch();
            // watch.Start();
            var fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo != null)
            {
                var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes != null)
                {
                    rtDescription = attributes.Length > 0 ? attributes[0].Description : string.Empty;
                }
            }
            // watch.Stop();
            return rtDescription;
        }

        /// <summary>
        /// 根据枚举、项的value值返回枚举的Description
        /// </summary>
        /// <param name="enumType">枚举</param>
        /// <param name="enumIntValue">项的value值</param>
        /// <returns></returns>
        public static string GetDescription(Type enumType, int enumIntValue)
        {
            //string nameDesc = System.Enum.GetName(enumType, enumIntValue);
            return GetDescription((System.Enum)System.Enum.ToObject(enumType, enumIntValue));
        }


    }
}