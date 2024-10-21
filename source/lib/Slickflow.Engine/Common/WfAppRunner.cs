﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Delegate;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 流程执行人(业务应用的办理者)
    /// 说明：WfAppRunner是流程流转参数的传递对象，传递引擎执行需要的业务数据、资源数据和流程定义数据等。
    /// WfAppRunner数据格式：
    /// {
    ///     "UserID":"10",
    ///     "UserName":"Long",
    ///     "AppName":"SamplePrice",
    ///     "AppInstanceID":"100",
    ///     "ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d"
    /// }
    /// </summary>
    public class WfAppRunner
    {
        #region 属性
        /// <summary>
        /// 业务数据：应用名称
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// 业务数据：应用实例ID（比如单据票据编号）
        /// </summary>
        public string AppInstanceID { get; set; }
        /// <summary>
        /// 业务数据：应用实例ID（比如单据票据代码）
        /// </summary>
        public string AppInstanceCode { get; set; }
        /// <summary>
        /// 流程数据：流程GUID
        /// </summary>
        public string ProcessGUID { get; set; }
        /// <summary>
        /// 流程数据：流程代码
        /// </summary>
        public string ProcessCode { get; set; }
        /// <summary>
        /// 流程数据：流程版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 用户数据：用户ID
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 流程数据：用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public string CompanyID { get; set; }
        /// <summary>
        /// 流程数据：待办任务ID
        /// </summary>
        public Nullable<int> TaskID { get; set; }        //任务ID，区分当前用户ActivityInstance列表的唯一任务
        /// <summary>
        /// 流程数据：活动实例ID
        /// </summary>
        public Nullable<int> ActivityInstanceID { get; set; }
        /// <summary>
        /// 审批结果状态
        /// </summary>
        public ApprovalStatusEnum ApprovalStatus { get; set; }

        private IDictionary<string, string> conditions;
        /// <summary>
        /// 流程数据：条件参数
        /// </summary>
        public IDictionary<string, string> Conditions 
        {
            get
            {
                if (conditions == null)
                {
                    conditions = new Dictionary<string, string>();
                }
                return conditions;
            }
            set
            {
                conditions = value;
            }
        }
        /// <summary>
        /// 流程数据：动态变量
        /// </summary>
        public IDictionary<string, string> DynamicVariables { get; set; }
        /// <summary>
        /// 流程数据：控制参数
        /// </summary>
        public ControlParameterSheet ControlParameterSheet { get; set; }
        /// <summary>
        /// 流程数据：下一步办理人员列表
        /// </summary>
        public IDictionary<string, PerformerList> NextActivityPerformers { get; set; }

        /// <summary>
        /// 流程数据：下一步执行类型
        /// </summary>
        public NextPerformerIntTypeEnum NextPerformerType { get; set; }

        /// <summary>
        /// 委托事件
        /// </summary>
        internal DelegateEventList DelegateEventList = new DelegateEventList();

        /// <summary>
        /// 用于消息启动时的主题
        /// </summary>
        public string MessageTopic { get; set; }
        #endregion
    }

    /// <summary>
    /// 流程返签、撤销和退回接收人的实体对象
    /// </summary>
    public class WfBackwardTaskReceiver
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string ActivityName { get; set; }

        /// <summary>
        /// 构造WfBackwardReceiver实例
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="userName">用户名称</param>
        /// <param name="backwardToActivityName">活动名称</param>
        /// <returns></returns>
        public static WfBackwardTaskReceiver Instance(string backwardToActivityName,
            string userID,
            string userName)
        {
            var instance = new WfBackwardTaskReceiver();
            instance.ActivityName = backwardToActivityName;
            instance.UserID = userID;
            instance.UserName = userName;
            
            return instance;
        }
    }
}
