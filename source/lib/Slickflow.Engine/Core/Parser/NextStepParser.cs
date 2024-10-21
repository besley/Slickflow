﻿using System;
using System.Collections.Generic;
using System.Linq;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Core.Parser
{
    /// <summary>
    /// 下一步的步骤列表读取类
    /// </summary>
    internal class NextStepParser
    {
        /// <summary>
        /// 流程下一步信息获取
        /// </summary>
        /// <param name="resourceService">资源服务</param>
        /// <param name="runner">当前运行用户</param>
        /// <param name="condition">条件</param>
        /// <returns>下一步信息</returns>
        internal NextStepInfo GetNextStepInfo(IResourceService resourceService,
            WfAppRunner runner, 
            IDictionary<string, string> condition = null)
        {
            NextStepInfo nextStepInfo = new NextStepInfo();
            var nextResult = GetNextActivityRoleUserTree(resourceService, runner, condition);
            nextStepInfo.Message = nextResult.Message;
            nextStepInfo.NextActivityRoleUserTree = nextResult.StepList;
            nextStepInfo.NextActivityPerformers = GetNextActivityPerformersPriliminary(runner);

            return nextStepInfo;
        }

        /// <summary>
        /// 获取预选步骤人员列表
        /// </summary>
        /// <param name="runner">当前运行用户</param>
        /// <returns>步骤预选人员列表</returns>
        private IDictionary<string, PerformerList> GetNextActivityPerformersPriliminary(WfAppRunner runner)
        {
            IDictionary<string, PerformerList> nextSteps = null;

            var tm = new TaskManager();
            TaskViewEntity taskView = tm.GetTaskOfMine(runner);

            //读取活动实例中记录的步骤预选数据
            var aim = new ActivityInstanceManager();
            if (taskView.MIHostActivityInstanceID != null)
            {
                var mainActivityInstanceID = taskView.MIHostActivityInstanceID.Value;
                var mainActivityInstance = aim.GetById(mainActivityInstanceID);
                if (mainActivityInstance != null)
                {
                    nextSteps = NextStepUtility.DeserializeNextStepPerformers(mainActivityInstance.NextStepPerformers);
                }
            }

            //获取下一步信息
            var processModel = ProcessModelFactory.CreateByTask(taskView);
            var nextActivity = processModel.GetNextActivity(taskView.ActivityGUID);
            if (nextActivity != null)
            {
                if (nextActivity.ActivityType == ActivityTypeEnum.GatewayNode)
                {
                    //获取网关节点信息
                    var gatewayActivityInstance = aim.GetActivityInstanceLatest(taskView.ProcessInstanceID, nextActivity.ActivityGUID);
                    if (gatewayActivityInstance != null
                        && !string.IsNullOrEmpty(gatewayActivityInstance.NextStepPerformers))
                    {
                        nextSteps = NextStepUtility.DeserializeNextStepPerformers(gatewayActivityInstance.NextStepPerformers);
                    }
                }
                else if (XPDLHelper.IsInterTimerEventComponentNode(nextActivity) == true)
                {
                    //中间Timer事件节点
                    var timerActivityInstance = aim.GetActivityInstanceLatest(taskView.ProcessInstanceID, nextActivity.ActivityGUID);
                    if (timerActivityInstance != null
                        && !string.IsNullOrEmpty(timerActivityInstance.NextStepPerformers))
                    {
                        nextSteps = NextStepUtility.DeserializeNextStepPerformers(timerActivityInstance.NextStepPerformers);
                    }
                }
            }
            return nextSteps;
        }

        /// <summary>
        /// 根据应用获取流程下一步节点列表，包含角色用户
        /// </summary>
        /// <param name="resourceService">资源服务</param>
        /// <param name="runner">应用执行人</param>
        /// <param name="condition">条件</param>
        /// <returns>节点列表</returns>
        internal NextActivityTreeResult GetNextActivityRoleUserTree(IResourceService resourceService, 
            WfAppRunner runner,
            IDictionary<string, string> condition)
        {
            //判断应用数据是否缺失
            if (string.IsNullOrEmpty(runner.AppInstanceID)
                || string.IsNullOrEmpty(runner.ProcessGUID))
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("nextstepparser.getnextactivityroleusertree.error"));
            }

            //条件参数一致
            if (condition == null && runner.Conditions != null)
            {
                condition = runner.Conditions;
            }

            NextActivityTreeResult nextTreeResult = null;
            IProcessModel processModel = null;

            using (var session = SessionFactory.CreateSession())
            {
                TaskViewEntity taskView = null;
                var tm = new TaskManager();
                ProcessInstanceEntity processInstance = null;
                var pim = new ProcessInstanceManager();
                if (runner.TaskID != null)
                {
                    //运行中的流程
                    taskView = tm.GetTaskView(runner.TaskID.Value);
                    processInstance = pim.GetById(taskView.ProcessInstanceID);
                }
                else
                {
                    var processInstanceList = pim.GetProcessInstance(session.Connection,
                        runner.AppInstanceID,
                        runner.ProcessGUID,
                        runner.Version,
                        session.Transaction).ToList();
                    processInstance = EnumHelper.GetFirst<ProcessInstanceEntity>(processInstanceList);
                }

                //判断流程是否创建还是已经运行
                if (processInstance != null
                    && processInstance.ProcessState == (short)ProcessStateEnum.Running)
                {
                    //运行状态的流程实例
                    if (taskView == null)
                    {
                        taskView = tm.GetTaskOfMine(session.Connection, runner, session.Transaction);
                    }

                    var isRunningTask = tm.CheckTaskStateInRunningState(taskView);
                    if (isRunningTask == false)
                    {
                        throw new WorkflowException(LocalizeHelper.GetEngineMessage("nextstepparser.getnextactivityroleusertree.notrunning.error"));
                    }

                    //获取下一步列表
                    processModel = ProcessModelFactory.CreateByTask(session.Connection, taskView, session.Transaction);
                    nextTreeResult = processModel.GetNextActivityTree(taskView.ActivityGUID, taskView.TaskID, condition, session);
                    
                    foreach (var ns in nextTreeResult.StepList)
                    {
                        if (ns.ReceiverType == ReceiverTypeEnum.ProcessInitiator)       //下一步执行人为流程发起人
                        {
                            ns.Users = AppendUserList(ns.Users, pim.GetProcessInitiator(session.Connection,
                                taskView.ProcessInstanceID,
                                session.Transaction));   //获取流程发起人
                        }
                        else
                        {
                            var roleIDs = ns.Roles.Select(x => x.ID).ToArray();
                            if (roleIDs.Count() > 0)
                            {
                                ns.Users = resourceService.GetUserListByRoleReceiverType(roleIDs, runner.UserID, (int)ns.ReceiverType);     //增加转移前置过滤条件
                            }
                        }
                    }
                }
                else
                {
                    //流程准备启动，获取第一个办理节点的用户列表
                    processModel = ProcessModelFactory.CreateByProcess(runner.ProcessGUID, runner.Version);
                    var firstActivity = processModel.GetFirstActivity();
                    nextTreeResult = processModel.GetNextActivityTree(firstActivity.ActivityGUID,
                        null,
                        condition,
                        session);
                    foreach (var ns in nextTreeResult.StepList)
                    {
                        var roleIDs = ns.Roles.Select(x => x.ID).ToArray();
                        ns.Users = resourceService.GetUserListByRoleReceiverType(roleIDs, runner.UserID, (int)ns.ReceiverType);     //增加转移前置过滤条件
                    }
                }
            }
            return nextTreeResult;
        }

        /// <summary>
        /// 增加单个用户
        /// </summary>
        /// <param name="existUserList">用户列表</param>
        /// <param name="user">追加用户</param>
        /// <returns>用户列表</returns>
        private IList<User> AppendUserList(IList<User> existUserList, User user)
        {
            if (existUserList == null)
            {
                existUserList = new List<User>();
            }
            existUserList.Add(new User { UserID = user.UserID, UserName = user.UserName });
            return existUserList;
        }


        /// <summary>
        /// 构造用户列表
        /// </summary>
        /// <param name="existUserList">用户列表</param>
        /// <param name="newUserList">追加用户列表</param>
        /// <returns>用户列表</returns>
        private IList<User> AppendUserList(IList<User> existUserList, IList<User> newUserList)
        {
            if (existUserList == null)
            {
                existUserList = new List<User>();
            }

            foreach (var user in newUserList)
            {
                if (existUserList.Select(r => r.UserID == user.UserID).ToList() == null)
                {
                    existUserList.Add(new User { UserID = user.UserID, UserName = user.UserName });
                }
            }
            return existUserList;
        }
    }
}
