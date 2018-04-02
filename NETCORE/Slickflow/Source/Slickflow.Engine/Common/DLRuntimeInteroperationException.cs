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
        public DLRuntimeInteroperationException(string message)
            : base(message)
        {
        }

        public DLRuntimeInteroperationException(string message, System.Exception ex)
            : base(message, ex)
        {

        }
    }
}
