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
using System.Threading;
using System.Transactions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Event;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Core.Pattern;

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
                result.Message = "流程已经处于运行状态，如果要重新启动，请先终止当前流程实例！";
                return rmins;
            }

            rmins.AppRunner = runner;

            //获取流程第一个可办理节点
            rmins.ProcessModel = ProcessModelFactory.Create(runner.ProcessGUID, runner.Version);
            var startActivity = rmins.ProcessModel.GetStartActivity();
            var firstActivity = rmins.ProcessModel.GetFirstActivity();

            if (startActivity.ActivityTypeDetail.TriggerType == TriggerTypeEnum.None)
            {
                rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(firstActivity.ActivityGUID,
                    runner.UserID,
                    runner.UserName);
            }
            else if (startActivity.ActivityTypeDetail.TriggerType == TriggerTypeEnum.Timer)
            {
                var roleList = rmins.ProcessModel.GetActivityRoles(firstActivity.ActivityGUID);
                rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(firstActivity.ActivityGUID, roleList);
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
        /// <param name="result">运行结果</param>
        /// <returns>运行时管理器</returns>
        public static WfRuntimeManager CreateRuntimeInstanceStartupSub(WfAppRunner runner,
            ProcessInstanceEntity parentProcessInstance,
            SubProcessNode subProcessNode,
            PerformerList performerList,
            ref WfExecutedResult result)
        {
            //检查流程是否可以被启动
            var rmins = new WfRuntimeManagerStartupSub();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            var pim = new ProcessInstanceManager();
            ProcessInstanceEntity processInstance = pim.GetProcessInstanceCurrent(runner.AppInstanceID,
                    subProcessNode.SubProcessGUID);

            //不能同时启动多个主流程
            if (processInstance != null
                && processInstance.ParentProcessInstanceID == null
                && processInstance.ProcessState == (short)ProcessStateEnum.Running)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Started_IsRunningAlready;
                result.Message = "流程已经处于运行状态，如果要重新启动，请先终止当前流程实例！";
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
        /// <param name="result">结果对象</param>
        /// <returns>运行时实例对象</returns>
        public static WfRuntimeManager CreateRuntimeInstanceAppRunning(
            WfAppRunner runner,
            ref WfExecutedResult result)
        {
            //检查传人参数是否有效
            var rmins = new WfRuntimeManagerAppRunning();
            rmins.WfExecutedResult = result = new WfExecutedResult();
            if (string.IsNullOrEmpty(runner.AppName)
                || String.IsNullOrEmpty(runner.AppInstanceID)
                || runner.ProcessGUID == null)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.RunApp_ErrorArguments;
                result.Message = "方法参数错误，无法运行流程！";
                return rmins;
            }

            //传递runner变量
            rmins.AppRunner = runner;

            var aim = new ActivityInstanceManager();
            TaskViewEntity taskView = null;
            var runningNode = aim.GetRunningNode(runner, out taskView);

            //判断是否是当前登录用户的任务
            if (runningNode.AssignedToUserIDs.Contains(runner.UserID.ToString()) == false)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.RunApp_HasNoTask;
                result.Message = "当前没有登录用户要办理的任务，无法运行流程！";
                return rmins;
            }

            var processModel = ProcessModelFactory.Create(taskView.ProcessGUID, taskView.Version);
            var activityResource = new ActivityResource(runner,
                runner.NextActivityPerformers,
                runner.Conditions,
                runner.DynamicVariables);

            var tm = new TaskManager();
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
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Jump_ErrorArguments;
                result.Message = "方法参数错误，无法运行流程！";
                return rmins;
            }

            //流程跳转时，只能跳转到一个节点
            if (runner.NextActivityPerformers.Count() > 1)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Jump_OverOneStep;
                result.Message = string.Format("不能跳转到多个节点！节点数:{0}",
                    runner.NextActivityPerformers.Count());
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

            var processModel = ProcessModelFactory.Create(taskView.ProcessGUID, taskView.Version);
            rmins.ProcessModel = processModel;

            #region 不考虑回跳方式
            ////获取跳转节点信息
            //var jumpActivityGUID = runner.NextActivityPerformers.First().Key;
            //var jumpActivityInstanceList = aim.GetActivityInstance(runner.AppInstanceID, runner.ProcessGUID, jumpActivityGUID);

            //if (jumpActivityInstanceList != null
            //    && jumpActivityInstanceList.Count > 0)
            //{
            //    //跳转到曾经执行过的节点上,可以作为跳回方式处理
            //    rmins.IsBackward = true;
            //    rmins.BackwardContext.ProcessInstance = (new ProcessInstanceManager()).GetById(runningNode.ProcessInstanceID);
            //    rmins.BackwardContext.BackwardToTaskActivity = processModel.GetActivity(jumpActivityGUID);

            //    //获取当前运行节点的上一步节点
            //    bool hasGatewayNode = false;
            //    var tim = new TransitionInstanceManager();
            //    var lastTaskTransitionInstance = tim.GetLastTaskTransition(runner.AppName,
            //        runner.AppInstanceID, runner.ProcessGUID);
            //    var previousActivityInstance = tim.GetPreviousActivityInstance(runningNode, true,
            //        out hasGatewayNode).ToList()[0];

            //    //仅仅是回跳到上一步节点，即按SendBack方式处理
            //    if (previousActivityInstance.ActivityGUID == jumpActivityGUID)
            //    {
            //        rmins.BackwardContext.BackwardToTaskActivityInstance = previousActivityInstance;
            //        rmins.BackwardContext.BackwardToTargetTransitionGUID =
            //            hasGatewayNode == false ? lastTaskTransitionInstance.TransitionGUID : System.Guid.Empty;        //如果中间有Gateway节点，则没有直接相连的TransitonGUID

            //        rmins.BackwardContext.BackwardFromActivity = processModel.GetActivity(runningNode.ActivityGUID);
            //        rmins.BackwardContext.BackwardFromActivityInstance = runningNode;
            //        rmins.BackwardContext.BackwardTaskReciever = WfBackwardTaskReciever.Instance(
            //            previousActivityInstance.ActivityName,
            //            previousActivityInstance.EndedByUserID.Value,
            //            previousActivityInstance.EndedByUserName);

            //        rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(
            //            previousActivityInstance.ActivityGUID,
            //            previousActivityInstance.EndedByUserID.Value,
            //            previousActivityInstance.EndedByUserName);
            //    }
            //    else
            //    {
            //        //回跳到早前节点
            //        var jumptoActivityInstance = jumpActivityInstanceList[0];
            //        if (jumptoActivityInstance.ActivityState != (short)ActivityStateEnum.Completed)
            //        {
            //            result.Status = WfExecutedStatus.Exception;
            //            result.Exception = WfJumpException.NotActivityBackCompleted;
            //            result.Message = string.Format("回跳到的节点不在完成状态，无法重新回跳！");

            //            return rmins;
            //        }

            //        rmins.BackwardContext.BackwardToTaskActivityInstance = jumptoActivityInstance;

            //        //判断两个节点是否有Transition的定义存在
            //        var transition = processModel.GetForwardTransition(runningNode.ActivityGUID, runner.JumpbackActivityGUID.Value);
            //        rmins.BackwardContext.BackwardToTargetTransitionGUID = transition != null ? transition.TransitionGUID : System.Guid.Empty;

            //        rmins.BackwardContext.BackwardFromActivity = processModel.GetActivity(runningNode.ActivityGUID);
            //        rmins.BackwardContext.BackwardFromActivityInstance = runningNode;
            //        rmins.BackwardContext.BackwardTaskReciever = WfBackwardTaskReciever.Instance(
            //            jumptoActivityInstance.ActivityName,
            //            jumptoActivityInstance.EndedByUserID.Value,
            //            jumptoActivityInstance.EndedByUserName);

            //        rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(
            //            jumptoActivityInstance.ActivityGUID,
            //            jumptoActivityInstance.EndedByUserID.Value,
            //            jumptoActivityInstance.EndedByUserName);
            //    }
            //    //获取资源数据
            //    var activityResourceBack = new ActivityResource(rmins.AppRunner, 
            //        rmins.AppRunner.NextActivityPerformers, 
            //        runner.Conditions);
            //    rmins.ActivityResource = activityResourceBack;
            //}
            //else
            //{
            //    //跳转到从未执行过的节点上
            //    var activityResource = new ActivityResource(runner, runner.NextActivityPerformers, runner.Conditions);
            //    rmins.ActivityResource = activityResource;
            //    rmins.RunningActivityInstance = runningNode;
            //}
            #endregion

            //跳转到从未执行过的节点上
            var activityResource = new ActivityResource(runner, runner.NextActivityPerformers, runner.Conditions);
            rmins.ActivityResource = activityResource;
            rmins.RunningActivityInstance = runningNode;

            return rmins;
        }
        #endregion

        #region WfRuntimeManager 创建撤销运行时对象
        /// <summary>
        /// 撤销操作
        /// 包括：
        /// 1) 正常流转
        /// 2) 多实例节点流转
        /// </summary>
        /// <param name="runner">执行者</param>
        /// <param name="result">结果对象</param>
        /// <returns>运行时实例对象</returns>
        internal static WfRuntimeManager CreateRuntimeInstanceWithdraw(WfAppRunner runner,
            ref WfExecutedResult result)
        {
            WfRuntimeManager rmins = new WfRuntimeManagerWithdraw();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            var aim = new ActivityInstanceManager();
            var runningActivityInstanceList = aim.GetRunningActivityInstanceList(runner.AppInstanceID, runner.ProcessGUID).ToList();

            WithdrawOperationTypeEnum withdrawOperation = WithdrawOperationTypeEnum.Default;

            //当前没有运行状态的节点存在，流程不存在，或者已经结束或取消
            if (runningActivityInstanceList == null || runningActivityInstanceList.Count() == 0)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Withdraw_NotInReady;
                result.Message = "当前没有运行状态的节点存在，流程不存在，或者已经结束或取消";

                return rmins;
            }

            if (runningActivityInstanceList.Count() == 1)      //如果只有1个运行节点
            {
                //先判断节点的状态是否是有效状态
                var runningNode = runningActivityInstanceList[0];
                if (runningNode.ActivityState != (short)ActivityStateEnum.Ready
                    && runningNode.ActivityState != (short)ActivityStateEnum.Suspended)        //只有准备或挂起状态的节点可以撤销
                {
                    result.Status = WfExecutedStatus.Exception;
                    result.ExceptionType = WfExceptionType.Withdraw_NotInReady;
                    result.Message = string.Format("无法撤销到上一步，因为要撤销的节点为空，或不在【待办/挂起】状态，当前状态: {0}",
                        runningNode.ActivityState);//，节点状态：{0}    runningNode.ActivityState     为空报错20150514

                    return rmins;
                }

                //判断是否是多实例节点的子节点
                if (runningNode.MIHostActivityInstanceID != null)
                {
                    if (runningNode.CompleteOrder == 1)
                    {
                        //只有串行模式下有CompleteOrder的值为 1
                        //串行模式多实例的第一个执行节点，此时上一步骤可以撤销
                        withdrawOperation = WithdrawOperationTypeEnum.MISFirstOneIsRunning;
                    }
                    else if (runningNode.CompleteOrder > 1)
                    {
                        //串行模式多实例内部撤销，其中只有1个节点处于运行状态
                        withdrawOperation = WithdrawOperationTypeEnum.MISOneIsRunning;
                    }
                    else if (runningNode.CompleteOrder == -1)
                    {
                        //并行模式下CompleteOrder的值为 -1，此时只剩余最后一个
                        //要撤销的话，是对并行会签节点的内部撤销，即认为是重新办理，找到当前运行人的节点，置状态为待办状态就可以
                        withdrawOperation = WithdrawOperationTypeEnum.MIPSeveralIsRunning;
                    }
                }
                else
                {
                    //当前运行节点是普通节点模式
                    withdrawOperation = WithdrawOperationTypeEnum.Normal;
                }
            }
            else if (runningActivityInstanceList.Count() > 1)       //有多个并行运行节点存在
            {
                //判断多实例主节点下的子节点是否都处于待办状态，如果是，则上一步可以撤销回去，否则不可以撤销
                var firstActivityInstance = runningActivityInstanceList[0];
                if (firstActivityInstance.MIHostActivityInstanceID != null)
                {
                    bool isAllInReadyState = true;
                    var allChildNodeList = aim.GetActivityMultipleInstance(firstActivityInstance.MIHostActivityInstanceID.Value,
                        firstActivityInstance.ProcessInstanceID);

                    foreach (var ai in allChildNodeList)
                    {
                        if (ai.ActivityState != (short)ActivityStateEnum.Ready
                            && ai.ActivityState != (short)ActivityStateEnum.Suspended)
                        {
                            isAllInReadyState = false;
                            break;
                        }
                    }

                    if (isAllInReadyState == true)
                    {
                        //子节点全部处于待办状态
                        withdrawOperation = WithdrawOperationTypeEnum.MIPAllIsInReadyState;
                    }
                    else
                    {
                        //部分子节点有完成的
                        withdrawOperation = WithdrawOperationTypeEnum.MIPSeveralIsRunning;
                    }
                }
                else
                {
                    //有其它非多实例的并行节点，暂时不处理，后期实现该功能
                    result.Status = WfExecutedStatus.Exception;
                    result.ExceptionType = WfExceptionType.Withdraw_HasTooMany;
                    result.Message = "有多个可以撤销回去的节点，而且不是多实例节点，此功能暂时不支持！";

                    return rmins;
                }
            }

            //根据不同分支场景，创建不同撤销运行时管理器
            return CreateRuntimeInstanceWithdrawByCase(runningActivityInstanceList, withdrawOperation, runner, ref result);
        }

        /// <summary>
        /// 根据不同撤销场景创建运行时管理器
        /// </summary>
        /// <param name="runningActivityInstanceList">运行节点列表</param>
        /// <param name="withdrawOperation">撤销类型</param>
        /// <param name="runner">执行者</param>
        /// <param name="result">结果对象</param>
        /// <returns>运行时管理器</returns>
        private static WfRuntimeManager CreateRuntimeInstanceWithdrawByCase(
            List<ActivityInstanceEntity> runningActivityInstanceList,
            WithdrawOperationTypeEnum withdrawOperation,
            WfAppRunner runner,
            ref WfExecutedResult result)
        {
            WfRuntimeManager rmins = new WfRuntimeManagerWithdraw();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            //根据当前运行节点获取
            ActivityInstanceEntity runningNode = runningActivityInstanceList[0];
            ProcessInstanceEntity processInstance = (new ProcessInstanceManager()).GetById(runningNode.ProcessInstanceID);
            IProcessModel processModel = ProcessModelFactory.Create(processInstance.ProcessGUID, processInstance.Version);

            //不同撤销的分支场景处理
            var aim = new ActivityInstanceManager();

            //以下处理，需要知道上一步是独立节点的信息
            //获取上一步流转节点信息，可能经过And, Or等路由节点
            var tim = new TransitionInstanceManager();
            bool hasGatewayNode = false;
            var currentNode = runningNode;

            if (runningNode.MIHostActivityInstanceID != null)
            {
                //如果当前运行节点是多实例子节点，则需要找到它的主节点的Transiton记录
                currentNode = aim.GetById(runningNode.MIHostActivityInstanceID.Value);
            }
            var lastActivityInstanceList = tim.GetPreviousActivityInstance(currentNode, false, out hasGatewayNode).ToList();

            if (lastActivityInstanceList == null || lastActivityInstanceList.Count > 1)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Withdraw_HasTooMany;
                result.Message = "当前没有可以撤销回去的节点，或者有多个可以撤销回去的节点，无法选择！";

                return rmins;
            }

            TransitionInstanceEntity lastTaskTransitionInstance = null;
            if (hasGatewayNode == false)
            {
                lastTaskTransitionInstance = tim.GetLastTaskTransition(runner.AppName,
                    runner.AppInstanceID, runner.ProcessGUID);
                if (lastTaskTransitionInstance.TransitionType == (short)TransitionTypeEnum.Loop)
                {
                    result.Status = WfExecutedStatus.Exception;
                    result.ExceptionType = WfExceptionType.Withdraw_IsLoopNode;
                    result.Message = "当前流转是自循环，无需撤销！";

                    return rmins;
                }
            }

            var previousActivityInstance = lastActivityInstanceList[0];
            if (previousActivityInstance.EndedByUserID != runner.UserID)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Withdraw_NotCreatedByMine;
                result.Message = string.Format("上一步节点的任务办理人跟当前登录用户不一致，无法撤销回上一步！节点办理人：{0}",
                    previousActivityInstance.EndedByUserName);

                return rmins;
            }

            if (previousActivityInstance.ActivityType == (short)ActivityTypeEnum.EndNode)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Withdraw_PreviousIsEndNode;
                result.Message = "上一步是结束节点，无法撤销！";

                return rmins;
            }

            //当前运行节点是普通节点
            if (withdrawOperation == WithdrawOperationTypeEnum.Normal)
            {
                //简单串行模式下的退回
                rmins = new WfRuntimeManagerWithdraw();
                rmins.WfExecutedResult = result = new WfExecutedResult();

                rmins.ProcessModel = processModel;
                rmins.AppRunner.ProcessGUID = runner.ProcessGUID;
                rmins.BackwardContext.ProcessInstance = processInstance;
                rmins.BackwardContext.BackwardToTargetTransitionGUID =
                    hasGatewayNode == false ? lastTaskTransitionInstance.TransitionGUID : String.Empty;
                rmins.BackwardContext.BackwardToTaskActivity = processModel.GetActivity(previousActivityInstance.ActivityGUID);
                rmins.BackwardContext.BackwardToTaskActivityInstance = previousActivityInstance;
                rmins.BackwardContext.BackwardFromActivity = processModel.GetActivity(runningNode.ActivityGUID);
                rmins.BackwardContext.BackwardFromActivityInstance = runningNode; //准备状态的接收节点
                rmins.BackwardContext.BackwardTaskReceiver = WfBackwardTaskReceiver.Instance(
                    previousActivityInstance.ActivityName,
                    previousActivityInstance.EndedByUserID,
                    previousActivityInstance.EndedByUserName);

                //封装AppUser对象
                rmins.AppRunner.AppName = runner.AppName;
                rmins.AppRunner.AppInstanceID = runner.AppInstanceID;
                rmins.AppRunner.UserID = runner.UserID;
                rmins.AppRunner.UserName = runner.UserName;
                rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(
                    previousActivityInstance.ActivityGUID,
                    runner.UserID,
                    runner.UserName);
                rmins.ActivityResource = new ActivityResource(runner, rmins.AppRunner.NextActivityPerformers);

                return rmins;
            }

            //如果有其它模式，没有处理到，则直接抛出异常
            throw new WorkflowException("未知的撤销场景，请报告给技术支持人员！");
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
            WfRuntimeManager rmins = new WfRuntimeManagerSendBack();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            var aim = new ActivityInstanceManager();
            var runningActivityInstanceList = aim.GetRunningActivityInstanceList(runner.AppInstanceID, runner.ProcessGUID).ToList();

            SendbackOperationTypeEnum sendbackOperation = SendbackOperationTypeEnum.Default;

            //当前没有运行状态的节点存在，流程不存在，或者已经结束或取消
            if (runningActivityInstanceList == null || runningActivityInstanceList.Count() == 0)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_NotInRunning;
                result.Message = "当前没有运行状态的节点存在，流程不存在，或者已经结束或取消";

                return rmins;
            }

            if (runningActivityInstanceList.Count() == 1)       //如果只有1个运行节点
            {
                var runningNode = runningActivityInstanceList[0];

                var activityType = EnumHelper.ParseEnum<ActivityTypeEnum>(runningNode.ActivityType.ToString());
                if (XPDLHelper.IsSimpleComponentNode(activityType) == false)
                {
                    result.Status = WfExecutedStatus.Exception;
                    result.ExceptionType = WfExceptionType.Sendback_NotTaskNode;
                    result.Message = "当前节点不是任务类型的节点，无法退回上一步节点！";
                    return rmins;
                }

                if (aim.IsMineTask(runningNode, runner.UserID) == false)
                {
                    result.Status = WfExecutedStatus.Exception;
                    result.ExceptionType = WfExceptionType.Sendback_NotMineTask;
                    result.Message = "不是登录用户的任务，无法退回！";
                    return rmins;
                }

                //判断当前节点是否是多实例节点
                if (runningNode.MIHostActivityInstanceID != null)
                {
                    if (runningNode.CompleteOrder == 1)
                    {
                        //只有串行模式下有CompleteOrder的值为 1
                        //串行模式多实例的第一个执行节点，此时可退回到上一步
                        sendbackOperation = SendbackOperationTypeEnum.MISFirstOneIsRunning;
                    }
                    else if (runningNode.CompleteOrder > 1)
                    {
                        //已经是中间节点，只能退回到上一步多实例子节点
                        sendbackOperation = SendbackOperationTypeEnum.MISOneIsRunning;
                    }
                    else if (runningNode.CompleteOrder == -1)
                    {
                        sendbackOperation = SendbackOperationTypeEnum.MIPOneIsRunning;
                    }
                }
                else
                {
                    sendbackOperation = SendbackOperationTypeEnum.Normal;
                }
            }
            else if (runningActivityInstanceList.Count() > 1)
            {
                var firstActivityInstance = runningActivityInstanceList[0];
                if (firstActivityInstance.MIHostActivityInstanceID != null)
                {
                    //判断多实例主节点下的子节点是否都处于待办状态，如果是，则可以退回到上一步
                    bool isAllInReadyState = true;
                    var allChildNodeList = aim.GetActivityMultipleInstance(firstActivityInstance.MIHostActivityInstanceID.Value,
                        firstActivityInstance.ProcessInstanceID);

                    int runningCount = 0;
                    foreach (var ai in allChildNodeList)
                    {

                        if (ai.ActivityState != (short)ActivityStateEnum.Ready
                            && ai.ActivityState != (short)ActivityStateEnum.Suspended)
                        {
                            if (ai.ActivityState == (short)ActivityStateEnum.Running)
                                runningCount++;
                            else
                            {
                                isAllInReadyState = false;
                                break;
                            }
                        }
                    }
                    if (runningCount > 1)
                        isAllInReadyState = false;

                    if (isAllInReadyState == false)
                    {
                        //部分子节点有完成的
                        sendbackOperation = SendbackOperationTypeEnum.MIPSeveralIsRunning;
                    }
                    else if (runningCount == 1)
                    {
                        if (firstActivityInstance.CompleteOrder == -1)
                            sendbackOperation = SendbackOperationTypeEnum.MIPOneIsRunning;
                        else
                            //第一个子节点处于待办状态
                            sendbackOperation = SendbackOperationTypeEnum.MISFirstOneIsRunning;
                    }
                    else if (isAllInReadyState)
                    {
                        //子节点全部处于待办状态
                        sendbackOperation = SendbackOperationTypeEnum.MIPAllIsInReadyState;
                    }
                }
                else
                {
                    //有其它非多实例的并行节点，暂时不处理，后期实现该功能
                    result.Status = WfExecutedStatus.Exception;
                    result.ExceptionType = WfExceptionType.Sendback_NullOrHasTooMany;
                    result.Message = "有多个可以退回的节点，而且不是多实例节点，此功能暂时不支持！";

                    return rmins;
                }
            }

            //根据不同分支场景，创建不同撤销运行时管理器
            return CreateRuntimeInstanceSendbackByCase(runningActivityInstanceList, sendbackOperation, runner, ref result);
        }

        /// <summary>
        /// 根据不同退回场景创建运行时管理器
        /// </summary>
        /// <param name="runningActivityInstanceList">运行节点列表</param>
        /// <param name="sendbackOperation">退回类型</param>
        /// <param name="runner">执行者</param>
        /// <param name="result">结果对象</param>
        /// <returns>运行时管理器</returns>
        private static WfRuntimeManager CreateRuntimeInstanceSendbackByCase(
            List<ActivityInstanceEntity> runningActivityInstanceList,
            SendbackOperationTypeEnum sendbackOperation,
            WfAppRunner runner,
            ref WfExecutedResult result)
        {
            WfRuntimeManager rmins = new WfRuntimeManagerSendBack();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            ActivityInstanceEntity runningNode = runningActivityInstanceList.Where(x => x.ActivityState == (int)ActivityStateEnum.Running).OrderBy(x => x.ID).FirstOrDefault();
            if (runningNode == null)
                runningNode = runningActivityInstanceList[0];
            ProcessInstanceEntity processInstance = (new ProcessInstanceManager()).GetById(runningNode.ProcessInstanceID);
            IProcessModel processModel = ProcessModelFactory.Create(processInstance.ProcessGUID, processInstance.Version);

            var aim = new ActivityInstanceManager();

            //以下处理，需要知道上一步是独立节点的信息
            //获取上一步流转节点信息，可能经过And, Or等路由节点
            var tim = new TransitionInstanceManager();
            bool hasGatewayNode = false;
            var currentNode = runningNode;

            var lastActivityInstanceList = tim.GetPreviousActivityInstance(currentNode, true, out hasGatewayNode).ToList();

            if (lastActivityInstanceList == null || lastActivityInstanceList.Count == 0 || lastActivityInstanceList.Count > 1)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_NullOrHasTooMany;
                result.Message = "当前没有可以退回的节点，或者有多个可以退回的节点，无法选择！";

                return rmins;
            }

            TransitionInstanceEntity lastTaskTransitionInstance = tim.GetLastTaskTransition(runner.AppName,
                runner.AppInstanceID, runner.ProcessGUID);

            if (lastTaskTransitionInstance.TransitionType == (short)TransitionTypeEnum.Loop)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_IsLoopNode;
                result.Message = "当前流转是自循环，无需退回！";

                return rmins;
            }

            //设置退回节点的相关信息
            var previousActivityInstance = lastActivityInstanceList[0];

            if (previousActivityInstance.ActivityType == (short)ActivityTypeEnum.StartNode)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_PreviousIsStartNode;
                result.Message = "上一步是开始节点，无需退回！";

                return rmins;
            }

            if (sendbackOperation == SendbackOperationTypeEnum.Normal)
            {
                //简单串行模式下的退回
                rmins = new WfRuntimeManagerSendBack();
                rmins.WfExecutedResult = result = new WfExecutedResult();

                rmins.ProcessModel = processModel;
                rmins.BackwardContext.ProcessInstance = processInstance;
                rmins.BackwardContext.BackwardToTaskActivity = processModel.GetActivity(previousActivityInstance.ActivityGUID);
                rmins.BackwardContext.BackwardToTaskActivityInstance = previousActivityInstance;
                rmins.BackwardContext.BackwardToTargetTransitionGUID =
                    hasGatewayNode == false ? lastTaskTransitionInstance.TransitionGUID : String.Empty;        //如果中间有Gateway节点，则没有直接相连的TransitonGUID

                rmins.BackwardContext.BackwardFromActivity = processModel.GetActivity(runningNode.ActivityGUID);
                rmins.BackwardContext.BackwardFromActivityInstance = runningNode;
                rmins.BackwardContext.BackwardTaskReceiver = WfBackwardTaskReceiver.Instance(previousActivityInstance.ActivityName,
                    previousActivityInstance.EndedByUserID, previousActivityInstance.EndedByUserName);

                //封装AppUser对象
                rmins.AppRunner.AppName = runner.AppName;
                rmins.AppRunner.AppInstanceID = runner.AppInstanceID;
                rmins.AppRunner.ProcessGUID = runner.ProcessGUID;
                rmins.AppRunner.UserID = runner.UserID;
                rmins.AppRunner.UserName = runner.UserName;
                rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(previousActivityInstance.ActivityGUID,
                    previousActivityInstance.EndedByUserID,
                    previousActivityInstance.EndedByUserName);
                rmins.ActivityResource = new ActivityResource(runner, rmins.AppRunner.NextActivityPerformers);

                return rmins;
            }

            //如果有其它模式，没有处理到，则直接抛出异常
            throw new WorkflowException("未知的退回场景，请报告给技术支持人员！");
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
                result.Message = string.Format("当前应用:{0}，实例ID：{1}, 没有完成的流程实例，无法让流程重新运行！",
                    runner.AppName, runner.AppInstanceID);
                return rmins;
            }

            var tim = new TransitionInstanceManager();
            var endTransitionInstance = tim.GetEndTransition(runner.AppName, runner.AppInstanceID, runner.ProcessGUID);

            var processModel = ProcessModelFactory.Create(processInstance.ProcessGUID, processInstance.Version);
            var endActivity = processModel.GetActivity(endTransitionInstance.ToActivityGUID);

            var aim = new ActivityInstanceManager();
            var endActivityInstance = aim.GetById(endTransitionInstance.ToActivityInstanceID);

            bool hasGatewayNode = false;
            var lastTaskActivityInstance = tim.GetPreviousActivityInstance(endActivityInstance, false,
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
    }
}
