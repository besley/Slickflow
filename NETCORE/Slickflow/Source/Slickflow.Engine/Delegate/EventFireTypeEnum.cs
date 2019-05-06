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
        /// 下一步节点已经创建
        /// </summary>
        OnActivityCreated = 30,

        /// <summary>
        /// 节点正在运行
        /// </summary>
        OnActivityExecuting = 31,

        /// <summary>
        /// 节点运行方法结束
        /// </summary>
        OnActivityExecuted = 32,

        /// <summary>
        /// 节点完成(节点状态为[完成]状态)
        /// </summary>
        OnActivityCompleted = 33,

        /// <summary>
        /// 任务记录已经创建(用于邮件消息通知)
        /// </summary>
        OnTaskCreaed = 50,

        /// <summary>
        /// 任务已经完成(用于邮件消息通知)
        /// </summary>
        OnTaskCompleted = 51,

        /// <summary>
        /// 成功执行
        /// </summary>
        OnSuccess = 60,

        /// <summary>
        /// 错误发生
        /// </summary>
        OnError = 61,

        /// <summary>
        /// 有警告异常信息
        /// </summary>
        OnWarning = 62
    }
}
