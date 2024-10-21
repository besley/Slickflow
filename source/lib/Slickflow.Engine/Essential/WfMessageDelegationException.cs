using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Essential
{
    /// <summary>
    /// 消息代理服务异常类
    /// </summary>
    public class WfMessageDelegationException : ApplicationException
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">消息</param>
        public WfMessageDelegationException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="ex">异常</param>
        public WfMessageDelegationException(string message, Exception ex)
            : base(message, ex)
        {

        }
    }
}
