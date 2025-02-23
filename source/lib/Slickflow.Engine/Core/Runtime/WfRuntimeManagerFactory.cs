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
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using System.Text;
using IronPython.Compiler.Ast;
using static IronPython.Runtime.Profiler;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Tasks;

namespace Slickflow.Engine.Core.Runtime
{
    /// <summary>
    /// Creating classes at runtime
    /// Static method: Create a runtime object for executing an instance
    /// 运行时的创建类
    /// 静态方法：创建执行实例的运行者对象
    /// </summary>
    internal class WfRuntimeManagerFactory
    {
        #region WfRuntimeManager Startup
        /// <summary>
        /// Startup Process
        /// 启动流程
        /// </summary>
        public static WfRuntimeManager CreateRuntimeInstanceStartup(WfAppRunner runner,
            ref WfExecutedResult result)
        {
            var rmins = new WfRuntimeManagerStartup();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            var pim = new ProcessInstanceManager();
            ProcessInstanceEntity processInstance = pim.GetProcessInstanceCurrent(runner.AppInstanceID,
                runner.ProcessID,
                runner.Version);

            //不能同时启动多个主流程
            //Cannot start multiple main processes simultaneously
            if (processInstance != null
                && string.IsNullOrEmpty(processInstance.SubProcessID)
                && processInstance.ProcessState == (short)ProcessStateEnum.Running)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Started_IsRunningAlready;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceStartup.error");
                return rmins;
            }

            rmins.AppRunner = runner;

            //获取流程第一个可办理节点
            //Obtain the first available processing node in the process
            rmins.ProcessModel = ProcessModelFactory.CreateByProcess(runner.ProcessID, runner.Version);
            var startActivity = rmins.ProcessModel.GetStartActivity();
            var nextActivityTree = rmins.ProcessModel.GetFirstActivityTree(startActivity, runner.Conditions);

            //开始节点之后可以有网关节点
            //After starting the node, there can be a gateway node
            if (startActivity.TriggerDetail != null)
            {
                if (startActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Timer
                    || startActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Message
                    || startActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Signal 
                    || startActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Conditional)
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
            }
            else
            {
                rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(nextActivityTree,
                    runner.UserID,
                    runner.UserName);
            }
            rmins.ActivityResource = new ActivityResource(runner, rmins.AppRunner.NextActivityPerformers, runner.Conditions);

            return rmins;
        }

        /// <summary>
        /// Sub Process Startup
        /// 子流程启动
        /// </summary>
        public static WfRuntimeManager CreateRuntimeInstanceStartupSub(WfAppRunner runner,
            ProcessInstanceEntity processInstance,
            SubProcessNode subProcessNode,
            PerformerList performerList,
            IDbSession session,
            ref WfExecutedResult result)
        {
            var rmins = new WfRuntimeManagerStartupSub();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            var pim = new ProcessInstanceManager();
            var subProcessInstance = pim.GetSubProcessInstance(session.Connection,
                runner.AppInstanceID,
                runner.ProcessID,
                subProcessNode.SubProcessID,
                session.Transaction);

            //不能同时启动多个主流程
            //Cannot start multiple main processes simultaneously
            if (subProcessInstance != null
                && subProcessInstance.ProcessState == (short)ProcessStateEnum.Running)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Started_IsRunningAlready;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceStartup.error");
                return rmins;
            }

            rmins.AppRunner = runner;
            rmins.ProcessInstance = subProcessInstance;
            rmins.InvokedSubProcessNode = subProcessNode;
            rmins.ProcessModel = ProcessModelFactory.CreateSubByNode(session.Connection, subProcessNode, session.Transaction);

            //获取流程第一个可办理节点
            //Obtain the first available processing node in the process
            var subFirstActivity = rmins.ProcessModel.GetFirstActivity();

            //子流程自动获取第一个办理节点上的人员列表
            //The subprocess automatically retrieves the list of performers on the first processing node
            rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(subFirstActivity.ActivityID, 
                performerList);
            rmins.ActivityResource = new ActivityResource(runner, rmins.AppRunner.NextActivityPerformers);

            return rmins;
        }
        #endregion

        #region WfRuntimeManager Running
        /// <summary>
        /// Create a runtime instance object
        /// 创建运行时实例对象
        /// </summary>
        public static WfRuntimeManager CreateRuntimeInstanceAppRunning(WfAppRunner runner,
            IDbSession session,
            ref WfExecutedResult result)
        {
            var rmins = new WfRuntimeManagerRun();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            //检查传入参数是否有效
            //Check if the incoming parameters are valid
            if (string.IsNullOrEmpty(runner.AppName)
                || string.IsNullOrEmpty(runner.AppInstanceID)
                || runner.ProcessID == null)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.RunApp_ErrorArguments;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceAppRunning.missing.error");
                return rmins;
            }
            rmins.AppRunner = runner;

            var aim = new ActivityInstanceManager();
            TaskViewEntity taskView = null;
            var runningNode = aim.GetRunningNode(runner, session, out taskView);

            //判断是否是当前登录用户的任务 
            //Determine whether it is the task of the currently logged in user
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
            //Provide process instance ID for process registration events
            var processInstance = (new ProcessInstanceManager()).GetById(session.Connection, runningNode.ProcessInstanceID, session.Transaction);
            rmins.ProcessInstanceID = runningNode.ProcessInstanceID;
            var processModel = ProcessModelFactory.CreateByTask(session.Connection, taskView, session.Transaction);
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

        #region WfRuntimeManager Run Automatically
        /// <summary>
        /// Create a runtime instance object
        /// 创建运行时实例对象
        /// </summary>
        public static WfRuntimeManager CreateRuntimeInstanceRunAuto(WfAppRunner runner,
            IDbSession session,
            ref WfExecutedResult result)
        {
            //检查传入参数是否有效
            //Check if the incoming parameters are valid
            var rmins = new WfRuntimeManagerRun();
            rmins.WfExecutedResult = result = new WfExecutedResult();
            if (string.IsNullOrEmpty(runner.AppName)
                || string.IsNullOrEmpty(runner.AppInstanceID)
                || runner.ProcessID == null)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.RunApp_ErrorArguments;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceAppRunning.missing.error");
                return rmins;
            }
            rmins.AppRunner = runner;

            var aim = new ActivityInstanceManager();
            var runningNode = aim.GetById(runner.ActivityInstanceID.Value);

            //用于流程注册事件时的流程实例ID提供
            //Provide process instance ID for process registration events
            var processInstance = (new ProcessInstanceManager()).GetById(session.Connection, runningNode.ProcessInstanceID, session.Transaction);
            rmins.ProcessInstanceID = runningNode.ProcessInstanceID;
            var processModel = ProcessModelFactory.CreateByProcess(processInstance.ProcessID, processInstance.Version);
            var activityResource = new ActivityResource(runner,
                runner.NextActivityPerformers,
                runner.Conditions);

            rmins.RunningActivityInstance = runningNode;
            rmins.ProcessModel = processModel;
            rmins.ActivityResource = activityResource;

            return rmins;
        }
        #endregion

        #region WfRuntimeManager Jump
        /// <summary>
        /// Create a runtime instance object
        /// 创建跳转实例信息
        /// </summary>
        public static WfRuntimeManager CreateRuntimeInstanceJump(WfAppRunner runner,
            ref WfExecutedResult result)
        {
            var rmins = new WfRuntimeManagerJump();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            //检查传入参数是否有效
            //Check if the incoming parameters are valid
            if (string.IsNullOrEmpty(runner.AppName)
               || string.IsNullOrEmpty(runner.AppInstanceID)
               || runner.ProcessID == null
               || runner.NextActivityPerformers == null)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Jump_ErrorArguments;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceJump.missing.error");
                return rmins;
            }

            //流程跳转时，只能跳转到一个节点
            //When a process jumps, it can only jump to one node
            if (runner.NextActivityPerformers.Count() > 1)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Jump_OverOneStep;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceJump.jump.error",
                    runner.NextActivityPerformers.Count().ToString());
                return rmins;
            }

            var aim = new ActivityInstanceManager();
            TaskViewEntity taskView = null;
            var runningNode = aim.GetRunningNode(runner, out taskView);

            rmins.TaskView = taskView;
            rmins.AppRunner.AppName = runner.AppName;
            rmins.AppRunner.AppInstanceID = runner.AppInstanceID;
            rmins.AppRunner.ProcessID = runner.ProcessID;
            rmins.AppRunner.UserID = runner.UserID;
            rmins.AppRunner.UserName = runner.UserName;
            rmins.AppRunner.NextActivityPerformers = runner.NextActivityPerformers;

            //用于流程注册时间调用时候的流程实例ID提供
            //Provide process instance ID for process registration events
            rmins.ProcessInstanceID = runningNode.ProcessInstanceID;
            rmins.RunningActivityInstance = runningNode;
            rmins.ActivityResource = new ActivityResource(runner, rmins.AppRunner.NextActivityPerformers);

            var processModel = ProcessModelFactory.CreateByTask(taskView);
            rmins.ProcessModel = processModel;

            return rmins;
        }
        #endregion

        #region WfRuntimeManager Reject
        /// <summary>
        /// Create a runtime instance object
        /// 创建运行时实例对象
        /// </summary>
        public static WfRuntimeManager CreateRuntimeInstanceReject(WfAppRunner runner,
            ref WfExecutedResult result)
        {
            var rmins = new WfRuntimeManagerReject();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            //检查传入参数是否有效
            //Check if the incoming parameters are valid
            if (string.IsNullOrEmpty(runner.AppName)
               || string.IsNullOrEmpty(runner.AppInstanceID)
               || runner.ProcessID == null
               || runner.NextActivityPerformers == null)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Jump_ErrorArguments;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceReject.missing.error");
                return rmins;
            }

            if (runner.NextActivityPerformers.Count() > 1)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Jump_OverOneStep;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceReject.jump.error",
                    runner.NextActivityPerformers.Count().ToString());
                return rmins;
            }

            var aim = new ActivityInstanceManager();
            TaskViewEntity taskView = null;
            var runningNode = aim.GetRunningNode(runner, out taskView);

            rmins.TaskView = taskView;
            rmins.AppRunner = runner;
            rmins.AppRunner.AppName = runner.AppName;
            rmins.AppRunner.AppInstanceID = runner.AppInstanceID;
            rmins.AppRunner.ProcessID = runner.ProcessID;
            rmins.AppRunner.UserID = runner.UserID;
            rmins.AppRunner.UserName = runner.UserName;

            rmins.ProcessInstanceID = runningNode.ProcessInstanceID;

            var processModel = ProcessModelFactory.CreateByTask(taskView);
            rmins.ProcessModel = processModel;

            //获取跳转节点信息
            //Obtain jump node information
            var jumpBackActivityID = runner.NextActivityPerformers.First().Key;
            var jumpBackActivityInstance = aim.GetActivityInstanceLatest(runningNode.ProcessInstanceID, jumpBackActivityID);


            //跳转到曾经执行过的节点上,可以作为跳回方式处理
            //Jumping to a node that has been executed before can be handled as a bounce back method
            rmins.IsBackward = true;
            rmins.BackwardContext.ProcessInstance = (new ProcessInstanceManager()).GetById(runningNode.ProcessInstanceID);
            rmins.BackwardContext.BackwardToTaskActivity = processModel.GetActivity(jumpBackActivityID);

            //获取当前运行节点的上一步节点
            //Retrieve the previous node of the current running node
            bool hasGatewayNode = false;
            var tim = new TransitionInstanceManager();
            var lastTaskTransitionInstance = tim.GetLastTaskTransition(runner.AppName,
                runner.AppInstanceID, runner.ProcessID);

            var npc = new PreviousStepChecker();
            var previousActivityInstanceList = npc.GetPreviousActivityInstanceList(runningNode, true,
                out hasGatewayNode).ToList();
            var previousActivityInstance = (previousActivityInstanceList.Count > 0) ? previousActivityInstanceList[0] : null;

            //仅仅是回跳到上一步节点，即按SendBack方式处理
            //Just jump back to the previous node and process it according to the SendBack method
            if (previousActivityInstance != null && previousActivityInstance.ActivityID == jumpBackActivityID)
            {
                rmins.BackwardContext.BackwardToTaskActivityInstance = previousActivityInstance;
                rmins.BackwardContext.BackwardToTargetTransitionGUID =
                    hasGatewayNode == false ? lastTaskTransitionInstance.TransitionID : WfDefine.WF_XPDL_GATEWAY_BYPASS_GUID;        //如果中间有Gateway节点，则没有直接相连的TransitonGUID

                rmins.BackwardContext.BackwardFromActivity = processModel.GetActivity(runningNode.ActivityID);
                rmins.BackwardContext.BackwardFromActivityInstance = runningNode;
                rmins.BackwardContext.BackwardTaskReceiver = WfBackwardTaskReceiver.Instance(
                    previousActivityInstance.ActivityName,
                    previousActivityInstance.EndedByUserID,
                    previousActivityInstance.EndedByUserName);

                rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(
                    previousActivityInstance.ActivityID,
                    previousActivityInstance.EndedByUserID,
                    previousActivityInstance.EndedByUserName);
            }
            else
            {
                //回跳到早前节点
                //Jump back to the previous node
                if (jumpBackActivityInstance.ActivityState != (short)ActivityStateEnum.Completed)
                {
                    result.Status = WfExecutedStatus.Exception;
                    result.ExceptionType = WfExceptionType.Jump_NotActivityBackCompleted;
                    result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceReject.back.error");

                    return rmins;
                }

                rmins.BackwardContext.BackwardToTaskActivityInstance = jumpBackActivityInstance;

                //判断两个节点是否有Transition的定义存在
                //Determine whether there is a definition of transition between two nodes
                var transition = processModel.GetForwardTransition(runningNode.ActivityID, jumpBackActivityID);
                rmins.BackwardContext.BackwardToTargetTransitionGUID = transition != null ? transition.TransitionID : WfDefine.WF_XPDL_GATEWAY_BYPASS_GUID;

                rmins.BackwardContext.BackwardFromActivity = processModel.GetActivity(runningNode.ActivityID);
                rmins.BackwardContext.BackwardFromActivityInstance = runningNode;
                rmins.BackwardContext.BackwardTaskReceiver = WfBackwardTaskReceiver.Instance(
                    jumpBackActivityInstance.ActivityName,
                    jumpBackActivityInstance.EndedByUserID,
                    jumpBackActivityInstance.EndedByUserName);

                rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(
                    jumpBackActivityInstance.ActivityID,
                    jumpBackActivityInstance.EndedByUserID,
                    jumpBackActivityInstance.EndedByUserName);
            }

            var activityResourceBack = new ActivityResource(rmins.AppRunner,
                rmins.AppRunner.NextActivityPerformers,
                runner.Conditions);
            rmins.ActivityResource = activityResourceBack;

            return rmins;
        }
        #endregion

        #region WfRuntimeManager Close
        /// <summary>
        /// Create a runtime instance object
        /// 创建运行时实例对象
        /// </summary>
        public static WfRuntimeManager CreateRuntimeInstanceClose(WfAppRunner runner,
            ref WfExecutedResult result)
        {
            var rmins = new WfRuntimeManagerClose();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            //检查传入参数是否有效
            //Check if the incoming parameters are valid
            if (string.IsNullOrEmpty(runner.AppName)
               || string.IsNullOrEmpty(runner.AppInstanceID)
               || runner.ProcessID == null
               || runner.NextActivityPerformers == null)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Jump_ErrorArguments;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceClose.missing.error");
                return rmins;
            }

            //流程跳转时，只能跳转到一个节点
            //When a process jumps, it can only jump to one node
            if (runner.NextActivityPerformers.Count() > 1)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Jump_OverOneStep;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceClose.jump.error",
                    runner.NextActivityPerformers.Count().ToString());
                return rmins;
            }

            var aim = new ActivityInstanceManager();
            TaskViewEntity taskView = null;
            var runningNode = aim.GetRunningNode(runner, out taskView);

            rmins.TaskView = taskView;
            rmins.AppRunner = runner;
            rmins.AppRunner.AppName = runner.AppName;
            rmins.AppRunner.AppInstanceID = runner.AppInstanceID;
            rmins.AppRunner.ProcessID = runner.ProcessID;
            rmins.AppRunner.UserID = runner.UserID;
            rmins.AppRunner.UserName = runner.UserName;

            //用于流程注册时间调用时候的流程实例ID提供
            //Provide process instance ID for process registration time call
            rmins.ProcessInstanceID = runningNode.ProcessInstanceID;

            var processModel = ProcessModelFactory.CreateByTask(taskView);
            rmins.ProcessModel = processModel;

            var endActivityID = runner.NextActivityPerformers.First().Key;
            var activityResource = new ActivityResource(runner, runner.NextActivityPerformers);
            rmins.ActivityResource = activityResource;
            rmins.RunningActivityInstance = runningNode;

            return rmins;
        }
        #endregion

        #region WfRuntimeManager SendBack
        /// <summary>
        /// Create a runtime instance object
        /// 创建运行时实例对象
        /// </summary>
        internal static WfRuntimeManager CreateRuntimeInstanceSendBack(WfAppRunner runner,
            ref WfExecutedResult result)
        {
            var rmins = new WfRuntimeManagerSendBack();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            //没有指定退回节点信息
            //No sendback node information specified
            if (runner.NextPerformerType != NextPerformerIntTypeEnum.Traced
                && (runner.NextActivityPerformers == null || runner.NextActivityPerformers.Count == 0))
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_IsNull;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceSendBack.null.error");

                return rmins;
            }

            //先查找当前用户正在办理的运行节点
            //First, search for the running node that the current user is currently processing
            var aim = new ActivityInstanceManager();
            TaskViewEntity taskView = null;
            var runningNode = aim.GetRunningNode(runner, out taskView);

            if (runningNode == null
                || (runningNode.ActivityState != (short)ActivityStateEnum.Ready && runningNode.ActivityState != (short)ActivityStateEnum.Running))
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
            if (XPDLHelper.IsSimpleComponentNode(activityType) == false
                 && XPDLHelper.IsCrossOverComponentNode(activityType) == false)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_NotTaskNode;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceSendBack.nottask.error");
                return rmins;
            }

            //获取上一步节点信息
            //Obtain the previous node information
            var hasGatewayPassed = false;
            var processInstance = (new ProcessInstanceManager()).GetById(runningNode.ProcessInstanceID);
            var processModel = ProcessModelFactory.CreateByProcessInstance(processInstance);
            var previousStepChecker = new PreviousStepChecker();
            var previousActivityList = previousStepChecker.GetPreviousActivityList(runningNode, processModel, out hasGatewayPassed);

            //判断退回是否有效
            //Determine if the return is valid
            if (previousActivityList == null || previousActivityList.Count == 0)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_IsNull;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceSendBack.empty.error");
                return rmins;
            }

            //前端用户指定退回步骤的模式
            //Front end user specifies the mode of return steps
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
                //Check if the nodes are consistent
                if (previousActivityList.Count == 1)
                {
                    var onlyActivityID = previousActivityList[0].ActivityID;
                    var isOnly = true;
                    foreach (var step in runner.NextActivityPerformers)
                    {
                        if (step.Key != onlyActivityID)
                        {
                            isOnly = false;
                            break;
                        }
                    }

                    //存在不一致的退回节点
                    //There are inconsistent return nodes
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
                //Traced is used to directly return to the previous step for testing mode
                var prevActivity = previousActivityList[0];
                var performerList = PerformerBuilder.CreatePerformerList(runningNode.CreatedByUserID, runningNode.CreatedByUserName);
                runner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(prevActivity.ActivityID, performerList);
            }

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
            //Set sendback options
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
        /// Determine whether the steps passed are in the list
        /// 判断传递的步骤是否在列表中
        /// </summary>
        private static bool IsInvalidStepsInPrevousActivityList(IList<Activity> previousActivityList, 
            IDictionary<string, PerformerList> steps,
            IList<Activity> sendbackPreviousActivityList)
        {
            var isInvalid = false;
            foreach (var key in steps.Keys)
            {
                var activity = previousActivityList.Single(s => s.ActivityID == key);
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
        /// Filter pre nodes
        /// 过滤前置节点
        /// </summary>
        private static IList<Activity> FilterPreviousActivityList(IList<Activity> previousActivityList,
            IList<ActivityInstanceEntity> previousActivityInstanceList)
        {
            IList<Activity> activityList = new List<Activity>();
            foreach (var activity in previousActivityList)
            {
                var list = previousActivityInstanceList.Where(a => a.ActivityID == activity.ActivityID).ToList();
                if (list != null && list.Count > 0)
                {
                    activityList.Add(activity);
                }
            }
            return activityList;
        }
        #endregion

        #region WfRuntimeManager Withdraw
        /// <summary>
        /// Create a runtime instance object
        /// 创建运行时实例对象
        /// </summary>
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
            //Retrieve information on completed tasks
            var tm = new TaskManager();
            var taskDone = tm.GetTaskView(runner.TaskID.Value);

            if (tm.IsMine(taskDone, runner.UserID) == false)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_NotMineTask;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceWithdraw.nonetask.error");
                return rmins;
            }

            runner.NextActivityPerformers = NextStepUtility.CreateNextStepPerformerList(taskDone.ActivityID,
                taskDone.AssignedToUserID, taskDone.AssignedToUserName);

            //没有指定退回节点信息
            //No return node information specified
            if (runner.NextActivityPerformers == null || runner.NextActivityPerformers.Count == 0)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_IsNull;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceWithdraw.noneperformer.error");

                return rmins;
            }

            //获取待办任务
            //Get todo tasks
            var tim = new TransitionInstanceManager();
            var nextStepList = tim.GetTargetActivityInstanceList(taskDone.ActivityInstanceID).ToList();
            
            ActivityInstanceEntity runningNode = nextStepList.Count > 0 ?  nextStepList[0] : null;
            if (runningNode.ActivityType == (short)ActivityTypeEnum.MultiSignNode)
            {
                using (var session = SessionFactory.CreateSession())
                {
                    var aim = new ActivityInstanceManager();
                    var childList = aim.GetActivityMulitipleInstanceWithStateBatch(runningNode.ProcessInstanceID, runningNode.ID, (short)ActivityStateEnum.Ready, session);
                    runningNode = childList.FirstOrDefault();
                }
            }

            if (runningNode == null
                || (runningNode.ActivityState != (short)ActivityStateEnum.Ready && runningNode.ActivityState !=(short)ActivityStateEnum.Running))
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_NotInRunning;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceWithdraw.nonerun.error");

                return rmins;
            }

            var activityType = EnumHelper.ParseEnum<ActivityTypeEnum>(runningNode.ActivityType.ToString());
            if (XPDLHelper.IsSimpleComponentNode(activityType) == false
                 && XPDLHelper.IsCrossOverComponentNode(activityType) == false)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_NotTaskNode;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceWithdraw.nottask.error");
                return rmins;
            }

            var taskToDo = tm.GetTaskViewByActivity(runningNode.ProcessInstanceID, runningNode.ID);
            runner.UserID = taskToDo.AssignedToUserID;
            runner.UserName = taskToDo.AssignedToUserName;

            //获取上一步节点信息
            //Obtain the previous node information
            var hasGatewayPassed = false;
            var processInstance = (new ProcessInstanceManager()).GetById(runningNode.ProcessInstanceID);
            
            var previousStepChecker = new PreviousStepChecker();
            var processModel = ProcessModelFactory.CreateByTask(taskToDo);
            var previousActivityList = previousStepChecker.GetPreviousActivityList(runningNode, processModel, out hasGatewayPassed);

            //判断退回是否有效
            //Determine if the return is valid
            if (previousActivityList == null || previousActivityList.Count == 0)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Sendback_IsNull;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceWithdraw.empty.error");
                return rmins;
            }

            //检查节点是否一致
            //Check if the nodes are consistent
            if (previousActivityList.Count == 1)
            {
                var onlyActivityID = previousActivityList[0].ActivityID;
                var isOnly = true;
                foreach (var step in runner.NextActivityPerformers)
                {
                    if (step.Key != onlyActivityID)
                    {
                        isOnly = false;
                        break;
                    }
                }

                //存在不一致的退回节点
                //There are inconsistent return nodes
                if (isOnly == false)
                {
                    result.Status = WfExecutedStatus.Exception;
                    result.ExceptionType = WfExceptionType.Sendback_IsTooManyPrevious;
                    result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceWithdraw.notunique.error");

                    return rmins;
                }
            }

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
            //Set return option class
            var sendbackOperation = new SendBackOperation();
            sendbackOperation.BackwardType = BackwardTypeEnum.Withdrawed;
            sendbackOperation.ProcessInstance = processInstance;
            sendbackOperation.BackwardFromActivityInstance = runningNode;
            sendbackOperation.HasGatewayPassed = hasGatewayPassed;
            sendbackOperation.ActivityResource = new ActivityResource(runner, rmins.AppRunner.NextActivityPerformers);
            sendbackOperation.ProcessModel = processModel;

            //撤销时默认撤销各个并行分支
            //Default revocation of each parallel branch during revocation
            sendbackOperation.IsCancellingBrothersNode = true;          

            rmins.SendBackOperation = sendbackOperation;

            return rmins;
        }
        #endregion

        #region WfRuntimeManager Resend
        /// <summary>
        /// Create a runtime instance object
        /// 创建运行时实例对象
        /// </summary>
        internal static WfRuntimeManager CreateRuntimeInstanceResend(WfAppRunner runner,
            ref WfExecutedResult result)
        {
            WfRuntimeManager rmins = new WfRuntimeManagerResend();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            if (runner.TaskID == null)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Resend_NotTaskNode;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceResend.missingtaskid.error");

                return rmins;
            }

            //获取退回源活动实例数据
            //Retrieve the data of the returned source activity instance
            var tm = new TaskManager();
            var taskView = tm.GetTaskView(runner.TaskID.Value);

            var aim = new ActivityInstanceManager();
            var runningActivityInstance = aim.GetById(taskView.ActivityInstanceID);
            if (runningActivityInstance.BackSrcActivityInstanceID == null)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Resend_WithoutBackSourceNode;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceResend.nonebacksrc.error");

                return rmins;
            }
            var backSrcActivityInstance = aim.GetById(runningActivityInstance.BackSrcActivityInstanceID.Value);

            rmins.TaskView = taskView;
            rmins.RunningActivityInstance = aim.GetById(taskView.ActivityInstanceID);
            rmins.ProcessModel = ProcessModelFactory.CreateByTask(taskView);

            rmins.AppRunner.AppName = runner.AppName;
            rmins.AppRunner.AppInstanceID = runner.AppInstanceID;
            rmins.AppRunner.UserID = runner.UserID;
            rmins.AppRunner.UserName = runner.UserName;
            if (runner.ControlParameterSheet != null
                && runner.ControlParameterSheet.RecreatedMultipleInstanceWhenResending == 1)
            {
                rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(
                    backSrcActivityInstance.ActivityID,
                    runner.NextActivityPerformers[backSrcActivityInstance.ActivityID]);
            }
            else
            {
                rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(
                    backSrcActivityInstance.ActivityID,
                    runningActivityInstance.CreatedByUserID,
                    runningActivityInstance.CreatedByUserName);
            }
            rmins.ActivityResource = new ActivityResource(runner, rmins.AppRunner.NextActivityPerformers);

            //用于流程注册事件时的流程实例ID提供
            //Provide process instance ID for process registration events
            rmins.ProcessInstanceID = runningActivityInstance.ProcessInstanceID;

            return rmins;
        }
        #endregion

        #region WfRuntimerManager Revise
        /// <summary>
        /// Create a runtime instance object
        /// 创建运行时实例对象
        /// </summary>
        public static WfRuntimeManager CreateRuntimeInstanceRevise(WfAppRunner runner,
            ref WfExecutedResult result)
        {
            WfRuntimeManager rmins = new WfRuntimeManagerRevise();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            if (runner.TaskID == null)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Resend_NotTaskNode;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceRevise.missingtaskid.error");

                return rmins;
            }

            //获取退回源活动实例数据
            //Retrieve the data of the returned source activity instance
            var tm = new TaskManager();
            var taskView = tm.GetTaskView(runner.TaskID.Value);

            var aim = new ActivityInstanceManager();

            rmins.TaskView = taskView;
            rmins.RunningActivityInstance = aim.GetById(taskView.ActivityInstanceID);
            rmins.ProcessModel = ProcessModelFactory.CreateByTask(taskView);

            rmins.AppRunner.AppName = runner.AppName;
            rmins.AppRunner.AppInstanceID = runner.AppInstanceID;
            rmins.AppRunner.UserID = runner.UserID;
            rmins.AppRunner.UserName = runner.UserName;
            rmins.AppRunner.NextActivityPerformers = runner.NextActivityPerformers;
            rmins.ActivityResource = new ActivityResource(runner, rmins.AppRunner.NextActivityPerformers);

            //用于流程注册事件时的流程实例ID提供
            //Provide process instance ID for process registration events
            rmins.ProcessInstanceID = taskView.ProcessInstanceID;

            return rmins;
        }
        #endregion

        #region WfRuntimeManager Reverse
        /// <summary>
        /// Create a runtime instance object
        /// 创建运行时实例对象
        /// </summary>
        public static WfRuntimeManager CreateRuntimeInstanceReverse(WfAppRunner runner,
            ref WfExecutedResult result)
        {
            var rmins = new WfRuntimeManagerReverse();
            rmins.WfExecutedResult = result = new WfExecutedResult();
            var pim = new ProcessInstanceManager();
            var processInstance = pim.GetProcessInstanceLatest(runner.AppInstanceID, runner.ProcessID, runner.Version);
            if (processInstance == null || processInstance.ProcessState != (short)ProcessStateEnum.Completed)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.Reverse_NotInCompleted;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceReverse.error");
                return rmins;
            }

            //用于注册事件时候的流程ID
            //Process ID used for registering events
            rmins.ProcessInstanceID = processInstance.ID;

            var tim = new TransitionInstanceManager();
            var endTransitionInstance = tim.GetEndTransition(runner.AppName, runner.AppInstanceID, runner.ProcessID);

            var processModel = ProcessModelFactory.CreateByProcessInstance(processInstance);
            var endActivity = processModel.GetActivity(endTransitionInstance.ToActivityID);

            var aim = new ActivityInstanceManager();
            var endActivityInstance = aim.GetById(endTransitionInstance.ToActivityInstanceID);

            bool hasGatewayNode = false;
            var npc = new PreviousStepChecker();
            var lastTaskActivityInstance = npc.GetPreviousActivityInstanceList(endActivityInstance, false,
                out hasGatewayNode).ToList()[0];
            var lastTaskActivity = processModel.GetActivity(lastTaskActivityInstance.ActivityID);

            //封装返签结束点之前办理节点的任务接收人
            //The recipient of the task to handle the node before the end point of the encapsulated return signature
            rmins.AppRunner.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(lastTaskActivityInstance.ActivityID,
                lastTaskActivityInstance.EndedByUserID,
                lastTaskActivityInstance.EndedByUserName);

            rmins.ActivityResource = new ActivityResource(runner, rmins.AppRunner.NextActivityPerformers);
            rmins.AppRunner.AppName = runner.AppName;
            rmins.AppRunner.AppInstanceID = runner.AppInstanceID;
            rmins.AppRunner.ProcessID = runner.ProcessID;
            rmins.AppRunner.UserID = runner.UserID;
            rmins.AppRunner.UserName = runner.UserName;

            rmins.BackwardContext.ProcessInstance = processInstance;
            rmins.BackwardContext.BackwardToTaskActivity = lastTaskActivity;
            rmins.BackwardContext.BackwardToTaskActivityInstance = lastTaskActivityInstance;
            rmins.BackwardContext.BackwardToTargetTransitionGUID =
                hasGatewayNode == false ? endTransitionInstance.TransitionID : string.Empty;
            rmins.BackwardContext.BackwardFromActivity = endActivity;
            rmins.BackwardContext.BackwardFromActivityInstance = endActivityInstance;
            rmins.BackwardContext.BackwardTaskReceiver = WfBackwardTaskReceiver.Instance(lastTaskActivityInstance.ActivityName,
                lastTaskActivityInstance.EndedByUserID,
                lastTaskActivityInstance.EndedByUserName);
            

            return rmins;
        }
        #endregion

        #region WfRuntimeManager SignForward
        /// <summary>
        /// Create a runtime instance object
        /// 创建运行时实例对象
        /// </summary>
        public static WfRuntimeManager CreateRuntimeInstanceSignForward(WfAppRunner runner,
            ref WfExecutedResult result)
        {
            var rmins = new WfRuntimeManagerSignForward();
            rmins.WfExecutedResult = result = new WfExecutedResult();

            if (string.IsNullOrEmpty(runner.AppName)
                || string.IsNullOrEmpty(runner.AppInstanceID)
                || runner.ProcessID == null
                || runner.NextActivityPerformers == null)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.SignForward_ErrorArguments;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceSignForward.missing.error");
                return rmins;
            }

            if (runner.NextActivityPerformers.Count() == 0)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.SignForward_NoneSigners;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceSignForward.noneperformer.error");
                return rmins;
            }

            rmins.AppRunner = runner;

            var aim = new ActivityInstanceManager();
            TaskViewEntity taskView = null;
            var runningNode = aim.GetRunningNode(runner, out taskView);

            //判断是否是当前登录用户的任务
            //Determine whether it is the task of the currently logged in user
            if (runningNode.AssignedToUserIDs.Contains(runner.UserID.ToString()) == false)
            {
                result.Status = WfExecutedStatus.Exception;
                result.ExceptionType = WfExceptionType.RunApp_HasNoTask;
                result.Message = LocalizeHelper.GetEngineMessage("wfruntimemanagerfactory.CreateRuntimeInstanceSignForward.nonetask.error");
                return rmins;
            }

            //用于注册事件时候的流程ID
            //Process ID used for registering events
            rmins.ProcessInstanceID = runningNode.ProcessInstanceID;

            var processModel = ProcessModelFactory.CreateByTask(taskView);
            var activityResource = new ActivityResource(runner, 
                runner.NextActivityPerformers, 
                runner.Conditions);

            var tm = new TaskManager();
            rmins.TaskView = taskView;
            rmins.RunningActivityInstance = runningNode;
            rmins.ProcessModel = processModel;
            rmins.ActivityResource = activityResource;

            return rmins;
        }
        #endregion

        #region WfRuntimeManager Register Event
        /// <summary>
        /// Regiester Event
        /// </summary>
        /// <param name="runtimeInstance"></param>
        /// <param name="executing"></param>
        /// <param name="executed"></param>
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
        /// Unregister Event
        /// 事件注销
        /// </summary>
        /// <param name="runtimeInstance"></param>
        /// <param name="executing"></param>
        /// <param name="executed"></param>
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
