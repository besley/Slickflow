using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using System;
using System.Threading.Tasks;

namespace Slickflow.Engine.Executor
{
    /// <summary>
    /// Workflow executor with chain-style API. Delegates graph run to WorkflowNodeRunner and activity execution to WorkflowActivityExecutor.
    /// 链式工作流执行器：图遍历委托给 WorkflowNodeRunner，节点执行委托给 WorkflowActivityExecutor。
    /// </summary>
    public class WorkflowExecutor : IWorkflowExecutor
    {
        #region Property
        private WfAppRunner _wfAppRunner = new WfAppRunner();
        private IProcessModel _processModel;
        private AutoExecutionContext _autoExecutionContext = new AutoExecutionContext();
        private string _notifyClientSessionId;
        private Action<string, object> _notifyClientCallback;
        #endregion

        public WorkflowExecutor()
        {
            _wfAppRunner.NextPerformerType = NextPerformerIntTypeEnum.Unattented;
        }

        /// <summary>
        /// User application / 绑定业务票据
        /// </summary>
        public IWorkflowExecutor UseApp(string appInstanceId, string appName, string appCode = null)
        {
            _wfAppRunner.AppInstanceId = appInstanceId;
            _wfAppRunner.AppName = appName;
            _wfAppRunner.AppInstanceCode = appCode;
            return this;
        }

        /// <summary>
        /// Use Process Definition / 使用流程定义
        /// </summary>
        public IWorkflowExecutor UseProcess(string processId, string version = null)
        {
            if (string.IsNullOrEmpty(version)) version = "1";

            Guid newGUID = Guid.Empty;
            bool isProcessId = Guid.TryParse(processId, out newGUID);
            if (isProcessId == true)
            {
                _wfAppRunner.ProcessId = processId;
                _wfAppRunner.Version = version;
            }
            else
            {
                var pm = new ProcessManager();
                var entity = pm.GetByVersion(processId, version);
                _wfAppRunner.ProcessId = entity.ProcessId;
                _wfAppRunner.Version = entity.Version;
            }

            _processModel = ProcessModelFactory.CreateByProcess(processId, version);

            return this;
        }

        /// <summary>
        /// Use Process Definition from an in-memory ProcessEntity (no database lookup).
        /// </summary>
        public IWorkflowExecutor UseProcess(ProcessEntity processEntity)
        {
            if (processEntity == null)
                throw new ArgumentNullException(nameof(processEntity));

            _wfAppRunner.ProcessId = processEntity.ProcessId;
            _wfAppRunner.Version = processEntity.Version;

            _processModel = ProcessModelFactory.CreateByProcess(processEntity);

            return this;
        }

        /// <summary>
        /// Add process variable / 添加流程变量
        /// </summary>
        public IWorkflowExecutor AddVariable(string name, string value)
        {
            _autoExecutionContext.Variables.Add(name, value);
            return this;
        }

        /// <summary>
        /// Set callback to push RAG node response to client when isNotifyClient is true.
        /// 当节点扩展属性 isNotifyClient 为 true 时，将 RAG 节点结果推送到前端。
        /// </summary>
        public IWorkflowExecutor SetNotifyClient(string sessionId, Action<string, object> callback)
        {
            _notifyClientSessionId = sessionId;
            _notifyClientCallback = callback;
            return this;
        }

        /// <summary>
        /// Inject delegate registry for LocalMethod ServiceTask. When null, ServiceTaskDelegateRegistry.Global is used.
        /// 注入委托注册表，用于 LocalMethod 类型的 ServiceTask。为空时使用全局注册表。
        /// </summary>
        public IWorkflowExecutor WithDelegateRegistry(IServiceTaskDelegateRegistry registry)
        {
            _autoExecutionContext.DelegateRegistry = registry;
            return this;
        }

        /// <summary>
        /// Execute workflow automatically from start to end / 自动执行流程从开始到结束
        /// </summary>
        public Task<WfExecutedResult> Run()
        {
            var result = new WfExecutedResult
            {
                Status = WfExecutedStatus.Success
            };

            try
            {
                var startActivity = _processModel.GetStartActivity();
                if (startActivity == null)
                {
                    result.Status = WfExecutedStatus.Failed;
                    result.Message = "There isnt start activity in the workflow diagram";
                    return Task.FromResult(result);
                }

                _autoExecutionContext.ProcessModel = _processModel;
                _autoExecutionContext.Runner = _wfAppRunner;

                var activityExecutor = new WorkflowActivityExecutor(_notifyClientSessionId, _notifyClientCallback);
                var nodeRunner = new WorkflowNodeRunner(_processModel, activityExecutor);
                nodeRunner.Run(startActivity, _autoExecutionContext, result);

                if (result.Status == WfExecutedStatus.Success)
                {
                    result.Message = "Workflow executed successfully";
                    if (_autoExecutionContext.Variables != null && _autoExecutionContext.Variables.TryGetValue("ai_response", out var aiResp) && aiResp != null && !string.IsNullOrWhiteSpace(aiResp.ToString()))
                        result.AiResponse = aiResp.ToString();
                }
            }
            catch (Exception ex)
            {
                result.Status = WfExecutedStatus.Failed;
                result.Message = $"Auto orchestration execution failed: {ex.Message}";
                LogManager.RecordLog(WfDefine.WF_WORKFLOW_EXECUTOR_RUN_ERROR, LogEventType.Error, LogPriority.High, _wfAppRunner, ex);
            }

            return Task.FromResult(result);
        }
    }
}
