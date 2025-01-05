using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Delegate;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Service
{
    /// <summary>
    /// Chain interface service class
    /// 链式接口服务类
    /// </summary>
    public partial class WorkflowService : IWorkflowService
    {
        #region Property
        private WfAppRunner _wfAppRunner = new WfAppRunner();
        #endregion

        #region Chain Service Basic Method
        /// <summary>
        /// Create runner
        /// 创建运行用户身份
        /// </summary>
        public IWorkflowService CreateRunner(WfAppRunner runner)
        {
            _wfAppRunner = runner;
            return this;
        }

        /// <summary>
        /// Create runner
        /// 创建运行用户身份
        /// </summary>
        public IWorkflowService CreateRunner(string userID, string userName)
        {
            _wfAppRunner.UserID = userID;
            _wfAppRunner.UserName = userName;
            _wfAppRunner.TaskID = null;

            return this;
        }

        /// <summary>
        /// User application
        /// 绑定业务票据
        /// </summary>
        public IWorkflowService UseApp(string appInstanceID, string appName, string appCode = null)
        {
            _wfAppRunner.AppInstanceID = appInstanceID;
            _wfAppRunner.AppName = appName;
            _wfAppRunner.AppInstanceCode = appCode;
            return this;
        }

        /// <summary>
        /// Use Process Definition
        /// </summary>
        public IWorkflowService UseProcess(string processCodeOrProcessGUID, string version = null)
        {
            if (string.IsNullOrEmpty(version)) version = "1";

            Guid newGUID = Guid.Empty;
            bool isProcessGUID = Guid.TryParse(processCodeOrProcessGUID, out newGUID);
            if (isProcessGUID == true)
            {
                _wfAppRunner.ProcessGUID = processCodeOrProcessGUID;
                _wfAppRunner.Version = version;
            }
            else
            {
                var pm = new ProcessManager();
                var entity = pm.GetByCode(processCodeOrProcessGUID, version);
                _wfAppRunner.ProcessGUID = entity.ProcessGUID;
                _wfAppRunner.Version = entity.Version;
            }

            return this;
        }

        /// <summary>
        /// Next activity
        /// This method is used for internal testing
        /// Special note: Do not use this method in formal production environments
        /// 下一步活动
        /// 内部测试时用到此方法
        /// 特别注意：正式生产环境，不要使用该方法
        /// </summary>
        public IWorkflowService NextStepInt(PerformerList performerList)
        {
            if (_wfAppRunner.TaskID.HasValue == false)
            {
                var tm = new TaskManager();
                var task = tm.GetTaskOfMine(_wfAppRunner);
                _wfAppRunner.TaskID = task.TaskID;
            }
            var nextStep = new Dictionary<string, PerformerList>();
            var nodeList = GetNextActivityTree(_wfAppRunner.TaskID.Value, _wfAppRunner.Conditions);
            foreach (var node in nodeList)
            {
                if (Xpdl.XPDLHelper.IsSimpleComponentNode(node.ActivityType) == true)
                {
                    nextStep.Add(node.ActivityGUID, performerList);
                }
            }
            _wfAppRunner.NextActivityPerformers = nextStep;

            return this;
        }

        /// <summary>
        /// Next activity
        /// This method is used for internal testing
        /// Special note: Do not use this method in formal production environments
        /// 下一步活动
        /// 内部测试时用到此方法
        /// 特别注意：正式生产环境，不要使用该方法
        /// </summary>
        public IWorkflowService NextStepInt(string userID, string userName)
        {
            var performerList = new PerformerList();
            performerList.Add(new Performer(userID, userName));

            return NextStepInt(performerList);
        }

        /// <summary>
        /// Next step information
        /// 下一步活动
        /// </summary>
        public IWorkflowService NextStep(IDictionary<string, PerformerList> nextActivityPerformers)
        {
            if (nextActivityPerformers != null && nextActivityPerformers.Count() > 0)
            {
                _wfAppRunner.NextActivityPerformers = nextActivityPerformers;
            }
            else
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("workflowservice.nextstep.error"));
            }
            return this;
        }

        /// <summary>
        /// Next step information
        /// 下一步活动
        /// </summary>
        public IWorkflowService NextStep(string activityGUID, PerformerList performerList)
        {
            if (performerList != null && performerList.Count() > 0)
            {
                _wfAppRunner.NextActivityPerformers.Add(activityGUID, performerList);
            }
            else
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("workflowservice.nextstep.error"));
            }
            return this;
        }

        /// <summary>
        /// Previous step internal
        /// 指定上一步类型
        /// </summary>
        public IWorkflowService PrevStepInt()
        {
            _wfAppRunner.NextPerformerType = NextPerformerIntTypeEnum.Traced;
            return this;
        }

        /// <summary>
        /// Set condition
        /// 设置变量条件
        /// </summary>
        public IWorkflowService IfCondition(IDictionary<string, string> variables)
        {
            _wfAppRunner.Conditions = variables;
            return this;
        }

        /// <summary>
        /// Set condition
        /// 添加条件变量
        /// </summary>
        public IWorkflowService IfCondition(string name, string value)
        {
            _wfAppRunner.Conditions.Add(name, value);
            return this;
        }

        /// <summary>
        /// Set task
        /// 传递任务ID
        /// </summary>
        public IWorkflowService OnTask(int taskID)
        {
            _wfAppRunner.TaskID = taskID;
            return this;
        }

        /// <summary>
        /// Set variable
        /// 添加动态变量
        /// </summary>
        public IWorkflowService SetVariable(string name, string value)
        {
            _wfAppRunner.DynamicVariables.Add(name, value);
            return this;
        }

        /// <summary>
        /// Set variable
        /// 添加动态变量
        /// </summary>
        public IWorkflowService SetVariable(IDictionary<string, string> variables)
        {
            _wfAppRunner.DynamicVariables = variables;
            return this;
        }

        /// <summary>
        /// Subscribe
        /// 活动事件订阅
        /// </summary>
        public IWorkflowService Subscribe(EventFireTypeEnum eventType, Func<DelegateContext, IDelegateService, Boolean> func)
        {
            _wfAppRunner.DelegateEventList.Add(
                new KeyValuePair<EventFireTypeEnum, Func<DelegateContext, IDelegateService, bool>>(eventType, func)
            );
            return this;
        }
        #endregion
    }
}
