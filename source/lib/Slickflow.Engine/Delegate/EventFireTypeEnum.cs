using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Delegate
{
    /// <summary>
    /// External event interaction trigger type
    ///  1.  Process event:
    /// Process Start ->Process Run ->Process End
    ///  2.  Event:
    /// Activity creation ->Activity execution ->Activity completion
    ///  3.  Task:
    /// Allocation ->Complete
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
        None = 0,

        OnProcessStarting = 1,

        OnProcessStarted = 2,

        OnProcessRunning = 3,

        OnProcessContinued = 4,

        OnProcessCompleted = 5,

        OnProcessWithdrawing = 10,

        OnProcessWithdrawn = 11,

        OnProcessSendBacking = 12,

        OnProcessSendBacked = 13,

        OnProcessResending = 14,

        OnProcessResent = 15,

        OnProcessReversing = 16,

        OnProcessReversed = 17,

        OnProcessJumping = 18,

        OnProcessJumped = 19,

        OnProcessSignForwarding = 20,

        OnProcessSignForwarded = 21,

        OnProcessRevising = 22,

        OnProcessRevised = 23,

        OnProcessRejecting = 24,

        OnProcessRejected = 25,

        OnProcessClosing = 26,

        OnProcessClosed = 27,

        OnActivityCreated = 40,

        OnActivityExecuting = 41,

        OnActivityExecuted = 42,

        OnActivityCompleted = 43,

        OnTaskCreaed = 60,

        OnTaskCompleted = 61,

        OnSuccess = 100,

        OnError = 101,

        OnWarning = 102
    }
}
