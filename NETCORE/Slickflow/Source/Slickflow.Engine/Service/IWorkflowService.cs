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
using System.Data;
using Slickflow.Data;
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Engine.Delegate;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Core;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Node;

namespace Slickflow.Engine.Service
{
    /// <summary>
    /// 工作流服务接口
    /// </summary>
    public interface IWorkflowService
    {
        ActivityEntity GetFirstActivity(string processGUID, string version);
        IList<ActivityEntity> GetTaskActivityList(string processGUID, string version);
		IList<ActivityEntity> GetAllTaskActivityList(string processGUID, string version);
        ActivityEntity GetNextActivity(string processGUID, string version, string activityGUID);
        IList<NodeView> GetNextActivity(string processGUID, string version, string activityGUID, IDictionary<string, string> condition);
        ActivityEntity GetActivityEntity(string processGUID, string version, string actvityGUID);
        IList<Role> GetActivityRoles(string processGUID, string version, string activityGUID);
        NodeView GetNextActivity(int taskID, IDictionary<string, string> condition = null);
        NodeView GetNextActivity(WfAppRunner runner, IDictionary<string, string> condition = null);
        IList<NodeView> GetNextActivityTree(int taskID, IDictionary<string, string> condition = null);
        IList<NodeView> GetNextActivityTree(WfAppRunner runner, IDictionary<string, string> condition = null);
        IList<NodeView> GetNextActivityRoleUserTree(WfAppRunner runner, IDictionary<string, string> condition = null);
        IList<NodeView> GetFirstActivityRoleUserTree(WfAppRunner runner, IDictionary<string, string> condition = null);


        WfExecutedResult StartProcess(WfAppRunner runner);
        WfExecutedResult StartProcess(WfAppRunner starter, IDbSession session);
        WfExecutedResult RunProcessApp(WfAppRunner runner);
        WfExecutedResult RunProcessApp(WfAppRunner runner, IDbSession session);
        WfExecutedResult JumpProcess(WfAppRunner runner);
        WfExecutedResult JumpProcess(WfAppRunner runner, IDbSession session);
        WfExecutedResult WithdrawProcess(WfAppRunner runner);
        WfExecutedResult WithdrawProcess(WfAppRunner withdrawer, IDbSession session);
        WfExecutedResult SendBackProcess(WfAppRunner runner);
        WfExecutedResult SendBackProcess(WfAppRunner runner, IDbSession session);
        WfExecutedResult ReverseProcess(WfAppRunner runner);
        WfExecutedResult ReverseProcess(WfAppRunner starter, IDbSession session);

        Boolean ResumeProcess(int activityInstanceId, WfAppRunner runner);
        Boolean SuspendProcess(int taskId, WfAppRunner runner);
        Boolean CancelProcess(int processInstanceID, WfAppRunner canceler);
        Boolean DiscardProcess(int processInstanceID, WfAppRunner discarder);
        Boolean SetTaskRead(WfAppRunner runner);
        Boolean EntrustTask(TaskEntrustedEntity entrusted, bool cancalOriginalTask = true);
        Boolean SetProcessOverdue(int processInstanceID, DateTime overdueDateTime, WfAppRunner runner);

        IList<NodeImage> GetActivityInstanceCompleted(int taskID);
        IList<NodeImage> GetActivityInstanceCompleted(WfAppRunner runner);
        IList<TransitionImage> GetTransitionInstanceList(TransitionInstanceQuery query);
        User GetProcessInitiator(int processInstanceID);

        IList<ActivityInstanceEntity> GetRunningActivityInstance(TaskQuery query);
        IList<TaskViewEntity> GetRunningTasks(TaskQuery query);
        IList<TaskViewEntity> GetReadyTasks(TaskQuery query);
        IList<TaskViewEntity> GetCompletedTasks(TaskQuery query);
        ActivityInstanceEntity GetRunningNode(WfAppRunner runner);
        IList<Performer> GetTaskPerformers(WfAppRunner runner);

        ProcessInstanceEntity GetRunningProcessInstance(WfAppRunner runner);
        ProcessInstanceEntity GetProcessInstance(int processInstanceID);
        ProcessInstanceEntity GetProcessInstanceByActivity(int activityInstanceID);
        Int32 GetProcessInstanceCount(string prcessGUID, string version);
        Boolean IsLastTask(int taskID);
        
        IList<ActivityInstanceEntity> GetActivityInstances(int processInstanceID);
        IList<ActivityInstanceEntity> GetNextActivityInstanceList(int fromActivityInstanceID);
        ActivityInstanceEntity GetActivityInstance(int activityInstanceID);

        ProcessEntity GetProcess(string processGUID);
        ProcessEntity GetProcessByVersion(string processGUID, string version = null);
        IList<ProcessEntity> GetProcessList();
        IList<ProcessEntity> GetProcessListSimple();

        ProcessFileEntity GetProcessFile(string processGUID, string version);
        ProcessFileEntity GetProcessFileByID(int id);
        void SaveProcessFile(ProcessFileEntity entity);

        int CreateProcess(ProcessEntity entity);
        int CreateProcessVersion(ProcessEntity entity);
        void UpdateProcess(ProcessEntity entity);
        void DeleteProcess(string processGUID, string version);
		void DeleteProcess(string processGUID);
        int ImportProcess(ProcessEntity entity);


        //资源接口
        IList<Role> GetRoleAll();
        IList<Role> GetRoleByProcess(string processGUID, string version);
        IList<Role> GetRoleUserListByProcess(string processGUId, string version);
        IList<User> GetUserListByRole(string roleID);
        PerformerList GetPerformerList(NodeView nextNode);


        //#region 链式服务接口
        //IWorkflowService CreateRunner(WfAppRunner runner);
        //IWorkflowService CreateRunner(string userID, string UserName);
        //IWorkflowService UseApp(string appInstanceID, string appName, string appCode = null);
        //IWorkflowService UseProcess(string processGUID, string version);
        //IWorkflowService IfCondition(IDictionary<string, string> conditions);
        //IWorkflowService IfCondition(string name, string value);
        //IWorkflowService Subscribe(EventFireTypeEnum eventType, Func<int, string, IDelegateService, Boolean> func);
        //IWorkflowService NextStep(IDictionary<string, PerformerList> nextActivityPerformers);
        //IWorkflowService NextStep(string activityGUID, PerformerList performerList);

        //WfExecutedResult Start();
        //WfExecutedResult Start(IDbConnection conn, IDbTransaction trans);
        //WfExecutedResult Run();
        //WfExecutedResult Run(IDbConnection conn, IDbTransaction trans);
        //WfExecutedResult Withdraw();
        //WfExecutedResult Withdraw(IDbConnection conn, IDbTransaction trans);
        //WfExecutedResult SendBack();
        //WfExecutedResult SendBack(IDbConnection conn, IDbTransaction trans);

        //WfExecutedResult Jump(JumpOptionEnum jumpOption = JumpOptionEnum.Default);
        //WfExecutedResult Jump(IDbConnection conn, IDbTransaction trans, JumpOptionEnum jumpOption = JumpOptionEnum.Default);
        //WfExecutedResult Reverse();
        //WfExecutedResult Reverse(IDbConnection conn, IDbTransaction trans);
        
        //#endregion
    }
}
