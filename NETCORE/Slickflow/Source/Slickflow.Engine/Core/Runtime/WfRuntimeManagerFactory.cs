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
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Core.Event;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.SendBack;
using Slickflow.Engine.Core.Parser;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Core.Runtime
{
    /// <summary>
    /// 运行时的创建类
    /// 静态方法：创建执行实例的运行者对象
    /// </summary>
    internal class WfRuntimeManagerFactory
    {
        #region WfRuntimeManager 创建启动运行时对象
        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="runner">执行者</param>
        /// <param name="result">结果对象</param>
        /// <returns>运行时实例对象</returns>
        public static WfRuntimeManager CreateRuntimeInstanceStartup(WfAppRunner runner,
            ref WfExecutedResult result)
        {
            //检查流程是否可以被启动
            var rmins = new WfRuntimeManagerStartup();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            //正常流程启动
            var pim = new ProcessInstanceManager();
            ProcessInstanceEntity processInstance = pim.GetProcessInstanceCurrent(runner.AppInstanceID,
                runner.ProcessGUID);

            //不能同时启动多个主流程
            if (processInstance != null
                && processInstance.ParentProcessInstanceID == null
                && processInstance.ProcessState == (short)ProcessStateEnum.Running)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Started_IsRunningAlready;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceStartup.error");
                return rmins;
            }

            rmins.AppRunner = runner;

            //获取流程第一个可办理节点
            rmins.ProcessModel = ProcessModelFactory.Create(runner.ProcessGUID, runner.Version);
            var startActivity = rmins.ProcessModel.GetStartActivity();
            var nextActivityTree = rmins.ProcessModel.GetNextActivityTree(startActivity.ActivityGUID, runner.Conditions);

            //开始节点之后可以有网关节点
            if (startActivity.ActivityTypeDetail.TriggerType == TriggerTypeEnum.None)
            {
                rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(nextActivityTree,
                    runner.UserID,
                    runner.UserName);
            }
            else if (startActivity.ActivityTypeDetail.TriggerType == TriggerTypeEnum.Timer
                || startActivity.ActivityTypeDetail.TriggerType == TriggerTypeEnum.Message
                || startActivity.ActivityTypeDetail.TriggerType == TriggerTypeEnum.Conditional)
            {
                if (!string.IsNullOrEmpty(runner.UserID))
                {
                    rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(nextActivityTree,
                        runner.UserID,
                        runner.UserName);
                }
                else
                {
                    rmins.AppRunner.NextActivityPerformers = rmins.ProcessModel.GetActivityPerformers(nextActivityTree);
                }
            }
            rmins.ActivityResource = new ActivityResource(runner, rmins.AppRunner.NextActivityPerformers);

            return rmins;
        }

        /// <summary>
        /// 子流程启动
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <param name="parentProcessInstance">父流程</param>
        /// <param name="subProcessNode">子流程节点</param>
        /// <param name="performerList">执行者列表</param>
        /// <param name="session">数据库会话</param>
        /// <param name="result">运行结果</param>
        /// <returns>运行时管理器</returns>
        public static WfRuntimeManager CreateRuntimeInstanceStartupSub(WfAppRunner runner,
            ProcessInstanceEntity parentProcessInstance,
            SubProcessNode subProcessNode,
            PerformerList performerList,
            IDbSession session,
            ref WfExecutedResult result)
        {
            //检查流程是否可以被启动
            var rmins = new WfRuntimeManagerStartupSub();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            var pim = new ProcessInstanceManager();
            ProcessInstanceEntity processInstance = pim.GetProcessInstanceCurrent(session.Connection,
                runner.AppInstanceID,
                subProcessNode.SubProcessGUID,
                session.Transaction);

            //不能同时启动多个主流程
            if (processInstance != null
                && processInstance.ParentProcessInstanceID == null
                && processInstance.ProcessState == (short)ProcessStateEnum.Running)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Started_IsRunningAlready;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceStartup.error");
                return rmins;
            }

            //processInstance 为空，此时继续执行启动操作
            rmins.AppRunner = runner;
            rmins.ParentProcessInstance = parentProcessInstance;
            rmins.InvokedSubProcessNode = subProcessNode;

            //获取流程第一个可办理节点
            rmins.ProcessModel = ProcessModelFactory.Create(runner.ProcessGUID, runner.Version);
            var startActivity = rmins.ProcessModel.GetStartActivity();
            var firstActivity = rmins.ProcessModel.GetFirstActivity();

            //子流程自动获取第一个办理节点上的人员列表
            rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(firstActivity.ActivityGUID, 
                performerList);
            rmins.ActivityResource = new ActivityResource(runner, rmins.AppRunner.NextActivityPerformers);

            return rmins;
        }
        #endregion

        #region WfRuntimeManager 创建应用执行运行时对象
        /// <summary>
        /// 创建运行时实例对象
        /// </summary>
        /// <param name="runner">执行者</param>
        /// <param name="session">数据库会话</param>
        /// <param name="result">结果对象</param>
        /// <returns>运行时实例对象</returns>
        public static WfRuntimeManager CreateRuntimeInstanceAppRunning(WfAppRunner runner,
            IDbSession session,
            ref WfExecutedResult result)
        {
            //检查传人参数是否有效
            var rmins = new WfRuntimeManagerRun();
            rmins.WfExecutedResult = result = new WfExecutedResult();
            if (string.IsNullOrEmpty(runner.AppName)
                || String.IsNullOrEmpty(runner.AppInstanceID)
                || runner.ProcessGUID == null)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.RunApp_ErrorArguments;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceAppRunning.missing.error");
                return rmins;
            }

            //传递runner变量
            rmins.AppRunner = runner;

            var aim = new ActivityInstanceManager();
            TaskViewEntity taskView = null;
            var runningNode = aim.GetRunningNode(runner, session, out taskView);

            //判断是否是当前登录用户的任务
            if (!string.IsNullOrEmpty(runningNode.AssignedToUserIDs)
                && runningNode.AssignedToUserIDs.Contains(runner.UserID.ToString()) == false)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.RunApp_HasNoTask;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceAppRunning.nonetask.error");
                return rmins;
            }

            var isRunning = (new TaskManager()).CheckTaskStateInRunningState(taskView);
            if (isRunning == false)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_NotInRunning;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceAppRunning.notrunning.error");
                return rmins;
            }

            //用于流程注册事件时的流程实例ID提供
            var processInstance = (new ProcessInstanceManager()).GetById(session.Connection, runningNode.ProcessInstanceID, session.Transaction);
            rmins.ProcessInstanceID = runningNode.ProcessInstanceID;
            var processModel = ProcessModelFactory.Create(processInstance.ProcessGUID, processInstance.Version);
            var activityResource = new ActivityResource(runner,
                runner.NextActivityPerformers,
                runner.Conditions);

            rmins.TaskView = taskView;
            rmins.RunningActivityInstance = runningNode;
            rmins.ProcessModel = processModel;
            rmins.ActivityResource = activityResource;

            return rmins;
        }
        #endregion

        #region WfRuntimeManager 创建跳转运行时对象
        /// <summary>
        /// 创建跳转实例信息
        /// </summary>
        /// <param name="runner">执行者</param>
        /// <param name="result">结果对象</param>
        /// <returns>运行时实例对象</returns>
        public static WfRuntimeManager CreateRuntimeInstanceJump(WfAppRunner runner,
            ref WfExecutedResult result)
        {
            var rmins = new WfRuntimeManagerJump();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            if (string.IsNullOrEmpty(runner.AppName)
               || String.IsNullOrEmpty(runner.AppInstanceID)
               || runner.ProcessGUID == null
               || runner.NextActivityPerformers == null)
            {
                //缺失方法参数
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Jump_ErrorArguments;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceJump.missing.error");
                return rmins;
            }

            //流程跳转时，只能跳转到一个节点
            if (runner.NextActivityPerformers.Count() > 1)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Jump_OverOneStep;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceJump.jump.error",
                    runner.NextActivityPerformers.Count().ToString());
                return rmins;
            }

            //获取当前运行节点信息
            var aim = new ActivityInstanceManager();
            TaskViewEntity taskView = null;
            var runningNode = aim.GetRunningNode(runner, out taskView);

            //传递runner变量
            rmins.TaskView = taskView;
            rmins.AppRunner = runner;
            rmins.AppRunner.AppName = runner.AppName;
            rmins.AppRunner.AppInstanceID = runner.AppInstanceID;
            rmins.AppRunner.ProcessGUID = runner.ProcessGUID;
            rmins.AppRunner.UserID = runner.UserID;
            rmins.AppRunner.UserName = runner.UserName;

            //用于流程注册时间调用时候的流程实例ID提供
            rmins.ProcessInstanceID = runningNode.ProcessInstanceID;

            var processModel = ProcessModelFactory.Create(taskView.ProcessGUID, taskView.Version);
            rmins.ProcessModel = processModel;

            #region 考虑回跳方式
            //获取跳转节点信息
            var jumpBackActivityGUID = runner.NextActivityPerformers.First().Key;
            var jumpBackActivityInstance = aim.GetActivityInstanceLatest(runningNode.ProcessInstanceID, jumpBackActivityGUID);

            if (jumpBackActivityInstance != null)
            {
                //跳转到曾经执行过的节点上,可以作为跳回方式处理
                rmins.IsBackward = true;
                rmins.BackwardContext.ProcessInstance = (new ProcessInstanceManager()).GetById(runningNode.ProcessInstanceID);
                rmins.BackwardContext.BackwardToTaskActivity = processModel.GetActivity(jumpBackActivityGUID);

                //获取当前运行节点的上一步节点
                bool hasGatewayNode = false;
                var tim = new TransitionInstanceManager();
                var lastTaskTransitionInstance = tim.GetLastTaskTransition(runner.AppName,
                    runner.AppInstanceID, runner.ProcessGUID);

                var npc = new PreviousStepChecker();
                var previousActivityInstance = npc.GetPreviousActivityInstanceList(runningNode, true,
                    out hasGatewayNode).ToList()[0];

                //仅仅是回跳到上一步节点，即按SendBack方式处理
                if (previousActivityInstance.ActivityGUID == jumpBackActivityGUID)
                {
                    rmins.BackwardContext.BackwardToTaskActivityInstance = previousActivityInstance;
                    rmins.BackwardContext.BackwardToTargetTransitionGUID =
                        hasGatewayNode == false ? lastTaskTransitionInstance.TransitionGUID : WfDefine.WF_XPDL_GATEWAY_BYPASS_GUID;        //如果中间有Gateway节点，则没有直接相连的TransitonGUID

                    rmins.BackwardContext.BackwardFromActivity = processModel.GetActivity(runningNode.ActivityGUID);
                    rmins.BackwardContext.BackwardFromActivityInstance = runningNode;
                    rmins.BackwardContext.BackwardTaskReceiver = WfBackwardTaskReceiver.Instance(
                        previousActivityInstance.ActivityName,
                        previousActivityInstance.EndedByUserID,
                        previousActivityInstance.EndedByUserName);

                    rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(
                        previousActivityInstance.ActivityGUID,
                        previousActivityInstance.EndedByUserID,
                        previousActivityInstance.EndedByUserName);
                }
                else
                {
                    //回跳到早前节点
                    if (jumpBackActivityInstance.ActivityState != (short)ActivityStateEnum.Completed)
                    {
                        //回跳节点不在完成状态
                        result.Status = WfExecutedStatus.Exception;
                        result.ExceptionType = WfExceptionType.Jump_NotActivityBackCompleted;
                        result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceJump.back.error");

                        return rmins;
                    }

                    rmins.BackwardContext.BackwardToTaskActivityInstance = jumpBackActivityInstance;

                    //判断两个节点是否有Transition的定义存在
                    var transition = processModel.GetForwardTransition(runningNode.ActivityGUID, jumpBackActivityGUID);
                    rmins.BackwardContext.BackwardToTargetTransitionGUID = transition != null ? transition.TransitionGUID : WfDefine.WF_XPDL_GATEWAY_BYPASS_GUID;

                    rmins.BackwardContext.BackwardFromActivity = processModel.GetActivity(runningNode.ActivityGUID);
                    rmins.BackwardContext.BackwardFromActivityInstance = runningNode;
                    rmins.BackwardContext.BackwardTaskReceiver = WfBackwardTaskReceiver.Instance(
                        jumpBackActivityInstance.ActivityName,
                        jumpBackActivityInstance.EndedByUserID,
                        jumpBackActivityInstance.EndedByUserName);

                    rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(
                        jumpBackActivityInstance.ActivityGUID,
                        jumpBackActivityInstance.EndedByUserID,
                        jumpBackActivityInstance.EndedByUserName);
                }
                //获取资源数据
                var activityResourceBack = new ActivityResource(rmins.AppRunner,
                    rmins.AppRunner.NextActivityPerformers,
                    runner.Conditions);
                rmins.ActivityResource = activityResourceBack;
            }
            else
            {
                //跳转到从未执行过的节点上
                var activityResource = new ActivityResource(runner, runner.NextActivityPerformers, runner.Conditions);
                rmins.ActivityResource = activityResource;
                rmins.RunningActivityInstance = runningNode;
            }
            #endregion

            return rmins;
        }
        #endregion

        #region WfRuntimeManager 创建退回运行时对象
        /// <summary>
        /// 退回操作
        /// </summary>
        /// <param name="runner">执行者</param>
        /// <param name="result">结果对象</param>
        /// <returns>运行时实例对象</returns>
        internal static WfRuntimeManager CreateRuntimeInstanceSendBack(WfAppRunner runner,
            ref WfExecutedResult result)
        {
            var rmins = new WfRuntimeManagerSendBack();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            //没有指定退回节点信息
            if (runner.NextPerformerType != NextPerformerIntTypeEnum.Traced
                && (runner.NextActivityPerformers == null || runner.NextActivityPerformers.Count == 0))
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_IsNull;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceSendBack.null.error");

                return rmins;
            }

            //先查找当前用户正在办理的运行节点
            var aim = new ActivityInstanceManager();
            TaskViewEntity taskView = null;
            var runningNode = aim.GetRunningNode(runner, out taskView);

            if (runningNode == null)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_NotInRunning;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceSendBack.nonerun.error");

                return rmins;
            }

            if (aim.IsMineTask(runningNode, runner.UserID) == false)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_NotMineTask;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceSendBack.nonetask.error");
                return rmins;
            }

            var activityType = EnumHelper.ParseEnum<ActivityTypeEnum>(runningNode.ActivityType.ToString());
            if (XPDLHelper.IsSimpleComponentNode(activityType) == false)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_NotTaskNode;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceSendBack.nottask.error");
                return rmins;
            }

            //获取上一步节点信息
            var hasGatewayPassed = false;
            var processInstance = (new ProcessInstanceManager()).GetById(runningNode.ProcessInstanceID);
            var processModel = ProcessModelFactory.Create(processInstance.ProcessGUID, processInstance.Version);
            var previousStepChecker = new PreviousStepChecker();
            var previousActivityList = previousStepChecker.GetPreviousActivityList(runningNode, processModel, out hasGatewayPassed);

            //判断退回是否有效
            if (previousActivityList == null || previousActivityList.Count == 0)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_IsNull;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceSendBack.empty.error");
                return rmins;
            }

            //前端用户指定退回步骤的模式
            if (runner.NextPerformerType != NextPerformerIntTypeEnum.Traced)
            {
                if (runner.NextActivityPerformers == null || runner.NextActivityPerformers.Count == 0)
                {
                    result.Status = WfExecutedStatus.Exception;
                    result.ExceptionType = WfExceptionType.Sendback_IsNull;
                    result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceSendBack.noneperformer.error");

                    return rmins;
                }

                //检查节点是否一致
                if (previousActivityList.Count == 1)
                {
                    var onlyActivityGUID = previousActivityList[0].ActivityGUID;
                    var isOnly = true;
                    foreach (var step in runner.NextActivityPerformers)
                    {
                        if (step.Key != onlyActivityGUID)
                        {
                            isOnly = false;
                            break;
                        }
                    }

                    //存在不一致的退回节点
                    if (isOnly == false)
                    {
                        result.Status = WfExecutedStatus.Exception;
                        result.ExceptionType = WfExceptionType.Sendback_IsTooManyPrevious;
                        result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceSendBack.notunique.error");

                        return rmins;
                    }
                }
            }
            else
            {
                //Traced 用于直接返回上一步使用，测试模式
                var prevActivity = previousActivityList[0];
                var performerList = PerformerBuilder.CreatePerformerList(runningNode.CreatedByUserID, runningNode.CreatedByUserName);
                runner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(prevActivity.ActivityGUID, performerList);
            }

            //创建运行时
            rmins.TaskView = taskView;
            rmins.RunningActivityInstance = runningNode;
            rmins.ProcessInstanceID = runningNode.ProcessInstanceID;
            rmins.ProcessInstance = processInstance;
            rmins.ProcessModel = processModel;

            rmins.AppRunner.AppName = runner.AppName;
            rmins.AppRunner.AppInstanceID = runner.AppInstanceID;
            rmins.AppRunner.UserID = runner.UserID;
            rmins.AppRunner.UserName = runner.UserName;
            rmins.AppRunner.NextActivityPerformers = runner.NextActivityPerformers;

            //设置退回选项类
            var sendbackOperation = new SendBackOperation();
            sendbackOperation.BackwardType = BackwardTypeEnum.Sendback;
            sendbackOperation.ProcessInstance = processInstance;
            sendbackOperation.BackwardFromActivityInstance = runningNode;
            sendbackOperation.HasGatewayPassed = hasGatewayPassed;
            sendbackOperation.ActivityResource = new ActivityResource(runner, rmins.AppRunner.NextActivityPerformers);
            sendbackOperation.ProcessModel = processModel;
            sendbackOperation.IsCancellingBrothersNode = (runner.ControlParameterSheet != null 
                    && runner.ControlParameterSheet.IsCancellingBrothersNode == 1) ? true : false;

            rmins.SendBackOperation = sendbackOperation;

            return rmins;
        }

        /// <summary>
        /// 判断传递的步骤是否在列表中
        /// </summary>
        /// <param name="previousActivityList">步骤列表</param>
        /// <param name="steps">要检查的步骤</param>
        /// <param name="sendbackPreviousActivityList">要退回的节点列表</param>
        /// <returns>是否没有包含</returns>
        private static Boolean IsInvalidStepsInPrevousActivityList(IList<ActivityEntity> previousActivityList, 
            IDictionary<string, PerformerList> steps,
            IList<ActivityEntity> sendbackPreviousActivityList)
        {
            var isInvalid = false;
            foreach (var key in steps.Keys)
            {
                var activity = previousActivityList.Single(s => s.ActivityGUID == key);
                if (activity == null)
                {
                    isInvalid = true;
                    break;
                }
                else if (activity.ActivityType == ActivityTypeEnum.StartNode)
                {
                    isInvalid = true;
                    break;
                }
                else
                {
                    sendbackPreviousActivityList.Add(activity);
                }
            }
            return isInvalid;
        }


        /// <summary>
        /// 过滤前置节点
        /// 流转历史数据
        /// </summary>
        /// <param name="previousActivityList">解析出的前置节点列表</param>
        /// <param name="previousActivityInstanceList">实际流转的前置节点列表</param>
        /// <returns>过滤后的节点列表</returns>
        private static IList<ActivityEntity> FilterPreviousActivityList(IList<ActivityEntity> previousActivityList,
            IList<ActivityInstanceEntity> previousActivityInstanceList)
        {
            IList<ActivityEntity> activityList = new List<ActivityEntity>();
            foreach (var activity in previousActivityList)
            {
                var list = previousActivityInstanceList.Where(a => a.ActivityGUID == activity.ActivityGUID).ToList();
                if (list != null && list.Count > 0)
                {
                    activityList.Add(activity);
                }
            }
            return activityList;
        }
        #endregion

        #region WfRuntimeManager 创建撤销处理的退回运行时对象
        /// <summary>
        /// 创建撤销处理运行时
        /// </summary>
        /// <param name="runner">撤销人</param>
        /// <param name="result">创建结果</param>
        /// <returns>运行时管理器</returns>
        internal static WfRuntimeManager CreateRuntimeInstanceWithdraw(WfAppRunner runner,
           ref WfExecutedResult result)
        {
            var rmins = new WfRuntimeManagerSendBack();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            if (runner.TaskID == null)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Withdraw_ErrorArguments;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceWithdraw.missingtaskid.error");
                return rmins;
            }
            //获取已经完成任务的信息
            var tm = new TaskManager();
            var taskDone = tm.GetTaskView(runner.TaskID.Value);

            if (tm.IsMine(taskDone, runner.UserID) == false)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_NotMineTask;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceWithdraw.nonetask.error");
                return rmins;
            }

            //赋值下一步办理列表
            runner.NextActivityPerformers = NextStepUtility.CreateNextStepPerformerList(taskDone.ActivityGUID,
                taskDone.AssignedToUserID, taskDone.AssignedToUserName);

            //没有指定退回节点信息
            if (runner.NextActivityPerformers == null || runner.NextActivityPerformers.Count == 0)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_IsNull;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceWithdraw.noneperformer.error");

                return rmins;
            }

            //获取待办任务
            var tim = new TransitionInstanceManager();
            var nextStepList = tim.GetTargetActivityInstanceList(taskDone.ActivityInstanceID).ToList();
            
            ActivityInstanceEntity runningNode = nextStepList.Count > 0 ?  nextStepList[0] : null;
            if (runningNode == null)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_NotInRunning;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceWithdraw.nonerun.error");

                return rmins;
            }

            var activityType = EnumHelper.ParseEnum<ActivityTypeEnum>(runningNode.ActivityType.ToString());
            if (XPDLHelper.IsSimpleComponentNode(activityType) == false)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_NotTaskNode;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceWithdraw.nottask.error");
                return rmins;
            }

            //获取待办任务(模拟待办任务用户做退回处理)
            var taskToDo = tm.GetTaskViewByActivity(runningNode.ProcessInstanceID, runningNode.ID);
            runner.UserID = taskToDo.AssignedToUserID;
            runner.UserName = taskToDo.AssignedToUserName;

            //获取上一步节点信息
            var hasGatewayPassed = false;
            var processInstance = (new ProcessInstanceManager()).GetById(runningNode.ProcessInstanceID);
            
            var previousStepChecker = new PreviousStepChecker();
            var processModel = ProcessModelFactory.Create(taskToDo.ProcessGUID, taskToDo.Version);
            var previousActivityList = previousStepChecker.GetPreviousActivityList(runningNode, processModel, out hasGatewayPassed);

            //判断退回是否有效
            if (previousActivityList == null || previousActivityList.Count == 0)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_IsNull;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceWithdraw.empty.error");
                return rmins;
            }

            //检查节点是否一致
            if (previousActivityList.Count == 1)
            {
                var onlyActivityGUID = previousActivityList[0].ActivityGUID;
                var isOnly = true;
                foreach (var step in runner.NextActivityPerformers)
                {
                    if (step.Key != onlyActivityGUID)
                    {
                        isOnly = false;
                        break;
                    }
                }

                //存在不一致的退回节点
                if (isOnly == false)
                {
                    result.Status = WfExecutedStatus.Exception;
                    result.ExceptionType = WfExceptionType.Sendback_IsTooManyPrevious;
                    result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceWithdraw.notunique.error");

                    return rmins;
                }
            }

            //创建运行时
            rmins.TaskView = taskToDo;
            rmins.RunningActivityInstance = runningNode;
            rmins.ProcessInstanceID = runningNode.ProcessInstanceID;
            rmins.ProcessInstance = processInstance;
            rmins.ProcessModel = processModel;

            rmins.AppRunner.AppName = runner.AppName;
            rmins.AppRunner.AppInstanceID = runner.AppInstanceID;
            rmins.AppRunner.UserID = runner.UserID;
            rmins.AppRunner.UserName = runner.UserName;
            rmins.AppRunner.NextActivityPerformers = runner.NextActivityPerformers;

            //设置退回选项类
            var sendbackOperation = new SendBackOperation();
            sendbackOperation.BackwardType = BackwardTypeEnum.Withdrawed;
            sendbackOperation.ProcessInstance = processInstance;
            sendbackOperation.BackwardFromActivityInstance = runningNode;
            sendbackOperation.HasGatewayPassed = hasGatewayPassed;
            sendbackOperation.ActivityResource = new ActivityResource(runner, rmins.AppRunner.NextActivityPerformers);
            sendbackOperation.ProcessModel = processModel;
            sendbackOperation.IsCancellingBrothersNode = true;          //撤销时默认撤销各个并行分支

            rmins.SendBackOperation = sendbackOperation;

            return rmins;
        }
        #endregion

        #region WfRuntimeManager 创建返签运行时对象
        /// <summary>
        /// 流程返签，先检查约束条件，然后调用wfruntimeinstance执行
        /// </summary>
        /// <param name="runner">执行者</param>
        /// <param name="result">结果对象</param>
        /// <returns>运行时实例对象</returns>
        public static WfRuntimeManager CreateRuntimeInstanceReverse(WfAppRunner runner,
            ref WfExecutedResult result)
        {
            var rmins = new WfRuntimeManagerReverse();
            rmins.WfExecutedResult = result = new WfExecutedResult();
            var pim = new ProcessInstanceManager();
            var processInstance = pim.GetProcessInstanceLatest(runner.AppInstanceID, runner.ProcessGUID);
            if (processInstance == null || processInstance.ProcessState != (short)ProcessStateEnum.Completed)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Reverse_NotInCompleted;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceReverse.error");
                return rmins;
            }

            //用于注册事件时候的流程ID
            rmins.ProcessInstanceID = processInstance.ID;

            var tim = new TransitionInstanceManager();
            var endTransitionInstance = tim.GetEndTransition(runner.AppName, runner.AppInstanceID, runner.ProcessGUID);

            var processModel = ProcessModelFactory.Create(processInstance.ProcessGUID, processInstance.Version);
            var endActivity = processModel.GetActivity(endTransitionInstance.ToActivityGUID);

            var aim = new ActivityInstanceManager();
            var endActivityInstance = aim.GetById(endTransitionInstance.ToActivityInstanceID);

            bool hasGatewayNode = false;
            var npc = new PreviousStepChecker();
            var lastTaskActivityInstance = npc.GetPreviousActivityInstanceList(endActivityInstance, false,
                out hasGatewayNode).ToList()[0];
            var lastTaskActivity = processModel.GetActivity(lastTaskActivityInstance.ActivityGUID);

            //封装返签结束点之前办理节点的任务接收人
            rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(lastTaskActivityInstance.ActivityGUID,
                lastTaskActivityInstance.EndedByUserID,
                lastTaskActivityInstance.EndedByUserName);

            rmins.ActivityResource = new ActivityResource(runner, rmins.AppRunner.NextActivityPerformers);
            rmins.AppRunner.AppName = runner.AppName;
            rmins.AppRunner.AppInstanceID = runner.AppInstanceID;
            rmins.AppRunner.ProcessGUID = runner.ProcessGUID;
            rmins.AppRunner.UserID = runner.UserID;
            rmins.AppRunner.UserName = runner.UserName;

            rmins.BackwardContext.ProcessInstance = processInstance;
            rmins.BackwardContext.BackwardToTaskActivity = lastTaskActivity;
            rmins.BackwardContext.BackwardToTaskActivityInstance = lastTaskActivityInstance;
            rmins.BackwardContext.BackwardToTargetTransitionGUID =
                hasGatewayNode == false ? endTransitionInstance.TransitionGUID : String.Empty;
            rmins.BackwardContext.BackwardFromActivity = endActivity;
            rmins.BackwardContext.BackwardFromActivityInstance = endActivityInstance;
            rmins.BackwardContext.BackwardTaskReceiver = WfBackwardTaskReceiver.Instance(lastTaskActivityInstance.ActivityName,
                lastTaskActivityInstance.EndedByUserID,
                lastTaskActivityInstance.EndedByUserName);
            

            return rmins;
        }
        #endregion

        #region WfRuntimeManager 事件注册及注销
        /// <summary>
        /// 事件注册
        /// </summary>
        /// <param name="runtimeInstance">运行时</param>
        /// <param name="executing">执行事件</param>
        /// <param name="executed">完成事件</param>
        internal static void RegisterEvent(WfRuntimeManager runtimeInstance,
            EventHandler<WfEventArgs> executing, 
            EventHandler<WfEventArgs> executed)
        {
            if (runtimeInstance != null)
            {
                runtimeInstance.RegisterEvent(executing, executed);
            }
        }

        /// <summary>
        /// 事件注销
        /// </summary>
        /// <param name="runtimeInstance">运行时</param>
        /// <param name="executing">执行事件</param>
        /// <param name="executed">完成事件</param>
        internal static void UnregisterEvent(WfRuntimeManager runtimeInstance, 
            EventHandler<WfEventArgs> executing, 
            EventHandler<WfEventArgs> executed)
        {
            if (runtimeInstance != null)
            {
                runtimeInstance.UnRegiesterEvent(executing, executed);
            }
        }
        #endregion
    }
}
