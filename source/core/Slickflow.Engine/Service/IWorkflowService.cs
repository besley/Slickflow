using System;
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
using Slickflow.Engine.Business.Result;

namespace Slickflow.Engine.Service
{
    /// <summary>
    /// Workflow Service Interface
    /// 工作流服务接口
    /// </summary>
    public interface IWorkflowService
    {
        Activity GetFirstActivity(string processId, string version);
        Activity GetActivityEntity(string processId, string version, string actvityId);
        Activity GetNextActivity(string processId, string version, string activityId);
        IList<Activity> GetTaskActivityList(string processId, string version);
        IList<Activity> GetTaskActivityList(int processId);
		IList<Activity> GetAllTaskActivityList(string processId, string version);
        IList<Role> GetActivityRoles(string processId, string version, string activityId);
        NodeView GetNextActivity(int taskId, IDictionary<string, string> condition = null);
        NodeView GetNextActivity(WfAppRunner runner, IDictionary<string, string> condition = null);
        IList<NodeView> GetNextActivityTree(string processId, string version, string activityId, IDictionary<string, string> condition);

        IList<NodeView> GetNextActivityTree(int taskId, IDictionary<string, string> condition = null);
        Task<IList<NodeView>> GetNextActivityTreeAsync(int taskId, IDictionary<string, string> condition = null);

        IList<NodeView> GetNextActivityTree(WfAppRunner runner, IDictionary<string, string> condition = null);
        Task<IList<NodeView>> GetNextActivityTreeAsync(WfAppRunner runner, IDictionary<string, string> condition = null);

        IList<NodeView> GetNextActivityTree(IDbConnection conn, WfAppRunner runner, 
            IDictionary<string, string> condition, IDbTransaction trans);
        Task<IList<NodeView>> GetNextActivityTreeAsync(IDbConnection conn, WfAppRunner runner,
            IDictionary<string, string> condition, IDbTransaction trans);

        IList<NodeView> GetNextActivityRoleUserTree(WfAppRunner runner, IDictionary<string, string> condition = null);
        Task<IList<NodeView>> GetNextActivityRoleUserTreeAsync(WfAppRunner runner, IDictionary<string, string> condition = null);

        /// <summary>
        /// Business-level API for obtaining the next-step information, including structure, roles and users.
        /// This is the recommended single entry point for UI / application code to query next steps.
        /// 业务层获取“下一步”信息的统一接口，返回结构、角色与用户等完整信息。
        /// 推荐界面层与业务服务统一通过此方法获取下一步，而不要直接依赖 IProcessModel 的结构层方法。
        /// </summary>
        NextStepInfo GetNextStepInfo(WfAppRunner runner, IDictionary<string, string> condition = null);
        Task<NextStepInfo> GetNextStepInfoAsync(WfAppRunner runner, IDictionary<string, string> condition = null);

        IList<NodeView> GetFirstActivityRoleUserTree(WfAppRunner runner, IDictionary<string, string> condition = null);
        Task<IList<NodeView>> GetFirstActivityRoleUserTreeAsync(WfAppRunner runner, IDictionary<string, string> condition = null);

        IList<NodeView> GetPreviousActivityTree(WfAppRunner runner);
        Task<IList<NodeView>> GetPreviousActivityTreeAsync(WfAppRunner runner);

        PreviousStepInfo GetPreviousStepInfo(WfAppRunner runner);
        Task<PreviousStepInfo> GetPreviousStepInfoAsync(WfAppRunner runner);

        SignForwardStepInfo GetSignForwardStepInfo(WfAppRunner runner);

        void ResetCache(string processId, string version = null);

        [Obsolete("replaced by the new method RunProcess()")]
        WfExecutedResult RunProcessApp(WfAppRunner runner);
        [Obsolete("replaced by the new method RunProcess()")]
        WfExecutedResult RunProcessApp(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
    
        //启动流程
        //Startup Process
        WfExecutedResult StartProcess(WfAppRunner runner);
        Task<WfExecutedResult> StartProcessAsync(WfAppRunner runner);

        WfExecutedResult StartProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> StartProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //消息启动流程
        //Message Startup Process
        WfExecutedResult StartProcessByMessage(WfAppRunner runner);
        Task<WfExecutedResult> StartProcessByMessageAsync(WfAppRunner runner);

        WfExecutedResult StartProcessByMessage(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> StartProcessByMessageAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //运行流程
        //Run Process
        WfExecutedResult RunProcess(WfAppRunner runner);
        Task<WfExecutedResult> RunProcessAsync(WfAppRunner runner);
        WfExecutedResult RunProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> RunProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //自动运行流程
        //Run Automatically Process
        WfExecutedResult RunProcessAuto(WfAppRunner runner);
        Task<WfExecutedResult> RunProcessAutoAsync(WfAppRunner runner);
        WfExecutedResult RunProcessAuto(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> RunProcessAutoAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //撤销流程
        //Withdraw Process
        WfExecutedResult WithdrawProcess(WfAppRunner runner);
        Task<WfExecutedResult> WithdrawProcessAsync(WfAppRunner runner);
        WfExecutedResult WithdrawProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> WithdrawProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //退回流程
        //SendBack Process
        WfExecutedResult SendBackProcess(WfAppRunner runner);
        Task<WfExecutedResult> SendBackProcessAsync(WfAppRunner runner);
        WfExecutedResult SendBackProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> SendBackProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //返送流程
        //Resend Process
        WfExecutedResult ResendProcess(WfAppRunner runner);
        Task<WfExecutedResult> ResendProcessAsync(WfAppRunner runner);
        WfExecutedResult ResendProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> ResendProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //修订流程
        //Revise Process
        WfExecutedResult ReviseProcess(WfAppRunner runner);
        Task<WfExecutedResult> ReviseProcessAsync(WfAppRunner runner);
        WfExecutedResult ReviseProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> ReviseProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //驳回流程
        //Reject Process
        WfExecutedResult RejectProcess(WfAppRunner runner);
        Task<WfExecutedResult> RejectProcessAsync(WfAppRunner runner);
        WfExecutedResult RejectProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> RejectProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //关闭流程
        //Close Process
        WfExecutedResult CloseProcess(WfAppRunner runner);
        Task<WfExecutedResult> CloseProcessAsync(WfAppRunner runner);

        WfExecutedResult CloseProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> CloseProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //返签流程
        //Reverse Process
        WfExecutedResult ReverseProcess(WfAppRunner runner);
        Task<WfExecutedResult> ReverseProcessAsync(WfAppRunner runner);
        WfExecutedResult ReverseProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> ReverseProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //跳转流程
        //Jump Process
        WfExecutedResult JumpProcess(WfAppRunner runner, JumpOptionEnum jumpOption = JumpOptionEnum.Default);
        Task<WfExecutedResult> JumpProcessAsync(WfAppRunner runner, JumpOptionEnum jumpOption = JumpOptionEnum.Default);
        WfExecutedResult JumpProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans, 
            JumpOptionEnum jumpOption = JumpOptionEnum.Default);
        Task<WfExecutedResult> JumpProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans, 
            JumpOptionEnum jumpOption = JumpOptionEnum.Default);

        //加签流程
        //Sign Forward Process
        WfExecutedResult SignForwardProcess(WfAppRunner runner);
        Task<WfExecutedResult> SignForwardProcessAsync(WfAppRunner runner);
        WfExecutedResult SignForwardProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        Task<WfExecutedResult> SignForwardProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);

        //审批决策
        //Approval
        void AgreeTask(int taskId);
        void RefuseTask(int taskId);

        Boolean ResumeProcess(int activityInstanceId, WfAppRunner runner);
        Boolean SuspendProcess(int taskId, WfAppRunner runner);
        Boolean CancelProcess(WfAppRunner canceler);
        Boolean DiscardProcess(WfAppRunner discarder);
        Boolean TerminateProcess(WfAppRunner terminator);
        Boolean TerminateProcess(IDbConnection conn, ProcessInstanceEntity entity, string userId, string userName, IDbTransaction trans);
        Boolean SetTaskRead(WfAppRunner runner);
        Boolean SetTaskEMailSent(int taskId);
        WfDataManagedResult EntrustTask(TaskEntrustedEntity entrusted, bool cancalOriginalTask = true);
        void CancelEntrustedTask(int id);
        Boolean ReplaceTask(int taskId, List<TaskReplacedEntity> replaced, WfAppRunner runner);
        Boolean SetProcessOverdue(int processInstanceId, DateTime overdueDateTime, WfAppRunner runner);
        void SetActivityJobTimerCompleted(IDbConnection conn, int activityInstanceId, IDbTransaction trans);
        void SetProcessJobTimerCompleted(IDbConnection conn, int processInstanceId, IDbTransaction trans);
        int SaveProcessVariable(ProcessVariableEntity entity);
        
        IList<NodeImage> GetActivityInstanceCompleted(int taskId);
        IList<NodeImage> GetActivityInstanceCompleted(WfAppRunner runner);
        IList<TransitionImage> GetTransitionInstanceList(TransitionInstanceQuery query);
        User GetProcessInitiator(int processInstanceId);

        IList<ActivityInstanceEntity> GetRunningActivityInstance(TaskQuery query);
        TaskViewEntity GetTaskView(int taskId);
        TaskViewEntity GetTaskView(int processInstanceId, int activityInstanceId);
        TaskViewEntity GetTaskView(IDbConnection conn, string appInstanceId, string processId, string userId, IDbTransaction trans);
        IList<TaskViewEntity> GetRunningTasks(TaskQuery query);
        IList<TaskViewEntity> GetReadyTasks(TaskQuery query);
        IList<TaskViewEntity> GetCompletedTasks(TaskQuery query);
        IList<TaskViewEntity> GetTaskListEMailUnSent();
        [Obsolete("replaced by the GetRunningTask, mainly used to run forward method internal")]
        ActivityInstanceEntity GetRunningNode(WfAppRunner runner);
        TaskViewEntity GetFirstRunningTask(int activityInstanceId);
        IList<Performer> GetTaskPerformers(WfAppRunner runner);

        ProcessInstanceEntity GetRunningProcessInstance(WfAppRunner runner);
        ProcessInstanceEntity GetProcessInstance(int processInstanceId);
        ProcessInstanceEntity GetProcessInstance(IDbConnection conn, int processInstanceId, IDbTransaction trans);
        IList<ProcessInstanceEntity> GetProcessInstance(string appInstanceId);
        ProcessInstanceEntity GetProcessInstanceByActivity(int activityInstanceId);
        Int32 GetProcessInstanceCount(string prcessId, string version);
        Boolean IsLastTask(int taskId);
        void UpdateProcessInstance(ProcessInstanceEntity entity);

        IList<ActivityInstanceEntity> GetActivityInstanceList(int processInstanceId);
        IList<ActivityInstanceEntity> GetTargetActivityInstanceList(int fromActivityInstanceId);
        ActivityInstanceEntity GetActivityInstance(int activityInstanceId);

        ProcessEntity GetProcessById(int processId);
        ProcessEntity GetProcessUsing(string processId);
        ProcessEntity GetProcessByVersion(string processId, string version);
        ProcessEntity GetProcessByName(string processName, string version = null);
        ProcessEntity GetProcessByCode(string processCode, string version = null);
        IList<ProcessEntity> GetProcessList();
        IList<ProcessEntity> GetProcessListSimple();

        ProcessFileEntity GetProcessFile(string processId, string version);
        ProcessFileEntity GetProcessFileById(int id);
        void SaveProcessFile(ProcessFileEntity entity);

        int CreateProcess(ProcessEntity entity);
        int CreateProcessVersion(ProcessEntity entity);
        ProcessFileEntity CreateProcessByXML(string xmlContent);
        int InsertProcess(ProcessEntity entity);
        void UpdateProcess(ProcessEntity entity);
        void UpdateProcessUsingState(string processId, string version, byte usingState);
        void UpgradeProcess(string processId, string version, string newVersion);
        void DeleteProcess(string processId, string version);
		void DeleteProcess(int id);
        bool DeleteInstanceInt(string processId, string version);
        void ImportProcess(string xmlContent);
        ProcessValidateResult ValidateProcess(ProcessEntity processValidateEntity);
        

        //角色资源接口
        //Role Resource Interfaces
        IList<Role> GetRoleAll();
        IList<Role> GetRoleByProcess(string processId, string version);
        IList<Role> GetRoleUserListByProcess(string processId, string version);
        IList<User> GetUserAll();
        IList<User> GetUserListByRole(string roleId);
        PerformerList GetPerformerList(NodeView nextNode);

        //表单资源接口
        //Form Resource Interfaces
        IList<Form> GetFormAll();
        IList<Form> GetFormList(string processId, string version);
        IList<Form> GetFormFieldEditComp(FieldQuery query);

        //流程变量接口
        //Process Variables Interfaces
        IList<ProcessVariableEntity> GetProcessVariableList(ProcessVariableQuery query);
        ProcessVariableEntity GetProcessVariable(ProcessVariableQuery query);
        ProcessVariableEntity GetProcessVariable(int variableId);
        Boolean ValidateProcessVariable(int processInstanceId, string expression);
        void DeleteProcessVariable(int variableId);

        #region Chain Service Interface
        IWorkflowService CreateRunner(WfAppRunner runner);
        IWorkflowService CreateRunner(string userId, string userName);
        IWorkflowService UseApp(string appInstanceId, string appName, string appCode = null);
        IWorkflowService UseProcess(string processCodeOrProcessId, string version = null);
        IWorkflowService IfCondition(IDictionary<string, string> conditions);
        IWorkflowService IfCondition(string name, string value);
        IWorkflowService OnTask(int taskId);
        IWorkflowService SetVariable(string name, string value);
        IWorkflowService SetVariable(IDictionary<string, string> variables);
        IWorkflowService Subscribe(EventFireTypeEnum eventType, Func<DelegateContext, IDelegateService, Boolean> func);
        IWorkflowService NextStep(string activityId, PerformerList performerList);
        IWorkflowService NextStep(IDictionary<string, PerformerList> nextActivityPerformers);
        IWorkflowService NextStepInt(string userId, string userName);
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