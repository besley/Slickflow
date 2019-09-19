using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Slickflow.Engine.Business.Entity;
using Slickflow.Scheduler.Entity;

namespace Slickflow.Scheduler.Utility
{
    /// <summary>
    /// 后台程序运行类
    /// Reference: https://stackoverflow.com/questions/3408397/asynchronously-sending-emails-in-c
    /// </summary>
    public static class BackgroundTaskRunner
    {
        /// <summary>
        /// 普通任务加入队列
        /// </summary>
        /// <param name="action">操作</param>
        /// <param name="task">任务视图</param>
        /// <param name="processList">流程列表</param>
        /// <param name="userList">用户列表</param>
        public static void FireAndForgetTask(Action<TaskViewEntity, IList<ProcessEntity>, IList<UserEMailEntity>> action,
            TaskViewEntity task,
            IList<ProcessEntity> processList,
            IList<UserEMailEntity> userList)
        {
            BackgroundJob.Enqueue(()=>action(task, processList, userList));
        }

        /// <summary>
        /// 异步任务加入队列
        /// </summary>
        /// <param name="func">操作</param>
        /// <param name="task">任务视图</param>
        /// <param name="processList">流程列表</param>
        /// <param name="userList">用户列表</param>
        public static void FireAndForgetTaskAsync(Func<TaskViewEntity, IList<ProcessEntity>, IList<UserEMailEntity>, Task> func,
            TaskViewEntity task,
            IList<ProcessEntity> processList,
            IList<UserEMailEntity> userList)
        {
            Func<Task> myFun = async () => await func(task, processList, userList);
            BackgroundJob.Enqueue(()=>myFun().Wait());
        }
    }
}