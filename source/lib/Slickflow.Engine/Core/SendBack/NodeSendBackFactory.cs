using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Core.SendBack
{
    /// <summary>
    /// 创建退回节点处理器
    /// </summary>
    internal class NodeSendBackFactory
    {
        /// <summary>
        /// 创建方法
        /// </summary>
        /// <param name="sendbackOperation">退回选项</param>
        /// <param name="session">会话</param>
        /// <returns>退回器</returns>
        internal static NodeSendBack CreateNodeReverter(SendBackOperation sendbackOperation, IDbSession session)
        {
            NodeSendBack nodeSendBack = null;
            if (sendbackOperation.CurrentNodeOperationType == SendBackOperationTypeEnum.Normal)
            {
                if (sendbackOperation.PreviousNodeOperationType == SendBackOperationTypeEnum.Normal)
                {
                    nodeSendBack = new NodeSendBackTask(sendbackOperation, session);
                }
                else if (sendbackOperation.PreviousNodeOperationType == SendBackOperationTypeEnum.MISPreviousIsLastOne)
                {
                    nodeSendBack = new NodeSendBackToMISPrevious(sendbackOperation, session);
                }
                else if (sendbackOperation.PreviousNodeOperationType == SendBackOperationTypeEnum.MIPAllIsInCompletedState)
                {
                    nodeSendBack = new NodeSendBackToMIPPrevious(sendbackOperation, session);
                }
                else
                {
                    //如果有其它模式，没有处理到，则直接抛出异常
                    throw new WorkflowException(LocalizeHelper.GetEngineMessage("nodesendbackfactory.CreateNodeReverter.error",
                        sendbackOperation.PreviousNodeOperationType.ToString()));
                }
            }
            else if (sendbackOperation.CurrentNodeOperationType == SendBackOperationTypeEnum.MultipleInstance)
            {
                if (sendbackOperation.CurrentMultipleInstanceDetailType == SendBackOperationTypeEnum.MISFirstOneIsRunning)
                {
                    nodeSendBack = new NodeSendBackMISReady(sendbackOperation, session);
                }
                else if(sendbackOperation.CurrentMultipleInstanceDetailType == SendBackOperationTypeEnum.MISOneIsRunning)
                {
                    nodeSendBack = new NodeSendBackMIS(sendbackOperation, session);
                }
                else if(sendbackOperation.CurrentMultipleInstanceDetailType == SendBackOperationTypeEnum.MIPOneIsRunning)
                {
                    nodeSendBack = new NodeSendBackMIP(sendbackOperation, session);
                }
                else
                {
                    //如果有其它模式，没有处理到，则直接抛出异常
                    throw new WorkflowException(LocalizeHelper.GetEngineMessage("nodesendbackfactory.CreateNodeReverter.error",
                        sendbackOperation.CurrentMultipleInstanceDetailType.ToString()));
                }
            }
            return nodeSendBack;
        }
    }
}
