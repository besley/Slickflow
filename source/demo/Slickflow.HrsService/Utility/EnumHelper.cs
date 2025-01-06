using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.HrsService.Utility
{
    /// <summary>
    /// Convert enumeration types based on string conversion
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// Parse enum string value
        /// </summary>
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
