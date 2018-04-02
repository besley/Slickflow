using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Scheduler.Service
{
    /// <summary>
    /// Job Schedule Service
    /// </summary>
    public interface ISchedulerService
    {
        void TerminateOverdueProcessInstance();
        IList<ProcessEntity> GetStartupTimingProcessList();
        string TriggerTimingStartupProcess(ProcessEntity entity);

    }
}
