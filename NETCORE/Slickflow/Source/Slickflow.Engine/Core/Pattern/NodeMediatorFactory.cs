/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。
* 
The Slickflow project.
Copyright (C) 2014  .NET Workflow Engine Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/

using System;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Core.Pattern.Event;
using Slickflow.Engine.Core.Pattern.Gateway;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// 节点执行器的工厂类
    /// </summary>
    internal class NodeMediatorFactory
    {
        /// <summary>
        /// 创建节点执行器的抽象类
        /// </summary>
        /// <param name="forwardContext">活动上下文</param>
        /// <param name="session">会话</param>
        /// <returns></returns>
        internal static NodeMediator CreateNodeMediator(ActivityForwardContext forwardContext,
            IDbSession session)
        {
            if (forwardContext.Activity.ActivityType == ActivityTypeEnum.StartNode)         //开始节点
            {
                if (forwardContext.Activity.ActivityTypeDetail.TriggerType == TriggerTypeEnum.None)
                {
                    return new NodeMediatorStart(forwardContext, session);
                }
                else
                {
                    throw new ApplicationException(LocalizeHelper.GetEngineMessage("nodemediatorfactory.CreateNodeMediator.uncertain.warn",
                        string.Format("ActivityType:{0},trigger:{1}", forwardContext.Activity.ActivityType.ToString(),
                                                         forwardContext.Activity.ActivityTypeDetail.TriggerType)
                    ));
                }
            }
            else if (forwardContext.Activity.ActivityType == ActivityTypeEnum.EndNode)
            {
                if (forwardContext.Activity.ActivityTypeDetail.TriggerType == TriggerTypeEnum.None)
                {
                    return new NodeMediatorEnd(forwardContext, session);
                }
                else
                {
                    throw new ApplicationException(LocalizeHelper.GetEngineMessage("nodemediatorfactory.CreateNodeMediator.uncertain.warn",
                        string.Format("ActivityType:{0},trigger:{1}", forwardContext.Activity.ActivityType.ToString(),
                                                         forwardContext.Activity.ActivityTypeDetail.TriggerType)
                    ));
                }
            }
            else if (forwardContext.Activity.ActivityType == ActivityTypeEnum.TaskNode)         //任务节点
            {
                return new NodeMediatorTask(forwardContext, session);
            }
            else if (forwardContext.Activity.ActivityType == ActivityTypeEnum.ServiceNode)      //自动服务节点
            {
                return new NodeMediatorService(forwardContext, session);
            }
            else if (forwardContext.Activity.ActivityType == ActivityTypeEnum.SubProcessNode)
            {
                return new NodeMediatorSubProcess(forwardContext, session);
            }
            else
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("nodemediatorfactory.CreateNodeMediator.uncertain.warn", 
                    forwardContext.Activity.ActivityType.ToString()));
            }
        }

        /// <summary>
        /// 创建Gateway节点处理类实例
        /// </summary>
        /// <param name="gActivity">节点</param>
        /// <param name="processModel">流程模型类</param>
        /// <param name="session">会话</param>
        /// <returns>Gateway节点处理实例</returns>
        internal static NodeMediatorGateway CreateNodeMediatorGateway(ActivityEntity gActivity,
            IProcessModel processModel,
            IDbSession session)
        {
            NodeMediatorGateway nodeMediator = null;
            if (gActivity.ActivityType == ActivityTypeEnum.GatewayNode)
            {
                if (gActivity.GatewayDirectionType == GatewayDirectionEnum.AndSplit)
                {
                    nodeMediator = new NodeMediatorAndSplit(gActivity, processModel, session);
                }
                else if (gActivity.GatewayDirectionType == GatewayDirectionEnum.OrSplit)
                {
                    nodeMediator = new NodeMediatorOrSplit(gActivity, processModel, session);
                }
                else if (gActivity.GatewayDirectionType == GatewayDirectionEnum.XOrSplit)
                {
                    nodeMediator = new NodeMediatorXOrSplit(gActivity, processModel, session);
                }
                else if (gActivity.GatewayDirectionType == GatewayDirectionEnum.AndJoin)
                {
                    nodeMediator = new NodeMediatorAndJoin(gActivity, processModel, session);
                }
                else if (gActivity.GatewayDirectionType == GatewayDirectionEnum.OrJoin)
                {
                    nodeMediator = new NodeMediatorOrJoin(gActivity, processModel, session);
                }
                else if (gActivity.GatewayDirectionType == GatewayDirectionEnum.XOrJoin)
                {
                    nodeMediator = new NodeMediatorXOrJoin(gActivity, processModel, session);
                }
                else if (gActivity.GatewayDirectionType == GatewayDirectionEnum.EOrJoin)
                {
                    nodeMediator = new NodeMediatorEOrJoin(gActivity, processModel, session);
                }
                else
                {
                    throw new XmlDefinitionException(LocalizeHelper.GetEngineMessage("nodemediatorfactory.CreateNodeMediatorGateway.uncertaingateway.warn", 
                        gActivity.GatewayDirectionType.ToString()));
                }
            }
            else
            {
                throw new XmlDefinitionException(LocalizeHelper.GetEngineMessage("nodemediatorfactory.CreateNodeMediatorGateway.uncertain.warn",
                    gActivity.ActivityType.ToString()));
            }
            return nodeMediator;
        }

        /// <summary>
        /// 创建结束事件节点处理类实例
        /// </summary>
        /// <param name="forwardContext">活动上下文</param>
        /// <param name="endActivity">节点</param>
        /// <param name="session">会话</param>
        /// <returns>节点处理实例</returns>
        internal static NodeMediator CreateNodeMediatorEnd(ActivityForwardContext forwardContext, 
            ActivityEntity endActivity, 
            IDbSession session)
        {

            if (endActivity.ActivityTypeDetail.TriggerType == TriggerTypeEnum.None)
            {
                return new NodeMediatorEnd(forwardContext, session);
            }
            else
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("nodemediatorfactory.CreateNodeMediator.uncertain.warn",
                    string.Format("ActivityType:{0},trigger:{1}", endActivity.ActivityType.ToString(),
                                                        endActivity.ActivityTypeDetail.TriggerType)
                ));
            }
        }
    }
}
