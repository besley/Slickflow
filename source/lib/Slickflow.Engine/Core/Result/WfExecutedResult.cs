using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Core.Result
{
    /// <summary>
    /// 执行结果对象封装
    /// </summary>
    public class WfExecutedResult
    {
        public WfExecutedStatus Status { get; set; }
        public String Message { get; set; }
        public String ExceptionType { get; set; }

        /// <summary>
        /// New instance ID returned by process initiation
        /// 流程启动返回的新实例ID
        /// </summary>
        public Int32 ProcessInstanceIDStarted { get; set; }

        /// <summary>
        /// When the process is rolled back, the recipient information of the returned task
        /// 流程做回退处理时，返回的任务接收人信息
        /// </summary>
        public WfBackwardTaskReceiver BackwardTaskReceiver { get; set; }

        /// <summary>
        /// Return parameters during process rollback
        /// 流程回退时返回参数
        /// </summary>
        public ReturnDataContext ReturnDataContext { get; set; }  
        public WfExecutedResult()
        {
            Status = WfExecutedStatus.Default;
            Message = string.Empty;
        }

        /// <summary>
        /// Default
        /// 缺省方法
        /// </summary>
        /// <returns></returns>
        public static WfExecutedResult Default()
        {
            return new WfExecutedResult();
        }
    }

    /// <summary>
    /// Status execution enumeration type
    /// 状态执行枚举类型
    /// </summary>
    public enum WfExecutedStatus
    {
        /// <summary>
        /// Default
        /// </summary>
        Default = 0,

        /// <summary>
        /// Success
        /// </summary>
        Success = 1,

        /// <summary>
        /// Failed
        /// </summary>
        Failed = 2,

        /// <summary>
        /// Exception
        /// </summary>
        Exception = 3
    }

    /// <summary>
    /// Workflow Exception Type
    /// 异常类型类
    /// </summary>
    public class WfExceptionType
    {
        //流程启动异常信息
        //Process startup exception information
        public const string Started_IsRunningAlready = "Started_IsRunningAlready";
        public const string Started_NoneExactlyProcessID = "Started_NoneExactlyProcessID";

        //流程运行异常信息
        //Process running exception information
        public const string RunApp_ErrorArguments = "RunApp_ErrorArguments";
        public const string RunApp_HasNoTask = "RunApp_HasNoTask";
        public const string RunApp_OverTasks = "RunApp_OverTasks";
        public const string RunApp_RuntimeError = "RunApp_RuntimeError";

        //流程跳转异常信息
        //Process jump execption information
        public const string Jump_ErrorArguments = "Jump_ErrorArguments";
        public const string Jump_OverOneStep = "Jump_OverOneStep";
        public const string Jump_NotActivityBackCompleted = "Jump_NotActivityBackCompleted";
        public const string Jump_OtherError = "Jump_OtherError";

        //流程撤销异常信息
        //Process withdraw exception information
        public const string Withdraw_ErrorArguments = "Withdraw_ErrorArguments";
        public const string Withdraw_NotInReady = "Withdraw_NotInReady";
        public const string Withdraw_NotCreatedByMine = "Withdraw_NotCreatedByMine";
        public const string Withdraw_HasTooMany = "Withdraw_HasTooMany";
        public const string Withdraw_PreviousIsEndNode = "Withdraw_PreviousIsEndNode";
        public const string Withdraw_SignTogetherNotAllowed = "Withdraw_SignTogetherNotAllowed";
        public const string Withdraw_IsLoopNode = "Withdraw_IsLoopNode";
        
        //流程退回异常信息
        //Process sendback exception information
        public const string Sendback_NotTaskNode = "Sendback_NotTaskNode";
        public const string Sendback_IsLoopNode = "Sendback_IsLoopNode";
        public const string Sendback_NotInRunning = "Sendback_NotInRunning";
        public const string Sendback_NotMineTask = "NotMineTask";
        public const string Sendback_IsNull = "Sendback_IsNull";
        public const string Sendback_IsTooManyPrevious = "Sendback_IsTooManyPrevious";
        public const string Sendback_NotContainedInPreviousOrStartNode = "Sendback_NotContainedInPreviousOrStartNode";
        public const string Sendback_IsStartNode = "Sendback_IsStartNode";
        public const string Sendback_HasTooManyRunningParallel = "Sendback_HasTooManyRunningParallel";

        //流程返送异常信息
        //Process resend exception information
        public const string Resend_NotTaskNode = "Resend_NotTaskNode";
        public const string Resend_WithoutBackSourceNode = "Resend_WithoutBackSourceNode";

        //流程返签异常信息
        //Process reverse exception information
        public const string Reverse_NotInCompleted = "Reverse_NotInCompleted";

        //流程加签异常信息
        //Process sign forward exception information
        public const string SignForward_ErrorArguments = "SignForward_ErrorArguments";
        public const string SignForward_NoneSigners = "SignForward_NoneSigners";
        public const string SignForward_RuntimeError = "SignForward_RuntimeError";
    }
}

