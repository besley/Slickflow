using System;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Event
{
    /// <summary>
    /// Event Service Base
    /// 事件服务基类
    /// </summary>
    public abstract class EventServiceBase
    {
        #region Property, Abstract and Constructor
        public EventContext EventContext { get; set; }
        public IDbSession Session { get; set; }

        public abstract T GetInstance<T>(int id) where T : class;

        public EventServiceBase(IDbSession session, 
            EventContext context)
        {
            Session = session;
            EventContext = context;
        }
        #endregion

        /// <summary>
        /// Read instance primary key Id
        /// 读取实例主键Id
        /// </summary>
        /// <returns></returns>
        public int GetProcessInstanceId()
        {
            return this.EventContext.ProcessInstanceId;
        }

        /// <summary>
        /// Get Session
        /// </summary>
        /// <returns></returns>
        public IDbSession GetSession()
        {
            return Session;
        }

        /// <summary>
        /// Get Condition Value
        /// 获取条件参数数值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetCondition(string name)
        {
            var value = this.EventContext.ActivityResource.ConditionKeyValuePair[name];
            return value;
        }
        /// <summary>
        /// Set Condition Value
        /// 设置条件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetCondition(string name, string value)
        {
            this.EventContext.ActivityResource.ConditionKeyValuePair[name] = value;
        }

        /// <summary>
        /// Set Activity Resource
        /// 设置活动资源
        /// </summary>
        /// <param name="activityResource"></param>
        internal void SetActivityResource(ActivityResource activityResource)
        {
            this.EventContext.ActivityResource = activityResource;
        }

        /// <summary>
        /// Save Variable
        /// 保存变量
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="variableType"></param>
        public void SaveVariable(ProcessVariableScopeEnum variableType, string name, string value)
        {
            ProcessVariableEntity processVariable = null;

            if (variableType == ProcessVariableScopeEnum.Process)
            {
                processVariable = new ProcessVariableEntity
                {
                    VariableScope = ProcessVariableScopeEnum.Process.ToString(),
                    AppInstanceId = this.EventContext.AppInstanceId,
                    ProcessId = this.EventContext.ProcessId,
                    ProcessInstanceId = this.EventContext.ProcessInstanceId,
                    ActivityInstanceId = this.EventContext.ActivityInstanceId,
                    Name = name,
                    Value = value,
                    UpdatedDateTime = System.DateTime.UtcNow
                };
            }
            else if (variableType == ProcessVariableScopeEnum.Activity)
            {
                processVariable = new ProcessVariableEntity
                {
                    VariableScope = ProcessVariableScopeEnum.Activity.ToString(),
                    AppInstanceId = this.EventContext.AppInstanceId,
                    ProcessId = this.EventContext.ProcessId,
                    ProcessInstanceId = this.EventContext.ProcessInstanceId,
                    ActivityInstanceId = this.EventContext.ActivityInstanceId,
                    ActivityId = this.EventContext.ActivityId,
                    ActivityName = this.EventContext.ActivityName,
                    Name = name,
                    Value = value,
                    UpdatedDateTime = System.DateTime.UtcNow
                };
            }
            var pvm = new ProcessVariableManager();
            pvm.SaveVariable(Session.Connection, processVariable, Session.Transaction);
        }

        /// <summary>
        /// Get Variable
        /// 获取变量
        /// </summary>
        /// <param name="name"></param>
        /// <param name="variableType"></param>
        /// <returns></returns>
        public string GetVariable(ProcessVariableScopeEnum variableType, string name)
        {
            var pvm = new ProcessVariableManager();
            var variable = pvm.GetVariable(Session.Connection, variableType, name, this.EventContext, Session.Transaction);
            return variable;
        }

        /// <summary>
        /// Get Variable Throughtly
        /// 获取变量内容
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetVariableByScopePriority(string name)
        {
            var pvm = new ProcessVariableManager();
            var variable = pvm.GetVariableByScopePriority(Session.Connection, name, this.EventContext, Session.Transaction);
            return variable;
        }
    }
}
