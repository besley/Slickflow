using System;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Delegate
{
    /// <summary>
    /// Delegate Service Base
    /// </summary>
    public abstract class DelegateServiceBase
    {
        #region Property, Abstract and Constructor
        private ActivityResource _activityResource = null;
        public string AppInstanceID { get; set; }
        public string ProcessID { get; set; }
        public int ProcessInstanceID { get; set; }
        public string ActivityID { get; set; }
        public string ActivityName { get; set; }
        public IDbSession Session { get; set; }

        public abstract T GetInstance<T>(int id) where T : class;

        public DelegateServiceBase(IDbSession session, 
            DelegateContext context)
        {
            Session = session;
            AppInstanceID = context.AppInstanceID;
            ProcessID = context.ProcessID;
            ProcessInstanceID = context.ProcessInstanceID;
            ActivityID = context.ActivityID;
            ActivityName = context.ActivityName;
        }
        #endregion

        /// <summary>
        /// Read instance primary key ID
        /// 读取实例主键ID
        /// </summary>
        /// <returns></returns>
        public int GetProcessInstanceID()
        {
            return ProcessInstanceID;
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
            var value = _activityResource.ConditionKeyValuePair[name];
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
            _activityResource.ConditionKeyValuePair[name] = value;
        }

        /// <summary>
        /// Set Activity Resource
        /// 设置活动资源
        /// </summary>
        /// <param name="activityResource"></param>
        internal void SetActivityResource(ActivityResource activityResource)
        {
            _activityResource = activityResource;
        }

        /// <summary>
        /// Save Variable
        /// 保存变量
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="variableType"></param>
        public void SaveVariable(ProcessVariableTypeEnum variableType, string name, string value)
        {
            var pvm = new ProcessVariableManager();
            ProcessVariableEntity entity = null;

            if (variableType == ProcessVariableTypeEnum.Process)
            {
                entity = new ProcessVariableEntity
                {
                    VariableType = ProcessVariableTypeEnum.Process.ToString(),
                    AppInstanceID = this.AppInstanceID,
                    ProcessID = this.ProcessID,
                    ProcessInstanceID = this.ProcessInstanceID,
                    Name = name,
                    Value = value,
                    LastUpdatedDateTime = System.DateTime.Now
                };
            }
            else if (variableType == ProcessVariableTypeEnum.Activity)
            {
                entity = new ProcessVariableEntity
                {
                    VariableType = ProcessVariableTypeEnum.Activity.ToString(),
                    AppInstanceID = this.AppInstanceID,
                    ProcessID = this.ProcessID,
                    ProcessInstanceID = this.ProcessInstanceID,
                    ActivityID = this.ActivityID,
                    ActivityName = this.ActivityName,
                    Name = name,
                    Value = value,
                    LastUpdatedDateTime = System.DateTime.Now
                };
            }
            pvm.SaveVariable(Session.Connection, entity, Session.Transaction);
        }

        /// <summary>
        /// Get Variable
        /// 获取变量
        /// </summary>
        /// <param name="name"></param>
        /// <param name="variableType"></param>
        /// <returns></returns>
        public string GetVariable(ProcessVariableTypeEnum variableType, string name)
        {
            var value = string.Empty;
            ProcessVariableQuery query = null;
            var pvm = new ProcessVariableManager();

            if (variableType == ProcessVariableTypeEnum.Process)
            {
                query = new ProcessVariableQuery
                {
                    VariableType = ProcessVariableTypeEnum.Process,
                    ProcessInstanceID = this.ProcessInstanceID,
                    Name = name
                };
                value = pvm.GetVariableValue(Session.Connection, query, Session.Transaction);
            }
            else if (variableType == ProcessVariableTypeEnum.Activity)
            {
                query = new ProcessVariableQuery
                {
                    VariableType = ProcessVariableTypeEnum.Activity,
                    ProcessInstanceID = this.ProcessInstanceID,
                    ActivityID = this.ActivityID,
                    Name = name
                };
                value = pvm.GetVariableValue(Session.Connection, query, Session.Transaction);
            }
            return value;
        }

        /// <summary>
        /// Get Variable Throughtly
        /// 获取变量内容
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetVariableThroughly(string name)
        {
            var query = new ProcessVariableQuery();
            query.VariableType = ProcessVariableTypeEnum.Activity;
            query.ProcessInstanceID = this.ProcessInstanceID;
            query.Name = name;

            var value = GetVariable(ProcessVariableTypeEnum.Activity, name);
            if (value == null)
            {
                value = GetVariable(ProcessVariableTypeEnum.Process, name);
            }
            return value;
        }
    }
}
