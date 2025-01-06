using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.BizAppService.Utility
{
    /// <summary>
    /// Convert enumeration types based on string conversion
    /// 根据字符串转换枚举类型
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// Parse enum data 
        /// 枚举类型解析
        /// </summary>
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
