using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Business.Entity;
using Slickflow.Scheduler.Entity;
using Slickflow.Scheduler.Common;

namespace Slickflow.Scheduler.Service
{
    /// <summary>
    /// Job Schedule Service
    /// </summary>
    public interface ISchedulerService
    {
        IList<ProcessEntity> GetStartupTimingProcessList();
        IList<JobScheduleEntity> GetJobScheduleList();
        void UpdateJobScheduleStatus(JobScheduleEntity entity, JobScheduleStatusEnum status);

        //process timing
        void TerminateOverdueProcessInstance();
        void TerminateOverdueActivityInstance();
        string TriggerTimingStartupProcess(ProcessEntity entity);
    }
}
