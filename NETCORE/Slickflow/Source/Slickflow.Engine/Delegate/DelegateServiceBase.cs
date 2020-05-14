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
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Delegate
{
    /// <summary>
    /// 委托服务基类
    /// </summary>
    public abstract class DelegateServiceBase
    {
        #region 属性、抽象方法及构造
        private ActivityResource _activityResource = null;
        public string AppInstanceID { get; set; }
        public string ProcessGUID { get; set; }
        public int ProcessInstanceID { get; set; }
        public string ActivityGUID { get; set; }
        public string ActivityName { get; set; }
        public IDbSession Session { get; set; }

        public abstract T GetInstance<T>(int id) where T : class;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="session">会话</param>
        /// <param name="context">上下文</param>
        public DelegateServiceBase(IDbSession session, 
            DelegateContext context)
        {
            Session = session;
            AppInstanceID = context.AppInstanceID;
            ProcessGUID = context.ProcessGUID;
            ProcessInstanceID = context.ProcessInstanceID;
            ActivityGUID = context.ActivityGUID;
            ActivityName = context.ActivityName;
        }
        #endregion

        /// <summary>
        /// 读取实例主键ID
        /// </summary>
        /// <returns>主键ID</returns>
        public int GetProcessInstanceID()
        {
            return ProcessInstanceID;
        }

        /// <summary>
        /// 获取Session
        /// </summary>
        /// <returns>会话</returns>
        public IDbSession GetSession()
        {
            return Session;
        }

        /// <summary>
        /// 获取条件参数数值
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>参数数值</returns>
        public string GetCondition(string name)
        {
            var value = _activityResource.ConditionKeyValuePair[name];
            return value;
        }
        /// <summary>
        /// 设置条件
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">数值</param>
        public void SetCondition(string name, string value)
        {
            _activityResource.ConditionKeyValuePair[name] = value;
        }

        /// <summary>
        /// 设置活动资源
        /// </summary>
        /// <param name="activityResource">资源</param>
        internal void SetActivityResource(ActivityResource activityResource)
        {
            _activityResource = activityResource;
        }

        /// <summary>
        /// 设置变量
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">数值</param>
        /// <param name="variableType">变量类型</param>
        public void SaveVariable(ProcessVariableTypeEnum variableType, string name, string value)
        {
            var pvm = new ProcessVariableManager();
            ProcessVariableEntity entity = null;

            if (variableType == ProcessVariableTypeEnum.Process)
            {
                //设置流程变量
                entity = new ProcessVariableEntity
                {
                    VariableType = ProcessVariableTypeEnum.Process.ToString(),
                    AppInstanceID = this.AppInstanceID,
                    ProcessGUID = this.ProcessGUID,
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
                    ProcessGUID = this.ProcessGUID,
                    ProcessInstanceID = this.ProcessInstanceID,
                    ActivityGUID = this.ActivityGUID,
                    ActivityName = this.ActivityName,
                    Name = name,
                    Value = value,
                    LastUpdatedDateTime = System.DateTime.Now
                };
            }
            pvm.SaveVariable(Session.Connection, entity, Session.Transaction);
        }

        /// <summary>
        /// 获取变量
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="variableType">变量类型</param>
        /// <returns>数值</returns>
        public string GetVariable(ProcessVariableTypeEnum variableType, string name)
        {
            //查询活动变量
            var value = string.Empty;
            ProcessVariableQuery query = null;
            var pvm = new ProcessVariableManager();

            if (variableType == ProcessVariableTypeEnum.Process)
            {
                //查询流程变量
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
                    ActivityGUID = this.ActivityGUID,
                    Name = name
                };
                value = pvm.GetVariableValue(Session.Connection, query, Session.Transaction);
            }
            return value;
        }

        /// <summary>
        /// 获取变量内容
        /// </summary>
        /// <param name="name">变量名称</param>
        /// <returns>变量内容</returns>
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
