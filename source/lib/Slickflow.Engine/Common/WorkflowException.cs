using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Process business data access exception class
    /// 流程业务数据访问异常类
    /// </summary>
    public class WorkflowException : ApplicationException
    {
        /// <summary>
        /// Constructor function
        /// </summary>
        /// <param name="message"></param>
        public WorkflowException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor function
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public WorkflowException(string message, Exception ex)
            : base(message, ex)
        {

        }
    }
}
