﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Slickflow.Module.Form;
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Engine.Delegate;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Common;

namespace Slickflow.Engine.Service
{
    /// <summary>
    /// Workflow Service Interface
    /// 工作流服务接口
    /// </summary>
    public interface IWorkflowService
    {
        Activity GetFirstActivity(string processID, string version);
        IList<Activity> GetTaskActivityList(string processID, string version);
        IList<Activity> GetTaskActivityList(int processID);
		IList<Activity> GetAllTaskActivityList(string processID, string version);
        Activity GetNextActivity(string processID, string version, string activityID);
        IList<NodeView> GetNextActivity(string processID, string version, string activityID, IDictionary<string, string> condition);
        Activity GetActivityEntity(string processID, string version, string actvityID);
        IList<Role> GetActivityRoles(string processID, string version, string activityID);

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
        SignForwardStepInfo GetSignForwardStepInfo(WfAppRunner runner);

        Task<IList<NodeView>> GetNextActivityTreeAsync(int taskID, IDictionary<string, string> condition = null);
        Task<IList<NodeView>> GetNextActivityTreeAsync(WfAppRunner runner, IDictionary<string, string> condition = null);
        Task<IList<NodeView>> GetNextActivityTreeAsync(IDbConnection conn, WfAppRunner runner, 
            IDictionary<string, string> condition, IDbTransaction trans);
        Task<IList<NodeView>> GetNextActivityRoleUserTreeAsync(WfAppRunner runner, IDictionary<string, string> condition = null);
        Task<NextStepInfo> GetNextStepInfoAsync(WfAppRunner runner, IDictionary<string, string> condition = null);
        Task<IList<NodeView>> GetFirstActivityRoleUserTreeAsync(WfAppRunner runner, IDictionary<string, string> condition = null);
        Task<IList<NodeView>> GetPreviousActivityTreeAsync(WfAppRunner runner);
        Task<PreviousStepInfo> GetPreviousStepInfoAsync(WfAppRunner runner);

        void ResetCache(string processID, string version = null);

        [Obsolete("replaced by the new method RunProcess()")]
        WfExecutedResult RunProcessApp(WfAppRunner runner);
        [Obsolete("replaced by the new method RunProcess()")]
        WfExecutedResult RunProcessApp(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
    
        //启动流程
        //Startup Process
        WfExecutedResult StartProcess(WfAppRunner runner);
        WfExecutedResult StartProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> StartProcessAsync(WfAppRunner runner);
        Task<WfExecutedResult> StartProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //消息启动流程
        //Message Startup Process
        WfExecutedResult StartProcessByMessage(WfAppRunner runner);
        WfExecutedResult StartProcessByMessage(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> StartProcessByMessageAsync(WfAppRunner runner);
        Task<WfExecutedResult> StartProcessByMessageAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //运行流程
        //Run Process
        WfExecutedResult RunProcess(WfAppRunner runner);
        WfExecutedResult RunProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> RunProcessAsync(WfAppRunner runner);
        Task<WfExecutedResult> RunProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //自动运行流程
        //Run Automatically Process
        WfExecutedResult RunProcessAuto(WfAppRunner runner);
        WfExecutedResult RunProcessAuto(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> RunProcessAutoAsync(WfAppRunner runner);
        Task<WfExecutedResult> RunProcessAutoAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //撤销流程
        //Withdraw Process
        WfExecutedResult WithdrawProcess(WfAppRunner runner);
        WfExecutedResult WithdrawProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> WithdrawProcessAsync(WfAppRunner runner);
        Task<WfExecutedResult> WithdrawProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //退回流程
        //SendBack Process
        WfExecutedResult SendBackProcess(WfAppRunner runner);
        WfExecutedResult SendBackProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> SendBackProcessAsync(WfAppRunner runner);
        Task<WfExecutedResult> SendBackProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //返送流程
        //Resend Process
        WfExecutedResult ResendProcess(WfAppRunner runner);
        WfExecutedResult ResendProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> ResendProcessAsync(WfAppRunner runner);
        Task<WfExecutedResult> ResendProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //修订流程
        //Revise Process
        WfExecutedResult ReviseProcess(WfAppRunner runner);
        WfExecutedResult ReviseProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> ReviseProcessAsync(WfAppRunner runner);
        Task<WfExecutedResult> ReviseProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //驳回流程
        //Reject Process
        WfExecutedResult RejectProcess(WfAppRunner runner);
        WfExecutedResult RejectProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> RejectProcessAsync(WfAppRunner runner);
        Task<WfExecutedResult> RejectProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //关闭流程
        //Close Process
        WfExecutedResult CloseProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        WfExecutedResult CloseProcess(WfAppRunner runner);
        Task<WfExecutedResult> CloseProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> CloseProcessAsync(WfAppRunner runner);

        //返签流程
        //Reverse Process
        WfExecutedResult ReverseProcess(WfAppRunner runner);
        WfExecutedResult ReverseProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> ReverseProcessAsync(WfAppRunner runner);
        Task<WfExecutedResult> ReverseProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //跳转流程
        //Jump Process
        WfExecutedResult JumpProcess(WfAppRunner runner, JumpOptionEnum jumpOption = JumpOptionEnum.Default);
        WfExecutedResult JumpProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans, 
            JumpOptionEnum jumpOption = JumpOptionEnum.Default);
        Task<WfExecutedResult> JumpProcessAsync(WfAppRunner runner, JumpOptionEnum jumpOption = JumpOptionEnum.Default);
        Task<WfExecutedResult> JumpProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans, 
            JumpOptionEnum jumpOption = JumpOptionEnum.Default);

        //加签流程
        //Sign Forward Process
        WfExecutedResult SignForwardProcess(WfAppRunner runner);
        WfExecutedResult SignForwardProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> SignForwardProcessAsync(WfAppRunner runner);
        Task<WfExecutedResult> SignForwardProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //审批决策
        //Approval
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
        TaskViewEntity GetTaskView(IDbConnection conn, string appInstanceID, string processID, string userID, IDbTransaction trans);
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
        ProcessEntity GetProcessUsing(string processID);
        ProcessEntity GetProcessByVersion(string processID, string version);
        ProcessEntity GetProcessByName(string processName, string version = null);
        ProcessEntity GetProcessByCode(string processCode, string version = null);
        IList<ProcessEntity> GetProcessList();
        IList<ProcessEntity> GetProcessListSimple();

        ProcessFileEntity GetProcessFile(string processID, string version);
        ProcessFileEntity GetProcessFileByID(int id);
        void SaveProcessFile(ProcessFileEntity entity);

        int CreateProcess(ProcessEntity entity);
        int CreateProcessVersion(ProcessEntity entity);
        ProcessFileEntity CreateProcessByXML(string xmlContent);
        int InsertProcess(ProcessEntity entity);
        void UpdateProcess(ProcessEntity entity);
        void UpdateProcessUsingState(string processID, string version, byte usingState);
        void UpgradeProcess(string processID, string version, string newVersion);
        void DeleteProcess(string processID, string version);
		void DeleteProcess(string processID);
        bool DeleteInstanceInt(string processID, string version);
        void ImportProcess(string xmlContent);
        ProcessValidateResult ValidateProcess(ProcessEntity processValidateEntity);
        

        //角色资源接口
        //Role Resource Interfaces
        IList<Role> GetRoleAll();
        IList<Role> GetRoleByProcess(string processID, string version);
        IList<Role> GetRoleUserListByProcess(string processID, string version);
        IList<User> GetUserAll();
        IList<User> GetUserListByRole(string roleID);
        PerformerList GetPerformerList(NodeView nextNode);

        //流程变量接口
        //Process Variables Interfaces
        IList<ProcessVariableEntity> GetProcessVariableList(ProcessVariableQuery query);
        ProcessVariableEntity GetProcessVariable(ProcessVariableQuery query);
        ProcessVariableEntity GetProcessVariable(int variableID);
        Boolean ValidateProcessVariable(int processInstanceID, string expression);
        void DeleteProcessVariable(int variableID);

        #region Chain Service Interface
        IWorkflowService CreateRunner(WfAppRunner runner);
        IWorkflowService CreateRunner(string userID, string UserName);
        IWorkflowService UseApp(string appInstanceID, string appName, string appCode = null);
        IWorkflowService UseProcess(string processCodeOrProcessID, string version = null);
        IWorkflowService IfCondition(IDictionary<string, string> conditions);
        IWorkflowService IfCondition(string name, string value);
        IWorkflowService OnTask(int taskID);
        IWorkflowService SetVariable(string name, string value);
        IWorkflowService SetVariable(IDictionary<string, string> variables);
        IWorkflowService Subscribe(EventFireTypeEnum eventType, Func<DelegateContext, IDelegateService, Boolean> func);
        IWorkflowService NextStep(string activityID, PerformerList performerList);
        IWorkflowService NextStep(IDictionary<string, PerformerList> nextActivityPerformers);
        IWorkflowService NextStepInt(string userID, string userName);
        IWorkflowService NextStepInt(PerformerList performerList);
        IWorkflowService PrevStepInt();

        //流转相关接口
        //Flow related method
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
        WfExecutedResult Resend();
        WfExecutedResult Resend(IDbConnection conn, IDbTransaction trans);
        WfExecutedResult Revise();
        WfExecutedResult Revise(IDbConnection conn, IDbTransaction trans);
        WfExecutedResult Jump(JumpOptionEnum jumpOption = JumpOptionEnum.Default);
        WfExecutedResult Jump(IDbConnection conn, IDbTransaction trans, JumpOptionEnum jumpOption = JumpOptionEnum.Default);
        WfExecutedResult Reverse();
        WfExecutedResult Reverse(IDbConnection conn, IDbTransaction trans);
        WfExecutedResult Reject();
        WfExecutedResult Reject(IDbConnection conn, IDbTransaction trans);
        WfExecutedResult Close();
        WfExecutedResult Close(IDbConnection conn, IDbTransaction trans);
        WfExecutedResult SignForward();
        WfExecutedResult SignForward(IDbConnection conn, IDbTransaction trans);
        #endregion
    }
}
;