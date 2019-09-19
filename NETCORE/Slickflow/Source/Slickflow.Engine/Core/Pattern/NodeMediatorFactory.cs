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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Data;
using Slickflow.Engine.Xpdl;

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
                return new NodeMediatorStart(forwardContext, session);
            }
            else if (forwardContext.Activity.ActivityType == ActivityTypeEnum.TaskNode)         //任务节点
            {
                return new NodeMediatorTask(forwardContext, session);
            }
            else if (forwardContext.Activity.ActivityType == ActivityTypeEnum.MultipleInstanceNode)         //多实例节点
            {
                if (forwardContext.FromActivityInstance.MIHostActivityInstanceID != null)
                {
                    if (forwardContext.Activity.ActivityTypeDetail.ComplexType == Xpdl.ComplexTypeEnum.SignTogether)        //会签子节点
                    {
                        return new NodeMediatorMISignTogether(forwardContext, session);
                    }
                    else if (forwardContext.Activity.ActivityTypeDetail.ComplexType == Xpdl.ComplexTypeEnum.SignForward)            //加签子节点
                    {
                        return new NodeMediatorMISignForward(forwardContext, session);
                    }
                    else
                    {
                        throw new ApplicationException("未知的多实例节点类型！");
                    }
                }
                else if (forwardContext.FromActivityInstance.MIHostActivityInstanceID == null
                    && forwardContext.Activity.ActivityTypeDetail.ComplexType == Xpdl.ComplexTypeEnum.SignForward)        //加签主节点的分发操作
                {
                    //加签的动态变量传入
                    var controlParamSheet = forwardContext.ActivityResource.AppRunner.ControlParameterSheet;
                    if (controlParamSheet != null)
                    {
                        if (!string.IsNullOrEmpty(controlParamSheet.SignForwardType)
                            && controlParamSheet.SignForwardType.ToUpper() != "NONE")
                        {
                            return new NodeMediatorSignForward(forwardContext, session);
                        }
                        else
                        {
                            return new NodeMediatorTask(forwardContext, session);
                        }
                    }
                    else
                    {
                        throw new ApplicationException("加签类型的动态变量未传入，不能确定加签的子类型！");
                    }
                }
                else
                {
                    throw new ApplicationException("未知的多实例节点类型！");
                }
            }
            else if (forwardContext.Activity.ActivityType == ActivityTypeEnum.SubProcessNode)
            {
                return new NodeMediatorSubProcess(forwardContext, session);
            }
            else if (forwardContext.Activity.ActivityType == ActivityTypeEnum.EndNode)
            {
                return new NodeMediatorEnd(forwardContext, session);
            }
            else
            {
                throw new ApplicationException(string.Format("不明确的节点类型: {0}", forwardContext.Activity.ActivityType.ToString()));
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
                else if (gActivity.GatewayDirectionType == GatewayDirectionEnum.AndSplitMI)
                {
                    nodeMediator = new NodeMediatorAndSplitMI(gActivity, processModel, session);
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
                else if (gActivity.GatewayDirectionType == GatewayDirectionEnum.AndJoinMI)
                {
                    nodeMediator = new NodeMediatorAndJoinMI(gActivity, processModel, session);
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
                    throw new XmlDefinitionException(string.Format("不明确的节点分支Gateway类型！{0}", gActivity.GatewayDirectionType.ToString()));
                }
            }
            else
            {
                throw new XmlDefinitionException(string.Format("不明确的节点类型！{0}", gActivity.ActivityType.ToString()));
            }
            return nodeMediator;
        }

        /// <summary>
        /// 创建中间事件节点处理类实例
        /// </summary>
        /// <param name="forwardContext">活动上下文</param>
        /// <param name="eActivity">节点</param>
        /// <param name="processModel">流程模型类</param>
        /// <param name="session">会话</param>
        /// <returns>Gateway节点处理实例</returns>
        internal static NodeMediatorEvent CreateNodeMediatorEvent(ActivityForwardContext forwardContext,
            ActivityEntity eActivity,
            IProcessModel processModel,
            IDbSession session)
        {
            NodeMediatorEvent nodeMediator = null;
            if (eActivity.ActivityType == ActivityTypeEnum.IntermediateNode)
            {
                nodeMediator = new NodeMediatorIntermediate(forwardContext, eActivity, processModel, session);
            }
            else
            {
                throw new XmlDefinitionException(string.Format("不明确的节点类型！{0}", eActivity.ActivityType.ToString()));
            }
            return nodeMediator;
        }
    }
}
