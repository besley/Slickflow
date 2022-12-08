using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 动态语言运行类交互异常类
    /// </summary>
    public class DLRuntimeInteroperationException : System.ApplicationException
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">消息</param>
        public DLRuntimeInteroperationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="ex">异常</param>
        public DLRuntimeInteroperationException(string message, System.Exception ex)
            : base(message, ex)
        {

        }
    }
}
