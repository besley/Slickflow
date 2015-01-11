/*
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
using System.Data.Linq;
using System.Transactions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Data;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Event;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Core.Pattern;

namespace Slickflow.Engine.Core
{
    /// <summary>
    /// 运行时的创建类
    /// </summary>
    internal class WfRuntimeManagerFactory
    {
        #region WfRuntimeManager 静态方法：创建执行实例的运行者对象
        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="user"></param>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="nextActivityGUID"></param>
        /// <returns></returns>
        public static WfRuntimeManager CreateRuntimeInstanceStartup(WfAppRunner runner,
            ref WfExecutedResult result)
        {
            return CreateRuntimeInstanceStartup(runner, null, null, ref result);
        }

        public static WfRuntimeManager CreateRuntimeInstanceStartup(WfAppRunner runner,
            ProcessInstanceEntity parentProcessInstance,
            SubProcessNode subProcessNode,
            ref WfExecutedResult result)
        {
            //检查流程是否可以被启动
            var rmins = new WfRuntimeManagerStartup();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            var pim = new ProcessInstanceManager();
            ProcessInstanceEntity processInstance = null;

            if (subProcessNode == null)
            {
                //正常流程启动
                processInstance = pim.GetProcessInstanceLatest(runner.AppName,
                    runner.AppInstanceID,
                    runner.ProcessGUID);
            }
            else
            {
                //子流程启动
                processInstance = pim.GetProcessInstanceLatest(runner.AppName,
                    runner.AppInstanceID,
                    subProcessNode.SubProcessGUID);
            }

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
            rmins.ParentProcessInstance = parentProcessInstance;
            rmins.InvokedSubProcessNode = subProcessNode;

            //获取流程第一个可办理节点
            rmins.ProcessModel = new ProcessModel(runner.ProcessGUID);
            var firstActivity = rmins.ProcessModel.GetFirstActivity();

            rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(firstActivity.ActivityGUID,
                runner.UserID,
                runner.UserName);

            rmins.ActivityResource = new ActivityResource(runner, rmins.AppRunner.NextActivityPerformers);

            return rmins;
        }

        /// <summary>
        /// 创建运行时实例
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <param name="nextActivityPerformers"></param>
        /// <returns></returns>
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
            var runningNode = aim.GetRunningNodeOfMine(runner, out taskView);

            //判断是否是当前登录用户的任务
            if (runningNode.AssignedToUsers.Contains(runner.UserID.ToString()) == false)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.RunApp_HasNoTask;
                result.Message = "当前没有登录用户要办理的任务，无法运行流程！";
                return rmins;
            }

            var processModel = new ProcessModel(runningNode.ProcessGUID);
            var activityResource = new ActivityResource(runner, runner.NextActivityPerformers, runner.Conditions);

            var tm = new TaskManager();
            rmins.TaskView = taskView;
            rmins.RunningActivityInstance = runningNode;
            rmins.ProcessModel = processModel;
            rmins.ActivityResource = activityResource;

            return rmins;
        }

        /// <summary>
        /// 创建跳转实例信息
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="result"></param>
        /// <returns></returns>
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
            var runningNode = aim.GetRunningNodeOfMine(runner, out taskView);

            //传递runner变量
            rmins.AppRunner = runner;
            rmins.AppRunner.AppName = runner.AppName;
            rmins.AppRunner.AppInstanceID = runner.AppInstanceID;
            rmins.AppRunner.ProcessGUID = runner.ProcessGUID;
            rmins.AppRunner.UserID = runner.UserID;
            rmins.AppRunner.UserName = runner.UserName;

            var processModel = (new ProcessModel(runner.ProcessGUID));
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

        /// <summary>
        /// 撤销操作
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static WfRuntimeManager CreateRuntimeInstanceWithdraw(WfAppRunner runner,
            ref WfExecutedResult result)
        {
            //获取当前运行节点信息
            var rmins = new WfRuntimeManagerWithdraw();
            rmins.WfExecutedResult = result = new WfExecutedResult();
            var aim = new ActivityInstanceManager();
            var runningNode = aim.GetRunningNodeOfMine(runner);

            if ((runningNode == null) || (runningNode.ActivityState != (short)ActivityStateEnum.Ready))
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Withdraw_NotInReady;
                result.Message = string.Format("要撤销的节点不在【待办】状态，已经无法撤销到上一步，节点状态：{0}",
                   runningNode.ActivityState);
            }

            //获取上一步流转节点信息，可能经过And, Or等路由节点
            var tim = new TransitionInstanceManager();
            bool hasGatewayNode = false;
            var lastActivityInstanceList = tim.GetPreviousActivityInstance(runningNode, false, out hasGatewayNode).ToList();

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
            }

            var withdrawActivityInstance = lastActivityInstanceList[0];
            if (withdrawActivityInstance.EndedByUserID != runner.UserID)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Withdraw_NotCreatedByMine;
                result.Message = string.Format("上一步节点的任务办理人跟当前登录用户不一致，无法撤销回上一步！节点办理人：{0}",
                    withdrawActivityInstance.EndedByUserName);

                return rmins;
            }

            if (withdrawActivityInstance.ActivityType == (short)ActivityTypeEnum.EndNode)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Withdraw_PreviousIsEndNode;
                result.Message = "上一步是结束节点，无法撤销！";

                return rmins;
            }

            //准备撤销节点的相关信息
            var processModel = (new ProcessModel(runner.ProcessGUID));
            rmins.ProcessModel = processModel;
            rmins.AppRunner.ProcessGUID = runner.ProcessGUID;
            rmins.BackwardContext.ProcessInstance = (new ProcessInstanceManager()).GetById(runningNode.ProcessInstanceID);
            rmins.BackwardContext.BackwardToTargetTransitionGUID =
                hasGatewayNode == false ? lastTaskTransitionInstance.TransitionGUID : String.Empty;
            rmins.BackwardContext.BackwardToTaskActivity = processModel.GetActivity(withdrawActivityInstance.ActivityGUID);
            rmins.BackwardContext.BackwardToTaskActivityInstance = withdrawActivityInstance;
            rmins.BackwardContext.BackwardFromActivity = processModel.GetActivity(runningNode.ActivityGUID);
            rmins.BackwardContext.BackwardFromActivityInstance = runningNode; //准备状态的接收节点
            rmins.BackwardContext.BackwardTaskReciever = WfBackwardTaskReciever.Instance(
                withdrawActivityInstance.ActivityName,
                withdrawActivityInstance.EndedByUserID,
                withdrawActivityInstance.EndedByUserName);

            //封装AppUser对象
            rmins.AppRunner.AppName = runner.AppName;
            rmins.AppRunner.AppInstanceID = runner.AppInstanceID;
            rmins.AppRunner.UserID = runner.UserID;
            rmins.AppRunner.UserName = runner.UserName;
            rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(
                withdrawActivityInstance.ActivityGUID,
                runner.UserID,
                runner.UserName);
            rmins.ActivityResource = new ActivityResource(runner, rmins.AppRunner.NextActivityPerformers);

            return rmins;
        }

        /// <summary>
        /// 退回操作
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static WfRuntimeManager CreateRuntimeInstanceSendBack(WfAppRunner runner,
            ref WfExecutedResult result)
        {
            //检查当前运行节点信息
            var rmins = new WfRuntimeManagerSendBack();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            var aim = new ActivityInstanceManager();
            var runningNode = aim.GetRunningNodeOfMine(runner);
            if (runningNode.ActivityType != (short)ActivityTypeEnum.TaskNode)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_NotTaskNode;
                result.Message = "当前节点不是任务节点，无法退回上一步节点！";
                return rmins;
            }

            if (!(runningNode.ActivityState == (short)ActivityStateEnum.Ready
                || runningNode.ActivityState == (short)ActivityStateEnum.Running))
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_NotInRunning;
                result.Message = string.Format("当前节点的状态不在运行状态，无法退回上一步节点！当前节点状态：{0}",
                    runningNode.ActivityState);
                return rmins;
            }

            if (aim.IsMineTask(runningNode, runner.UserID) == false)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_NotMineTask;
                result.Message = "不是登录用户的任务，无法退回！";
                return rmins;
            }

            var tim = new TransitionInstanceManager();
            var lastTaskTransitionInstance = tim.GetLastTaskTransition(runner.AppName,
                runner.AppInstanceID, runner.ProcessGUID);

            if (lastTaskTransitionInstance.TransitionType == (short)TransitionTypeEnum.Loop)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_IsLoopNode;
                result.Message = "当前流转是自循环，无需退回！";
                return rmins;
            }

            //设置退回节点的相关信息
            bool hasGatewayNode = false;
            var sendbackToActivityInstance = tim.GetPreviousActivityInstance(runningNode, true,
                out hasGatewayNode).ToList()[0];

            if (sendbackToActivityInstance.ActivityType == (short)ActivityTypeEnum.StartNode)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_PreviousIsStartNode;
                result.Message = "上一步是开始节点，无需退回！";
                return rmins;
            }

            var processModel = (new ProcessModel(runner.ProcessGUID));
            rmins.ProcessModel = processModel;
            rmins.BackwardContext.ProcessInstance = (new ProcessInstanceManager()).GetById(lastTaskTransitionInstance.ProcessInstanceID);
            rmins.BackwardContext.BackwardToTaskActivity = processModel.GetActivity(sendbackToActivityInstance.ActivityGUID);
            rmins.BackwardContext.BackwardToTaskActivityInstance = sendbackToActivityInstance;
            rmins.BackwardContext.BackwardToTargetTransitionGUID =
                hasGatewayNode == false ? lastTaskTransitionInstance.TransitionGUID : String.Empty;        //如果中间有Gateway节点，则没有直接相连的TransitonGUID

            rmins.BackwardContext.BackwardFromActivity = processModel.GetActivity(runningNode.ActivityGUID);
            rmins.BackwardContext.BackwardFromActivityInstance = runningNode;
            rmins.BackwardContext.BackwardTaskReciever = WfBackwardTaskReciever.Instance(sendbackToActivityInstance.ActivityName,
                sendbackToActivityInstance.EndedByUserID, sendbackToActivityInstance.EndedByUserName);

            //封装AppUser对象
            rmins.AppRunner.AppName = runner.AppName;
            rmins.AppRunner.AppInstanceID = runner.AppInstanceID;
            rmins.AppRunner.ProcessGUID = runner.ProcessGUID;
            rmins.AppRunner.UserID = runner.UserID;
            rmins.AppRunner.UserName = runner.UserName;
            rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(sendbackToActivityInstance.ActivityGUID,
                sendbackToActivityInstance.EndedByUserID,
                sendbackToActivityInstance.EndedByUserName);
            rmins.ActivityResource = new ActivityResource(runner, rmins.AppRunner.NextActivityPerformers);

            return rmins;
        }

        /// <summary>
        /// 流程返签，先检查约束条件，然后调用wfruntimeinstance执行
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static WfRuntimeManager CreateRuntimeInstanceReverse(WfAppRunner runner,
            ref WfExecutedResult result)
        {
            var rmins = new WfRuntimeManagerReverse();
            rmins.WfExecutedResult = result = new WfExecutedResult();
            var pim = new ProcessInstanceManager();
            var processInstance = pim.GetProcessInstanceLatest(runner.AppName, runner.AppInstanceID, runner.ProcessGUID);
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

            var processModel = new ProcessModel(runner.ProcessGUID);
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
            rmins.BackwardContext.BackwardTaskReciever = WfBackwardTaskReciever.Instance(lastTaskActivityInstance.ActivityName,
                lastTaskActivityInstance.EndedByUserID,
                lastTaskActivityInstance.EndedByUserName);

            return rmins;
        }

        #endregion
    }
}
