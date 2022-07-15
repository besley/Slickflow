using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 流程业务数据访问异常类
    /// </summary>
    public class WorkflowException : ApplicationException
    {
        public WorkflowException(string message)
            : base(message)
        {

        }

        public WorkflowException(string message, Exception ex)
            : base(message, ex)
        {

        }
    }

    /// <summary>
    /// 工作流xml定义文件异常类
    /// </summary>
    public class WfXpdlException : ApplicationException
    {
        public WfXpdlException(string message)
            : base(message)
        {
        }

        public WfXpdlException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
