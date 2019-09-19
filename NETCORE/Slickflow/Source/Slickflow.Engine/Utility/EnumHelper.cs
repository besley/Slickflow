using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Utility
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

        /// <summary>
        /// 枚举类型解析
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">值</param>
        /// <returns>枚举</returns>
        public static T TryParseEnum<T>(string value) where T : struct
        {
            T t = default(T);
            bool ok = Enum.TryParse<T>(value, out t);
            return t;
        }

        /// <summary>
        /// 获取非空列表的首个元素
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">列表</param>
        /// <returns>首元素</returns>
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
