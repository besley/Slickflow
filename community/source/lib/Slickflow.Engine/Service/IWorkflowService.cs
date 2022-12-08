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
using System.Data;
using System.Threading.Tasks;
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Engine.Delegate;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Xpdl.Entity;


namespace Slickflow.Engine.Service
{
    /// <summary>
    /// 工作流服务接口
    /// </summary>
    public interface IWorkflowService
    {
        Activity GetFirstActivity(string processGUID, string version);
        IList<Activity> GetTaskActivityList(string processGUID, string version);
        IList<Activity> GetTaskActivityList(int processID);
		IList<Activity> GetAllTaskActivityList(string processGUID, string version);
        Activity GetNextActivity(string processGUID, string version, string activityGUID);
        IList<NodeView> GetNextActivity(string processGUID, string version, string activityGUID, IDictionary<string, string> condition);
        Activity GetActivityEntity(string processGUID, string version, string actvityGUID);
        IList<Role> GetActivityRoles(string processGUID, string version, string activityGUID);

        NodeView GetNextActivity(int taskID, IDictionary<string, string> condition = null);
        NodeView GetNextActivity(WfAppRunner runner, IDictionary<string, string> condition = null);
        IList<NodeView> GetNextActivityTree(int taskID, IDictionary<string, string> condition = null);
        IList<NodeView> GetNextActivityTree(WfAppRunner runner, IDictionary<string, string> condition = null);
        IList<NodeView> GetNextActivityTree(IDbConnection conn, WfAppRunner runner, 
            IDictionary<string, string> condition, IDbTransaction trans);
        IList<NodeView> GetNextActivityRoleUserTree(WfAppRunner runner, IDictionary<string, string> condition = null);
        NextStepInfo GetNextStepInfo(WfAppRunner runner, IDictionary<string, string> condition = null);
        IList<NodeView> GetFirstActivityRoleUserTree(WfAppRunner runner, IDictionary<string, string> condition = null);
        IList<NodeView> GetPreviousActivityTree(WfAppRunner runner);
        PreviousStepInfo GetPreviousStepInfo(WfAppRunner runner);

        Task<IList<NodeView>> GetNextActivityTreeAsync(int taskID, IDictionary<string, string> condition = null);
        Task<IList<NodeView>> GetNextActivityTreeAsync(WfAppRunner runner, IDictionary<string, string> condition = null);
        Task<IList<NodeView>> GetNextActivityTreeAsync(IDbConnection conn, WfAppRunner runner, 
            IDictionary<string, string> condition, IDbTransaction trans);
        Task<IList<NodeView>> GetNextActivityRoleUserTreeAsync(WfAppRunner runner, IDictionary<string, string> condition = null);
        Task<NextStepInfo> GetNextStepInfoAsync(WfAppRunner runner, IDictionary<string, string> condition = null);
        Task<IList<NodeView>> GetFirstActivityRoleUserTreeAsync(WfAppRunner runner, IDictionary<string, string> condition = null);
        Task<IList<NodeView>> GetPreviousActivityTreeAsync(WfAppRunner runner);
        Task<PreviousStepInfo> GetPreviousStepInfoAsync(WfAppRunner runner);

        void ResetCache(string processGUID, string version = null);


        [Obsolete("replaced by the new method RunProcess()")]
        WfExecutedResult RunProcessApp(WfAppRunner runner);
        [Obsolete("replaced by the new method RunProcess()")]
        WfExecutedResult RunProcessApp(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
    
        //启动流程
        WfExecutedResult StartProcess(WfAppRunner runner);
        WfExecutedResult StartProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> StartProcessAsync(WfAppRunner runner);
        Task<WfExecutedResult> StartProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //消息启动流程
        WfExecutedResult StartProcessByMessage(WfAppRunner runner);
        WfExecutedResult StartProcessByMessage(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> StartProcessByMessageAsync(WfAppRunner runner);
        Task<WfExecutedResult> StartProcessByMessageAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //运行流程
        WfExecutedResult RunProcess(WfAppRunner runner);
        WfExecutedResult RunProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> RunProcessAsync(WfAppRunner runner);
        Task<WfExecutedResult> RunProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //撤销流程
        WfExecutedResult WithdrawProcess(WfAppRunner runner);
        WfExecutedResult WithdrawProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> WithdrawProcessAsync(WfAppRunner runner);
        Task<WfExecutedResult> WithdrawProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //退回流程
        WfExecutedResult SendBackProcess(WfAppRunner runner);
        WfExecutedResult SendBackProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> SendBackProcessAsync(WfAppRunner runner);
        Task<WfExecutedResult> SendBackProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //返签流程
        WfExecutedResult ReverseProcess(WfAppRunner runner);
        WfExecutedResult ReverseProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> ReverseProcessAsync(WfAppRunner runner);
        Task<WfExecutedResult> ReverseProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //跳转流程
        WfExecutedResult JumpProcess(WfAppRunner runner, JumpOptionEnum jumpOption = JumpOptionEnum.Default);
        WfExecutedResult JumpProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans, 
            JumpOptionEnum jumpOption = JumpOptionEnum.Default);
        Task<WfExecutedResult> JumpProcessAsync(WfAppRunner runner, JumpOptionEnum jumpOption = JumpOptionEnum.Default);
        Task<WfExecutedResult> JumpProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans, 
            JumpOptionEnum jumpOption = JumpOptionEnum.Default);

        //审批决策
        void AgreeTask(int taskID);
        void RefuseTask(int taskID);

        Boolean ResumeProcess(int activityInstanceId, WfAppRunner runner);
        Boolean SuspendProcess(int taskId, WfAppRunner runner);
        Boolean CancelProcess(WfAppRunner canceler);
        Boolean DiscardProcess(WfAppRunner discarder);
        Boolean TerminateProcess(WfAppRunner terminator);
        Boolean TerminateProcess(IDbConnection conn, ProcessInstanceEntity entity, string userID, string userName, IDbTransaction trans);
        Boolean SetTaskRead(WfAppRunner runner);
        Boolean SetTaskEMailSent(int taskID);
        Boolean EntrustTask(TaskEntrustedEntity entrusted, bool cancalOriginalTask = true);
        Boolean ReplaceTask(int taskID, List<TaskReplacedEntity> replaced, WfAppRunner runner);
        Boolean SetProcessOverdue(int processInstanceID, DateTime overdueDateTime, WfAppRunner runner);
        void SetActivityJobTimerCompleted(IDbConnection conn, int activityInstanceID, IDbTransaction trans);
        void SetProcessJobTimerCompleted(IDbConnection conn, int processInstanceID, IDbTransaction trans);
        int SaveProcessVariable(ProcessVariableEntity entity);
        
        IList<NodeImage> GetActivityInstanceCompleted(int taskID);
        IList<NodeImage> GetActivityInstanceCompleted(WfAppRunner runner);
        IList<TransitionImage> GetTransitionInstanceList(TransitionInstanceQuery query);
        User GetProcessInitiator(int processInstanceID);

        IList<ActivityInstanceEntity> GetRunningActivityInstance(TaskQuery query);
        TaskViewEntity GetTaskView(int taskID);
        TaskViewEntity GetTaskView(int processInstanceID, int activityInstanceID);
        IList<TaskViewEntity> GetRunningTasks(TaskQuery query);
        IList<TaskViewEntity> GetReadyTasks(TaskQuery query);
        IList<TaskViewEntity> GetCompletedTasks(TaskQuery query);
        IList<TaskViewEntity> GetTaskListEMailUnSent();
        [Obsolete("replaced by the GetRunningTask, mainly used to run forward method internal")]
        ActivityInstanceEntity GetRunningNode(WfAppRunner runner);
        TaskViewEntity GetFirstRunningTask(int activityInstanceID);
        IList<Performer> GetTaskPerformers(WfAppRunner runner);

        ProcessInstanceEntity GetRunningProcessInstance(WfAppRunner runner);
        ProcessInstanceEntity GetProcessInstance(int processInstanceID);
        ProcessInstanceEntity GetProcessInstance(IDbConnection conn, int processInstanceID, IDbTransaction trans);
        IList<ProcessInstanceEntity> GetProcessInstance(string appInstanceID);
        ProcessInstanceEntity GetProcessInstanceByActivity(int activityInstanceID);
        Int32 GetProcessInstanceCount(string prcessGUID, string version);
        Boolean IsLastTask(int taskID);
        void UpdateProcessInstance(ProcessInstanceEntity entity);

        IList<ActivityInstanceEntity> GetActivityInstances(int processInstanceID);
        IList<ActivityInstanceEntity> GetTargetActivityInstanceList(int fromActivityInstanceID);
        ActivityInstanceEntity GetActivityInstance(int activityInstanceID);

        ProcessEntity GetProcessByID(int processID);
        ProcessEntity GetProcessUsing(string processGUID);
        ProcessEntity GetProcessByVersion(string processGUID, string version);
        ProcessEntity GetProcessByName(string processName, string version = null);
        ProcessEntity GetProcessByCode(string processCode, string version = null);
        IList<ProcessEntity> GetProcessList();
        IList<ProcessEntity> GetProcessListSimple();

        ProcessFileEntity GetProcessFile(string processGUID, string version);
        ProcessFileEntity GetProcessFileByID(int id);
        void SaveProcessFile(ProcessFileEntity entity);
        void SaveProcessFilePool(ProcessFilePool entity);

        int CreateProcess(ProcessEntity entity);
        int CreateProcessVersion(ProcessEntity entity);
        int InsertProcess(ProcessEntity entity);
        void UpdateProcess(ProcessEntity entity);
        void UpgradeProcess(string processGUID, string version, string newVersion);
        void DeleteProcess(string processGUID, string version);
		void DeleteProcess(string processGUID);
        bool DeleteInstanceInt(string processGUID, string version);
        int ImportProcess(ProcessEntity entity);
        ProcessValidateResult ValidateProcess(ProcessValidate processValidateEntity);
        

        //资源接口
        IList<Role> GetRoleAll();
        IList<Role> GetRoleByProcess(string processGUID, string version);
        IList<Role> GetRoleUserListByProcess(string processGUId, string version);
        IList<User> GetUserListByRole(string roleID);
        PerformerList GetPerformerList(NodeView nextNode);
        IList<ProcessVariableEntity> GetProcessVariableList(ProcessVariableQuery query);
        ProcessVariableEntity GetProcessVariable(ProcessVariableQuery query);
        ProcessVariableEntity GetProcessVariable(int variableID);
        Boolean ValidateProcessVariable(int processInstanceID, string expression);
        void DeleteProcessVariable(int variableID);


        #region 链式服务接口
        //创建方法
        IWorkflowService CreateRunner(WfAppRunner runner);
        IWorkflowService CreateRunner(string userID, string UserName);
        IWorkflowService UseApp(string appInstanceID, string appName, string appCode = null);
        IWorkflowService UseProcess(string processCodeOrProcessGUID, string version = null);
        IWorkflowService IfCondition(IDictionary<string, string> conditions);
        IWorkflowService IfCondition(string name, string value);
        IWorkflowService OnTask(int taskID);
        IWorkflowService SetVariable(string name, string value);
        IWorkflowService SetVariable(IDictionary<string, string> variables);
        IWorkflowService Subscribe(EventFireTypeEnum eventType, Func<DelegateContext, IDelegateService, Boolean> func);
        IWorkflowService NextStep(string activityGUID, PerformerList performerList);
        IWorkflowService NextStep(IDictionary<string, PerformerList> nextActivityPerformers);
        IWorkflowService NextStepInt(string userID, string userName);
        IWorkflowService NextStepInt(PerformerList performerList);
        IWorkflowService PrevStepInt();

        //流传方法
        WfExecutedResult Start();
        WfExecutedResult Start(IDbConnection conn, IDbTransaction trans);
        WfExecutedResult StartByMessage();
        WfExecutedResult StartByMessage(IDbConnection conn, IDbTransaction trans);
        WfExecutedResult Run();
        WfExecutedResult Run(IDbConnection conn, IDbTransaction trans);
        WfExecutedResult Withdraw();
        WfExecutedResult Withdraw(IDbConnection conn, IDbTransaction trans);
        WfExecutedResult SendBack();
        WfExecutedResult SendBack(IDbConnection conn, IDbTransaction trans);
        WfExecutedResult Jump(JumpOptionEnum jumpOption = JumpOptionEnum.Default);
        WfExecutedResult Jump(IDbConnection conn, IDbTransaction trans, JumpOptionEnum jumpOption = JumpOptionEnum.Default);
        WfExecutedResult Reverse();
        WfExecutedResult Reverse(IDbConnection conn, IDbTransaction trans);
        #endregion
    }
}
;