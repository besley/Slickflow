using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Utility
{
    /// <summary>
    /// Convert enumeration types based on string conversion
    /// 根据字符串转换枚举类型
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// Parse Enum
        /// 枚举类型解析
        /// </summary>
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Parse Enum
        /// 枚举类型解析
        /// </summary>
        public static T TryParseEnum<T>(string value) where T : struct
        {
            T t = default(T);
            bool ok = Enum.TryParse<T>(value, out t);
            return t;
        }

        /// <summary>
        /// Get the first element of a non empty list
        /// 获取非空列表的首个元素
        /// </summary>
        public static T GetFirst<T>(List<T> list)
        {
            if (list != null && list.Count() > 0)
            {
                return list[0];
            }
            else
            {
                return default(T);
            }
        }
    }
}
