using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Slickflow.Engine.Business.Entity;
using Slickflow.Scheduler.Service;
using Hangfire;

namespace Slickflow.Scheduler.Web.Models
{
    /// <summary>
    /// 调度作业模型
    /// </summary>
    public class SchedulerModel
    {
        /// <summary>
        /// 增加调度作业
        /// </summary>
        public void AddSchedulerJob()
        {
            AddJobOfTerminateOverdueProcessInstance();
            //AddJobOfTriggerTimingStartupProcess();
        }

        /// <summary>
        /// 流程逾期结束
        /// </summary>
        private void AddJobOfTerminateOverdueProcessInstance()
        {
            RecurringJob.AddOrUpdate<SchedulerService>(s => s.TerminateOverdueProcessInstance(), Cron.Minutely);
        }

        /// <summary>
        /// 定时启动作业
        /// </summary>
        private void AddJobOfTriggerTimingStartupProcess()
        {
            var schedulerService = new SchedulerService();
            List<ProcessEntity> processList = schedulerService.GetStartupTimingProcessList().ToList();

            foreach (var entity in processList)
            {
                if (entity.StartType == 1 && !String.IsNullOrEmpty(entity.StartExpression))
                {
                    RecurringJob.AddOrUpdate<SchedulerService>(entity.ProcessGUID,
                        s => s.TriggerTimingStartupProcess(entity),
                        entity.StartExpression,
                        TimeZoneInfo.Local);
                    
                }
            }
        }
    }
}