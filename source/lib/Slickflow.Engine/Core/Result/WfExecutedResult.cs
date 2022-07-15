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
        public const string Started_NoneExactlyProcessGUID = "Started_NoneExactlyProcessGUID";

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
        public const string Withdraw_ErrorArguments = "Withdraw_ErrorArguments";
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

        //流程返送异常信息
        public const string Resend_NotTaskNode = "Resend_NotTaskNode";
        public const string Resend_WithoutBackSourceNode = "Resend_WithoutBackSourceNode";

        //流程返签异常信息
        public const string Reverse_NotInCompleted = "Reverse_NotInCompleted";



        //流程加签异常信息
        public const string SignForward_ErrorArguments = "SignForward_ErrorArguments";
        public const string SignForward_NoneSigners = "SignForward_NoneSigners";
        public const string SignForward_RuntimeError = "SignForward_RuntimeError";

    }
}

