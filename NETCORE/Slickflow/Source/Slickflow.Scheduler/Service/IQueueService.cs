using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Scheduler.Entity;
using Hangfire.Storage;

namespace Slickflow.Scheduler.Service
{
    /// <summary>
    /// job queue operation
    /// </summary>
    public interface IQueueService
    {
        void AddJobIntoQueue(JobScheduleEntity entity);
        void RemoveJobFromQueue(JobScheduleEntity entity);
        IList<JobHangFireEntity> FindQueueJobByName(string name);
        bool IsExistQueueJobByName(string name);
    }
}
