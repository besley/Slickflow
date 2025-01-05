
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Delegate;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Process executor (operator of business applications)
    /// Explanation: WfAppRunner is the object for transmitting process flow parameters, 
    /// including business data, resource data, and process definition data required for engine execution.
    /// WfAppRunner Json Format：
    /// {
    ///     "UserID":"10",
    ///     "UserName":"Long",
    ///     "AppName":"SamplePrice",
    ///     "AppInstanceID":"100",
    ///     "ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d"
    /// }
    /// 流程执行人(业务应用的办理者)
    /// 说明：WfAppRunner是流程流转参数的传递对象，传递引擎执行需要的业务数据、资源数据和流程定义数据等。
    /// </summary>
    public class WfAppRunner
    {
        /// <summary>
        /// Business Data: Application Name
        /// 业务数据：应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// Business data: Application instance ID (such as document number)
        /// 业务数据：应用实例ID（比如单据票据编号）
        /// </summary>
        public string AppInstanceID { get; set; }

        /// <summary>
        /// Business data: Application instance ID (such as document code)
        /// 业务数据：应用实例ID（比如单据票据代码）
        /// </summary>
        public string AppInstanceCode { get; set; }

        /// <summary>
        /// Process data: Process GUID
        /// 流程数据：流程GUID
        /// </summary>
        public string ProcessGUID { get; set; }

        /// <summary>
        /// Process data: Process Code
        /// 流程数据：流程代码
        /// </summary>
        public string ProcessCode { get; set; }

        /// <summary>
        /// Process data: Process Version
        /// 流程数据：流程版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// User data: User ID
        /// 用户数据：用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// User data: User Name
        /// 流程数据：用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Company ID
        /// 公司ID
        /// </summary>
        public string CompanyID { get; set; }

        /// <summary>
        /// Process Data: Task ID
        /// Task ID, distinguishing the unique task in the current user's ActiveInstance list
        /// 流程数据：待办任务ID
        /// </summary>
        public Nullable<int> TaskID { get; set; }

        /// <summary>
        /// Process data: Activity instance ID
        /// 流程数据：活动实例ID
        /// </summary>
        public Nullable<int> ActivityInstanceID { get; set; }

        /// <summary>
        /// Approval Status
        /// 审批结果状态
        /// </summary>
        public ApprovalStatusEnum ApprovalStatus { get; set; }

        /// <summary>
        /// Conditions
        /// 条件
        /// </summary>
        private IDictionary<string, string> conditions;

        /// <summary>
        /// Process data: conditions parameters
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
        /// Process data: dynamic variables
        /// 流程数据：动态变量
        /// </summary>
        public IDictionary<string, string> DynamicVariables { get; set; }

        /// <summary>
        /// Process data: control parameter sheet
        /// 流程数据：控制参数
        /// </summary>
        public ControlParameterSheet ControlParameterSheet { get; set; }

        /// <summary>
        /// Process data: List of performers for the next step of handling
        /// 流程数据：下一步办理人员列表
        /// </summary>
        public IDictionary<string, PerformerList> NextActivityPerformers { get; set; }

        /// <summary>
        /// Process data: performer type for the next step of handling
        /// 流程数据：下一步执行类型
        /// </summary>
        public NextPerformerIntTypeEnum NextPerformerType { get; set; }

        /// <summary>
        /// Deleage Event
        /// 委托事件
        /// </summary>
        internal DelegateEventList DelegateEventList = new DelegateEventList();

        /// <summary>
        /// Topic used for message initiation
        /// 用于消息启动时的主题
        /// </summary>
        public string MessageTopic { get; set; }
    }
}
