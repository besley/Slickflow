using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Core.Pattern
{
    internal class GatewayExecutedResult
    {
        internal string Message
        {
            get;
            set;
        }

        internal GatewayExecutedStatus Status
        {
            get;
            set;
        }

        private GatewayExecutedResult(GatewayExecutedStatus status,
            string message)
        {
            Status = status;
            Message = message;
        }

        internal static GatewayExecutedResult CreateGatewayExecutedResult(GatewayExecutedStatus status)
        {
            GatewayExecutedResult result = new GatewayExecutedResult(GatewayExecutedStatus.Unknown, "Gateway节点的执行状态未知！");
            switch (status)
            {
                case GatewayExecutedStatus.Successed:
                    result = new GatewayExecutedResult(GatewayExecutedStatus.Successed, "Gateway节点成功执行！");
                    break;
                case GatewayExecutedStatus.FallBehindOfXOrJoin:
                    result = new GatewayExecutedResult(GatewayExecutedStatus.FallBehindOfXOrJoin, "第一个满足条件的节点已经执行，互斥合并节点不能再次被实例化！");
                    break;
                default:
                    break;
            }
            return result;
        }
    }

    
}
