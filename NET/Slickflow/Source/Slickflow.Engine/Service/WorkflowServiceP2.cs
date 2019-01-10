/*
* Slickflow 软件遵循自有项目开源协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。
* 
The Slickflow Open License (SfPL 1.0)
Copyright (C) 2014  .NET Workflow Engine Library

1. Slickflow software must be legally used, and should not be used in violation of law, 
   morality and other acts that endanger social interests;
2. Non-transferable, non-transferable and indivisible authorization of this software;
3. The source code can be modified to apply Slickflow components in their own projects 
   or products, but Slickflow source code can not be separately encapsulated for sale or 
   distributed to third-party users;
4. The intellectual property rights of Slickflow software shall be protected by law, and
   no documents such as technical data shall be made public or sold.
5. The enterprise, ultimate and universe version can be provided with commercial license, 
   technical support and upgrade service.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Delegate;
using Slickflow.Engine.Core.Result;


namespace Slickflow.Engine.Service
{
    /// <summary>
    /// 链式接口服务类
    /// </summary>
    public partial class WorkflowService : IWorkflowService
    {
        #region 基本属性
        private WfAppRunner _wfAppRunner = new WfAppRunner();
        #endregion

        #region 链式初始化方法
        /// <summary>
        /// 创建运行用户身份
        /// </summary>
        /// <param name="runner">运行用户</param>
        /// <returns>服务类</returns>
        public IWorkflowService CreateRunner(WfAppRunner runner)
        {
            _wfAppRunner = runner;
            return this;
        }

        /// <summary>
        /// 创建运行用户身份
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="userName">用户名称</param>
        /// <returns>服务类</returns>
        public IWorkflowService CreateRunner(string userID, string userName)
        {
            _wfAppRunner.UserID = userID;
            _wfAppRunner.UserName = userName;
            return this;
        }

        /// <summary>
        /// 绑定业务票据
        /// </summary>
        /// <param name="appInstanceID">业务实例ID</param>
        /// <param name="appName">业务应用名称</param>
        /// <param name="appCode">业务应用编码</param>
        /// <returns>服务类</returns>
        public IWorkflowService UseApp(string appInstanceID, string appName, string appCode = null)
        {
            _wfAppRunner.AppInstanceID = appInstanceID;
            _wfAppRunner.AppName = appName;
            _wfAppRunner.AppInstanceCode = appCode;
            return this;
        }

        /// <summary>
        /// 绑定流程定义
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">流程版本</param>
        /// <returns>服务类</returns>
        public IWorkflowService UseProcess(string processGUID, string version)
        {
            _wfAppRunner.ProcessGUID = processGUID;
            _wfAppRunner.Version = version;
            return this;
        }


        /// <summary>
        /// 下一步活动
        /// </summary>
        /// <param name="nextActivityPerformers">待办用户列表</param>
        /// <returns>服务类</returns>
        public IWorkflowService NextStep(IDictionary<string, PerformerList> nextActivityPerformers)
        {
            if (nextActivityPerformers != null && nextActivityPerformers.Count() > 0)
            {
                _wfAppRunner.NextActivityPerformers = nextActivityPerformers;
            }
            else
            {
                throw new ApplicationException("下一步办理用户列表不能为空！");
            }
            return this;
        }

        /// <summary>
        /// 下一步活动
        /// </summary>
        /// <param name="activityGUID">活动节点GUID</param>
        /// <param name="performerList">执行用户列表</param>
        /// <returns>服务类</returns>
        public IWorkflowService NextStep(string activityGUID, PerformerList performerList)
        {
            if (performerList != null && performerList.Count() > 0)
            {
                _wfAppRunner.NextActivityPerformers.Add(activityGUID, performerList);
            }
            else
            {
                throw new ApplicationException("下一步办理用户列表不能为空！");
            }
            return this;
        }

        /// <summary>
        /// 设置变量条件
        /// </summary>
        /// <param name="variables">变量列表</param>
        /// <returns>服务类</returns>
        public IWorkflowService IfCondition(IDictionary<string, string> variables)
        {
            _wfAppRunner.Conditions = variables;
            return this;
        }

        /// <summary>
        /// 添加条件变量
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">数值</param>
        /// <returns>服务类</returns>
        public IWorkflowService IfCondition(string name, string value)
        {
            _wfAppRunner.Conditions.Add(name, value);
            return this;
        }

        /// <summary>
        /// 活动事件订阅
        /// </summary>
        /// <param name="eventType">活动事件类型</param>
        /// <param name="func">回调方法</param>
        /// <returns>服务类</returns>
        public IWorkflowService Subscribe(EventFireTypeEnum eventType, Func<int, string, IDelegateService, Boolean> func)
        {
            _wfAppRunner.DelegateEventList.Add(
                new KeyValuePair<EventFireTypeEnum, Func<int, string, IDelegateService, bool>>(eventType, func)
            );
            return this;
        }
        #endregion
    }
}
