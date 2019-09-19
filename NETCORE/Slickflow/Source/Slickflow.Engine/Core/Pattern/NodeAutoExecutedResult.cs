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
            NodeAutoExecutedResult result = new NodeAutoExecutedResult(NodeAutoExecutedStatus.Unknown, "Gateway节点的执行状态未知！");
            switch (status)
            {
                case NodeAutoExecutedStatus.Successed:
                    result = new NodeAutoExecutedResult(NodeAutoExecutedStatus.Successed, "Gateway节点成功执行！");
                    break;
                case NodeAutoExecutedStatus.FallBehindOfXOrJoin:
                    result = new NodeAutoExecutedResult(NodeAutoExecutedStatus.FallBehindOfXOrJoin, "第一个满足条件的节点已经执行，互斥合并节点不能再次被实例化！");
                    break;
                default:
                    break;
            }
            return result;
        }
    }

    
}
