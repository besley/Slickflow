using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Workflow XML Definition File Exception Class
    /// 工作流xml定义文件异常类
    /// </summary>
    public class WfXpdlException : ApplicationException
    {
        /// <summary>
        /// Constructor function
        /// </summary>
        /// <param name="message"></param>
        public WfXpdlException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor function
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public WfXpdlException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
