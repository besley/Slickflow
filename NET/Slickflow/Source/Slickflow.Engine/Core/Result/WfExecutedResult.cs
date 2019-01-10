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
        public Int32 ProcessInstanceIDStarted { get; set; }     //流程启动返回的新实例ID
        public WfBackwardTaskReceiver BackwardTaskReceiver { get; set; }    //流程做回退处理时，返回的任务接收人信息
        public ReturnDataContext ReturnDataContext { get; set; }  //流程回退时返回参数
        public WfExecutedResult()
        {
            Status = WfExecutedStatus.Default;
            Message = string.Empty;
        }

        /// <summary>
        /// 缺省方法
        /// </summary>
        /// <returns></returns>
        public static WfExecutedResult Default()
        {
            return new WfExecutedResult();
        }
    }

    /// <summary>
    /// 状态执行枚举类型
    /// </summary>
    public enum WfExecutedStatus
    {
        /// <summary>
        /// 缺省状态
        /// </summary>
        Default = 0,

        /// <summary>
        /// 成功状态
        /// </summary>
        Success = 1,

        /// <summary>
        /// 执行失败状态
        /// </summary>
        Failed = 2,

        /// <summary>
        /// 异常状态
        /// </summary>
        Exception = 3
    }

    /// <summary>
    /// 异常类型类
    /// </summary>
    public class WfExceptionType
    {
        //流程启动异常信息
        public const string Started_IsRunningAlready = "Started_IsRunningAlready";

        //流程运行异常信息
        public const string RunApp_ErrorArguments = "RunApp_ErrorArguments";
        public const string RunApp_HasNoTask = "RunApp_HasNoTask";
        public const string RunApp_OverTasks = "RunApp_OverTasks";
        public const string RunApp_RuntimeError = "RunApp_RuntimeError";

        //流程跳转异常信息
        public const string Jump_ErrorArguments = "Jump_ErrorArguments";
        public const string Jump_OverOneStep = "Jump_OverOneStep";
        public const string Jump_NotActivityBackCompleted = "Jump_NotActivityBackCompleted";
        public const string Jump_OtherError = "Jump_OtherError";

        //流程撤销异常信息
        public const string Withdraw_NotInReady = "Withdraw_NotInReady";
        public const string Withdraw_NotCreatedByMine = "Withdraw_NotCreatedByMine";
        public const string Withdraw_HasTooMany = "Withdraw_HasTooMany";
        public const string Withdraw_PreviousIsEndNode = "Withdraw_PreviousIsEndNode";
        public const string Withdraw_SignTogetherNotAllowed = "Withdraw_SignTogetherNotAllowed";
        public const string Withdraw_IsLoopNode = "Withdraw_IsLoopNode";
        
        //流程退回异常信息
        public const string Sendback_NotTaskNode = "Sendback_NotTaskNode";
        public const string Sendback_IsLoopNode = "Sendback_IsLoopNode";
        public const string Sendback_NotInRunning = "Sendback_NotInRunning";
        public const string Sendback_NotMineTask = "NotMineTask";
        public const string Sendback_IsNull = "Sendback_IsNull";
        public const string Sendback_IsTooManyPrevious = "Sendback_IsTooManyPrevious";
        public const string Sendback_NotContainedInPreviousOrStartNode = "Sendback_NotContainedInPreviousOrStartNode";
        public const string Sendback_IsStartNode = "Sendback_IsStartNode";
        public const string Sendback_HasTooManyRunningParallel = "Sendback_HasTooManyRunningParallel";

        //流程返签异常信息
        public const string Reverse_NotInCompleted = "Reverse_NotInCompleted";

        //流程加签异常信息
        public const string SignForward_ErrorArguments = "SignForward_ErrorArguments";
        public const string SignForward_NoneSigners = "SignForward_NoneSigners";
        public const string SignForward_RuntimeError = "SignForward_RuntimeError";

    }
}

