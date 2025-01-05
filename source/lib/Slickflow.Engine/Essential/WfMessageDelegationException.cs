using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Essential
{
    /// <summary>
    /// Message Delegation Exception
    /// 消息代理服务异常类
    /// </summary>
    public class WfMessageDelegationException : ApplicationException
    {
        public WfMessageDelegationException(string message)
            : base(message)
        {

        }

        public WfMessageDelegationException(string message, Exception ex)
            : base(message, ex)
        {

        }
    }
}
