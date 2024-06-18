using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// Gateway执行结果类
    /// </summary>
    internal class NodeAutoExecutedResult
    {
        internal string Message
        {
            get;
            set;
        }

        internal NodeAutoExecutedStatus Status
        {
            get;
            set;
        }

        private NodeAutoExecutedResult(NodeAutoExecutedStatus status,
            string message)
        {
            Status = status;
            Message = message;
        }

        internal static NodeAutoExecutedResult CreateGatewayExecutedResult(NodeAutoExecutedStatus status)
        {
            NodeAutoExecutedResult result = new NodeAutoExecutedResult(NodeAutoExecutedStatus.Unknown, "Unknown Gateway executing status!");
            switch (status)
            {
                case NodeAutoExecutedStatus.Successed:
                    result = new NodeAutoExecutedResult(NodeAutoExecutedStatus.Successed, "Gateway executed successfully!");
                    break;
                case NodeAutoExecutedStatus.FallBehindOfXOrJoin:
                    result = new NodeAutoExecutedResult(NodeAutoExecutedStatus.FallBehindOfXOrJoin, "The first matched node has been executed(XOrJoin)!");
                    break;
                default:
                    break;
            }
            return result;
        }
    }

    
}
