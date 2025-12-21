
using System;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Core.Pattern.Auto;
using Slickflow.Engine.Core.Pattern.Event;
using Slickflow.Engine.Core.Pattern.Event.Timer;
using Slickflow.Engine.Core.Pattern.Event.Message;
using Slickflow.Engine.Core.Pattern.Event.Conditional;
using Slickflow.Engine.Core.Pattern.Event.Signal;
using Slickflow.Engine.Core.Pattern.Gateway;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl.Node;
using static IronPython.Modules._ast;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// Node Mediator Factory
    /// 节点执行器的工厂类
    /// </summary>
    internal class NodeMediatorFactory
    {
        /// <summary>
        /// Create Method
        /// </summary>
        /// <param name="forwardContext"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        internal static NodeMediator CreateNodeMediator(ActivityForwardContext forwardContext,
            IDbSession session)
        {
            if (forwardContext.CurrentActivity.ActivityType == ActivityTypeEnum.StartNode)        
            {
                if (forwardContext.CurrentActivity.TriggerDetail == null || 
                    forwardContext.CurrentActivity.TriggerDetail.TriggerType == TriggerTypeEnum.None)
                {
                    return new NodeMediatorStart(forwardContext, session);
                }
                else if (forwardContext.CurrentActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Timer)
                {
                    return new NodeMediatorStartTimer(forwardContext, session);
                }
                else if (forwardContext.CurrentActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Conditional)
                {
                    return new NodeMediatorStartConditional(forwardContext, session);
                }
                else if (forwardContext.CurrentActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Message)
                {
                    if (forwardContext.CurrentActivity.TriggerDetail.MessageDirection == MessageDirectionEnum.Catch)
                    {
                        return new NodeMediatorStartMsgCatch(forwardContext, session);
                    }
                    else if (forwardContext.CurrentActivity.TriggerDetail.MessageDirection == MessageDirectionEnum.Throw)
                    {
                        return new NodeMediatorStartMsgThrow(forwardContext, session);
                    }
                    else
                    {
                        ThrowUncerternXmlNodeTypeException(forwardContext.CurrentActivity.ActivityType.ToString(), 
                            forwardContext.CurrentActivity.TriggerDetail.TriggerType.ToString(),
                            forwardContext.CurrentActivity.TriggerDetail.MessageDirection.ToString());
                        return null;
                    }
                }
                else if (forwardContext.CurrentActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Signal)
                {
                    if (forwardContext.CurrentActivity.TriggerDetail.MessageDirection == MessageDirectionEnum.Catch)
                    {
                        return new NodeMediatorStartSignalCatch(forwardContext, session);
                    }
                    else if (forwardContext.CurrentActivity.TriggerDetail.MessageDirection == MessageDirectionEnum.Throw)
                    {
                        return new NodeMediatorStartSignalThrow(forwardContext, session);
                    }
                    else
                    {
                        ThrowUncerternXmlNodeTypeException(forwardContext.CurrentActivity.ActivityType.ToString(),
                            forwardContext.CurrentActivity.TriggerDetail.TriggerType.ToString(),
                            forwardContext.CurrentActivity.TriggerDetail.MessageDirection.ToString());
                        return null;
                    }
                }
                else
                {
                    ThrowUncerternXmlNodeTypeException(forwardContext.CurrentActivity.ActivityType.ToString(),
                        forwardContext.CurrentActivity.TriggerDetail.TriggerType.ToString());
                    return null;
                }
            }
            else if (forwardContext.CurrentActivity.ActivityType == ActivityTypeEnum.EndNode)
            {
                if (forwardContext.CurrentActivity.TriggerDetail == null || 
                    forwardContext.CurrentActivity.TriggerDetail.TriggerType == TriggerTypeEnum.None)
                {
                    return new NodeMediatorEnd(forwardContext, session);
                }
                else if (forwardContext.CurrentActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Timer)
                {
                    return new NodeMediatorEndTimer(forwardContext, session);
                }
                else if (forwardContext.CurrentActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Message)
                {
                    if (forwardContext.CurrentActivity.TriggerDetail.MessageDirection == MessageDirectionEnum.Catch)
                    {
                        return new NodeMediatorEndMsgCatch(forwardContext, session);
                    }
                    else if (forwardContext.CurrentActivity.TriggerDetail.MessageDirection == MessageDirectionEnum.Throw)
                    {
                        return new NodeMediatorEndMsgThrow(forwardContext, session);
                    }
                    else
                    {
                        ThrowUncerternXmlNodeTypeException(forwardContext.CurrentActivity.ActivityType.ToString(),
                            forwardContext.CurrentActivity.TriggerDetail.TriggerType.ToString(),
                            forwardContext.CurrentActivity.TriggerDetail.MessageDirection.ToString());
                        return null;
                    }
                }
                else if (forwardContext.CurrentActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Signal)
                {
                    if (forwardContext.CurrentActivity.TriggerDetail.MessageDirection == MessageDirectionEnum.Catch)
                    {
                        return new NodeMediatorEndSignalCatch(forwardContext, session);
                    }
                    else if (forwardContext.CurrentActivity.TriggerDetail.MessageDirection == MessageDirectionEnum.Throw)
                    {
                        return new NodeMediatorEndSignalThrow(forwardContext, session);
                    }
                    else
                    {
                        ThrowUncerternXmlNodeTypeException(forwardContext.CurrentActivity.ActivityType.ToString(),
                            forwardContext.CurrentActivity.TriggerDetail.TriggerType.ToString(),
                            forwardContext.CurrentActivity.TriggerDetail.MessageDirection.ToString());
                        return null;
                    }
                }
                else
                {
                    ThrowUncerternXmlNodeTypeException(forwardContext.CurrentActivity.ActivityType.ToString(),
                        forwardContext.CurrentActivity.TriggerDetail.TriggerType.ToString());
                    return null;
                }
            }
            else if (forwardContext.CurrentActivity.ActivityType == ActivityTypeEnum.IntermediateNode)
            {
                if (forwardContext.CurrentActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Timer)
                {
                    return new NodeMediatorInterTimerGo(forwardContext, session);
                }
                else if (forwardContext.CurrentActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Conditional)
                {
                    return new NodeMediatorInterConditionalGo(forwardContext, session);
                }
                else if (forwardContext.CurrentActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Message)
                {
                    if (forwardContext.CurrentActivity.TriggerDetail.MessageDirection == MessageDirectionEnum.Catch)
                    {
                        return new NodeMediatorInterMsgCatchContinue(forwardContext, session);
                    }
                    else if (forwardContext.CurrentActivity.TriggerDetail.MessageDirection == MessageDirectionEnum.Throw)
                    {
                        return new NodeMediatorInterMsgThrow(forwardContext, session);
                    }
                    else
                    {
                        ThrowUncerternXmlNodeTypeException(forwardContext.CurrentActivity.ActivityType.ToString(),
                            forwardContext.CurrentActivity.TriggerDetail.TriggerType.ToString(),
                            forwardContext.CurrentActivity.TriggerDetail.MessageDirection.ToString());
                        return null;
                    }
                }
                else if (forwardContext.CurrentActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Signal)
                {
                    if (forwardContext.CurrentActivity.TriggerDetail.MessageDirection == MessageDirectionEnum.Catch)
                    {
                        return new NodeMediatorInterSignalCatchContinue(forwardContext, session);
                    }
                    else if (forwardContext.CurrentActivity.TriggerDetail.MessageDirection == MessageDirectionEnum.Throw)
                    {
                        return new NodeMediatorInterSignalThrow(forwardContext, session);
                    }
                    else
                    {
                        ThrowUncerternXmlNodeTypeException(forwardContext.CurrentActivity.ActivityType.ToString(),
                            forwardContext.CurrentActivity.TriggerDetail.TriggerType.ToString(),
                            forwardContext.CurrentActivity.TriggerDetail.MessageDirection.ToString());
                        return null;
                    }
                }
                else
                {
                    ThrowUncerternXmlNodeTypeException(forwardContext.CurrentActivity.ActivityType.ToString(),
                        forwardContext.CurrentActivity.TriggerDetail.TriggerType.ToString());
                    return null;
                }
            }
            else if (forwardContext.CurrentActivity.ActivityType == ActivityTypeEnum.TaskNode)        
            {
                //普通任务节点，运行时临时加签变为多实例节点
                //Normal task node, temporarily signed at runtime to become a multi instance node
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
                else if(forwardContext.FromActivityInstance.MainActivityInstanceId != null)
                {
                    //加签子实例
                    //Signature forward sub instance
                    return new NodeMediatorMultiSignForward(forwardContext, session);
                }
                else
                {
                    return new NodeMediatorTask(forwardContext, session);
                } 
            }
            else if (forwardContext.CurrentActivity.ActivityType == ActivityTypeEnum.ServiceNode)      
            {
                return new NodeMediatorService(forwardContext, session);
            }
            else if (forwardContext.CurrentActivity.ActivityType == ActivityTypeEnum.AIServiceNode)
            {
                return new NodeMediatorAIService(forwardContext, session);
            }
            else if (forwardContext.CurrentActivity.ActivityType == ActivityTypeEnum.ScriptNode)
            {
                return new NodeMediatorScript(forwardContext, session);
            }
            else if (forwardContext.CurrentActivity.ActivityType == ActivityTypeEnum.MultiSignNode)
            {
                if (forwardContext.FromActivityInstance.MainActivityInstanceId != null)
                {
                    if (forwardContext.CurrentActivity.MultiSignDetail.ComplexType == ComplexTypeEnum.SignTogether)
                    {
                        return new NodeMediatorMultiSignTogether(forwardContext, session);
                    }
                    else if (forwardContext.CurrentActivity.MultiSignDetail.ComplexType == ComplexTypeEnum.SignForward)
                    {
                        return new NodeMediatorMultiSignForward(forwardContext, session);
                    }
                    else
                    {
                        throw new ApplicationException(LocalizeHelper.GetEngineMessage("nodemediatorfactory.CreateNodeMediator.error"));
                    }
                }
                else if (forwardContext.FromActivityInstance.MainActivityInstanceId == null)
                {
                    //加签主节点的分发操作
                    //Distribution operation of signing the main node
                    if (forwardContext.CurrentActivity.MultiSignDetail.ComplexType == ComplexTypeEnum.SignForward)
                    {
                        var aim = new ActivityInstanceManager();
                        var miChildList = aim.GetActivityMulitipleInstanceWithState(forwardContext.FromActivityInstance.Id, forwardContext.FromActivityInstance.ProcessInstanceId);

                        //加签的动态变量传入
                        //Pass in the signed dynamic variable
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
                        else if (miChildList.Count == 0)
                        {
                            return new NodeMediatorTask(forwardContext, session);
                        }
                        else
                        {
                            throw new ApplicationException(LocalizeHelper.GetEngineMessage("nodemediatorfactory.CreateNodeMediator.warn"));
                        }
                    }
                    else
                    {
                        throw new ApplicationException(LocalizeHelper.GetEngineMessage("nodemediatorfactory.CreateNodeMediator.error"));
                    }
                }
                else
                {
                    throw new ApplicationException(LocalizeHelper.GetEngineMessage("nodemediatorfactory.CreateNodeMediator.error"));
                }
            }
            else if (forwardContext.CurrentActivity.ActivityType == ActivityTypeEnum.SubProcessNode)
            {
                var nodeMediatorSubProcess = NodeMediatorFactory.CreateNodeMediatorSubProcess(forwardContext, forwardContext.CurrentActivity, session);
                return nodeMediatorSubProcess;
            }
            else
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("nodemediatorfactory.CreateNodeMediator.uncertain.warn",
                    forwardContext.CurrentActivity.ActivityType.ToString()));
            }
        }

        /// <summary>
        /// Throw uncertern xml node type exception
        /// </summary>
        /// <param name="activityType"></param>
        /// <param name="triggerType"></param>
        /// <param name="throwCatchDirection"></param>
        /// <exception cref="ApplicationException"></exception>
        private static void ThrowUncerternXmlNodeTypeException(string activityType, string triggerType, string throwCatchDirection = null)
        {
            if (!string.IsNullOrEmpty(throwCatchDirection))
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("nodemediatorfactory.CreateNodeMediator.uncertain.warn",
                    string.Format("ActivityType:{0},trigger:{1},direction:{2}", activityType, triggerType, throwCatchDirection)
                ));
            }
            else
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("nodemediatorfactory.CreateNodeMediator.uncertain.warn",
                    string.Format("ActivityType:{0},trigger:{1}", activityType, triggerType)
                ));
            }
        }

        /// <summary>
        /// Create Node Mediator Gateway
        /// 创建Gateway节点处理类实例
        /// </summary>
        /// <param name="gActivity"></param>
        /// <param name="processModel"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        internal static NodeMediatorGateway CreateNodeMediatorGateway(Activity gActivity,
            IProcessModel processModel,
            IDbSession session)
        {
            NodeMediatorGateway nodeMediator = null;
            if (gActivity.ActivityType == ActivityTypeEnum.GatewayNode)
            {
                if (gActivity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndSplit)
                {
                    nodeMediator = new NodeMediatorAndSplit(gActivity, processModel, session);
                }
                else if (gActivity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndSplitMI)
                {
                    nodeMediator = new NodeMediatorAndSplitMI(gActivity, processModel, session);
                }
                else if (gActivity.GatewayDetail.DirectionType == GatewayDirectionEnum.OrSplit)
                {
                    nodeMediator = new NodeMediatorOrSplit(gActivity, processModel, session);
                }
                else if (gActivity.GatewayDetail.DirectionType == GatewayDirectionEnum.XOrSplit)
                {
                    nodeMediator = new NodeMediatorXOrSplit(gActivity, processModel, session);
                }
                else if (gActivity.GatewayDetail.DirectionType == GatewayDirectionEnum.ApprovalOrSplit)
                {
                    nodeMediator = new NodeMediatorApprovalOrSplit(gActivity, processModel, session);
                }
                else if (gActivity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndJoin)
                {
                    nodeMediator = new NodeMediatorAndJoin(gActivity, processModel, session);
                }
                else if (gActivity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndJoinMI)
                {
                    nodeMediator = new NodeMediatorAndJoinMI(gActivity, processModel, session);
                }
                else if (gActivity.GatewayDetail.DirectionType == GatewayDirectionEnum.OrJoin)
                {
                    nodeMediator = new NodeMediatorOrJoin(gActivity, processModel, session);
                }
                else if (gActivity.GatewayDetail.DirectionType == GatewayDirectionEnum.XOrJoin)
                {
                    nodeMediator = new NodeMediatorXOrJoin(gActivity, processModel, session);
                }
                else if (gActivity.GatewayDetail.DirectionType == GatewayDirectionEnum.EOrJoin)
                {
                    nodeMediator = new NodeMediatorEOrJoin(gActivity, processModel, session);
                }
                else
                {
                    throw new XmlDefinitionException(LocalizeHelper.GetEngineMessage("nodemediatorfactory.CreateNodeMediatorGateway.uncertaingateway.warn", 
                        gActivity.GatewayDetail.DirectionType.ToString()));
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
        /// Create Node Mediator Event
        /// 创建中间事件节点处理类实例
        /// </summary>
        internal static NodeMediator CreateNodeMediatorEvent(ActivityForwardContext forwardContext,
            Activity eventActivity,
            IProcessModel processModel,
            IDbSession session)
        {
            NodeMediator nodeMediator = null;
            if (eventActivity.ActivityType == ActivityTypeEnum.IntermediateNode)
            {
                if (eventActivity.TriggerDetail != null)
                {
                    if (eventActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Timer)
                    {
                        nodeMediator = new NodeMediatorInterTimer(forwardContext, session);
                    }
                    else if (eventActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Conditional)
                    {
                        nodeMediator = new NodeMediatorInterConditional(forwardContext, session);
                    }
                    else if (eventActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Message)
                    {
                        if (eventActivity.TriggerDetail.MessageDirection == MessageDirectionEnum.Catch)
                        {
                            nodeMediator = new NodeMediatorInterMsgCatch(forwardContext, session);
                        }
                        else if (eventActivity.TriggerDetail.MessageDirection == MessageDirectionEnum.Throw)
                        {
                            nodeMediator = new NodeMediatorInterMsgThrow(forwardContext, session);
                        }
                        else
                        {
                            throw new XmlDefinitionException(LocalizeHelper.GetEngineMessage("nodemediatorfactory.CreateNodeMediatorEvent.uncertain.warn",
                                eventActivity.TriggerDetail.MessageDirection.ToString()));
                        }
                    }
                    else if (eventActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Signal)
                    {
                        if (eventActivity.TriggerDetail.MessageDirection == MessageDirectionEnum.Catch)
                        {
                            nodeMediator = new NodeMediatorInterSignalCatch(forwardContext, session);
                        }
                        else if (eventActivity.TriggerDetail.MessageDirection == MessageDirectionEnum.Throw)
                        {
                            nodeMediator = new NodeMediatorInterSignalThrow(forwardContext, session);
                        }
                        else
                        {
                            throw new XmlDefinitionException(LocalizeHelper.GetEngineMessage("nodemediatorfactory.CreateNodeMediatorEvent.uncertain.warn",
                                eventActivity.TriggerDetail.MessageDirection.ToString()));
                        }
                    }
                    else
                    {
                        throw new XmlDefinitionException(LocalizeHelper.GetEngineMessage("nodemediatorfactory.CreateNodeMediatorEvent.uncertain.warn",
                                eventActivity.TriggerDetail.MessageDirection.ToString()));
                    }
                }
                else
                {
                    nodeMediator = new NodeMediatorIntermediate(forwardContext, session);
                }
            }
            else if(eventActivity.ActivityType == ActivityTypeEnum.ServiceNode)
            {
                nodeMediator = new NodeMediatorService(forwardContext, session);
                nodeMediator.LinkContext.CurrentActivity = eventActivity;
            }
            else if(eventActivity.ActivityType == ActivityTypeEnum.AIServiceNode)
            {
                nodeMediator = new NodeMediatorAIService(forwardContext, session);
                nodeMediator.LinkContext.CurrentActivity = eventActivity;
            }
            else if (eventActivity.ActivityType == ActivityTypeEnum.ScriptNode)
            {
                nodeMediator = new NodeMediatorScript(forwardContext, session);
                nodeMediator.LinkContext.CurrentActivity = eventActivity;
            }
            else
            {
                throw new XmlDefinitionException(LocalizeHelper.GetEngineMessage("nodemediatorfactory.CreateNodeMediatorEvent.uncertain.warn",
                    eventActivity.ActivityType.ToString()));
            }
            return nodeMediator;
        }


        /// <summary>
        /// Create Node Mediator End
        /// 创建结束事件节点处理类实例
        /// </summary>
        /// <param name="forwardContext"></param>
        /// <param name="endActivity"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        internal static NodeMediator CreateNodeMediatorEnd(ActivityForwardContext forwardContext, 
            Activity endActivity, 
            IDbSession session)
        {
            if (endActivity.TriggerDetail == null 
                || endActivity.TriggerDetail.TriggerType == TriggerTypeEnum.None)
            {
                return new NodeMediatorEnd(forwardContext, session);
            }
            else if (endActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Timer)
            {
                return new NodeMediatorEndTimer(forwardContext, session);
            }
            else if (endActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Message)
            {
                if (endActivity.TriggerDetail.MessageDirection == MessageDirectionEnum.Catch)
                {
                    return new NodeMediatorEndMsgCatch(forwardContext, session);
                }
                else if (endActivity.TriggerDetail.MessageDirection == MessageDirectionEnum.Throw)
                {
                    return new NodeMediatorEndMsgThrow(forwardContext, session);
                }
                else
                {
                    throw new ApplicationException(LocalizeHelper.GetEngineMessage("nodemediatorfactory.CreateNodeMediator.uncertain.warn",
                        string.Format("ActivityType:{0},trigger:{1}", endActivity.ActivityType.ToString(),
                                                            endActivity.TriggerDetail.TriggerType)
                    ));
                }
            }
            else
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("nodemediatorfactory.CreateNodeMediator.uncertain.warn",
                    string.Format("ActivityType:{0},trigger:{1}", endActivity.ActivityType.ToString(),
                                                        endActivity.TriggerDetail.TriggerType)
                ));
            }
        }

        /// <summary>
        /// Create Node Mediator Sub Process
        /// 创建子流程节点处理类实例
        /// </summary>
        /// <param name="forwardContext"></param>
        /// <param name="activity"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        internal static NodeMediator CreateNodeMediatorSubProcess(ActivityForwardContext forwardContext,
            Activity activity,
            IDbSession session)
        {
            var subProcessNode = (SubProcessNode)activity.Node;
            if (subProcessNode.SubProcessNested != null)
            {
                return new NodeMediatorSubProcessNested(forwardContext, session);
            }
            else
            {
                return new NodeMediatorSubProcess(forwardContext, session);
            }
        }

        /// <summary>
        /// Create Node Mediator Sub Process
        /// 创建子流程节点处理类实例
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        internal static NodeMediator CreateNodeMediatorSubProcess(Activity activity,
            IDbSession session)
        {
            var subProcessNode = (SubProcessNode)activity.Node;
            if (subProcessNode.SubProcessNested != null)
            {
                return new NodeMediatorSubProcessNested(session);
            }
            else
            {
                return new NodeMediatorSubProcess(session);
            }
        }
    }
}
