using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.HrsService.Utility
{
    /// <summary>
    /// 根据字符串转换枚举类型
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// 枚举类型解析
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">值</param>
        /// <returns>枚举</returns>
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
