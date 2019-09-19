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

namespace Slickflow.Engine.Delegate
{
    /// <summary>
    ///  外部事件交互触发类型
    ///  1. 流程事件：
    ///  流程启动->流程运行->流程结束
    ///  2. 活动事件：
    ///  活动创建->活动执行->活动完成
    ///  3. 任务：
    ///  分配->完成
    /// </summary>
    public enum EventFireTypeEnum
    {
        /// <summary>
        /// 默认
        /// </summary>
        None = 0,

        /// <summary>
        /// 流程正在执行启动
        /// </summary>
        OnProcessStarting = 1,

        /// <summary>
        /// 流程完成启动
        /// </summary>
        OnProcessStarted = 2,

        /// <summary>
        /// 流程正在运行(通常由节点迁移来代表运行)
        /// </summary>
        OnProcessRunning = 3,

        /// <summary>
        /// 流程运行完某个节点(通常由节点迁移来代表运行)
        /// </summary>
        OnProcessContinued = 4,

        /// <summary>
        /// 结束流程(流程状态为[完成]状态)
        /// </summary>
        OnProcessCompleted = 5,

        /// <summary>
        /// 流程撤销回上一步骤
        /// </summary>
        OnProcessWithdrawing = 10,

        /// <summary>
        /// 流程已经撤销到上一步骤
        /// </summary>
        OnProcessWithdrawn = 11,

        /// <summary>
        /// 流程退回上一步骤
        /// </summary>
        OnProcessSendBacking = 12,

        /// <summary>
        /// 流程已经退回到上一步骤
        /// </summary>
        OnProcessSendBacked = 13,

        /// <summary>
        /// 流程返送
        /// </summary>
        OnProcessResending = 14,

        /// <summary>
        /// 流程已经返送
        /// </summary>
        OnProcessResent = 15,

        /// <summary>
        /// 流程返签
        /// </summary>
        OnProcessReversing = 16,

        /// <summary>
        /// 流程已经返签
        /// </summary>
        OnProcessReversed = 17,

        /// <summary>
        /// 流程跳转中
        /// </summary>
        OnProcessJumping = 18,

        /// <summary>
        /// 流程已经跳转
        /// </summary>
        OnProcessJumped = 19,

        /// <summary>
        /// 流程加签
        /// </summary>
        OnProcessSignForwarding = 20,

        /// <summary>
        /// 流程已经加签
        /// </summary>
        OnProcessSignForwarded = 21,

        /// <summary>
        /// 流程修订中
        /// </summary>
        OnProcessRevising = 22,

        /// <summary>
        /// 流程已经修订
        /// </summary>
        OnProcessRevised = 23,

        /// <summary>
        /// 流程驳回中
        /// </summary>
        OnProcessRejecting = 24,

        /// <summary>
        /// 流程已经驳回
        /// </summary>
        OnProcessRejected = 25,

        /// <summary>
        /// 流程关闭中
        /// </summary>
        OnProcessClosing = 26,

        /// <summary>
        /// 流程已经关闭
        /// </summary>
        OnProcessClosed = 27,

        /// <summary>
        /// 下一步节点已经创建
        /// </summary>
        OnActivityCreated = 40,

        /// <summary>
        /// 节点正在运行
        /// </summary>
        OnActivityExecuting = 41,

        /// <summary>
        /// 节点运行方法结束
        /// </summary>
        OnActivityExecuted = 42,

        /// <summary>
        /// 节点完成(节点状态为[完成]状态)
        /// </summary>
        OnActivityCompleted = 43,

        /// <summary>
        /// 任务记录已经创建(用于邮件消息通知)
        /// </summary>
        OnTaskCreaed = 60,

        /// <summary>
        /// 任务已经完成(用于邮件消息通知)
        /// </summary>
        OnTaskCompleted = 61,

        /// <summary>
        /// 成功执行
        /// </summary>
        OnSuccess = 100,

        /// <summary>
        /// 错误发生
        /// </summary>
        OnError = 101,

        /// <summary>
        /// 有警告异常信息
        /// </summary>
        OnWarning = 102
    }
}
