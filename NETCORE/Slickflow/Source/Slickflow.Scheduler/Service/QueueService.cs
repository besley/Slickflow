using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;
using Slickflow.Scheduler.Entity;
using Slickflow.Scheduler.Common;
using Slickflow.Scheduler.Config;
using Hangfire;
using Hangfire.Storage;

namespace Slickflow.Scheduler.Service
{
    /// <summary>
    /// job queue service
    /// </summary>
    public class QueueService : IQueueService
    {
        /// <summary>
        /// add job into queue
        /// </summary>
        /// <param name="entity">entity</param>
        public void AddJobIntoQueue(JobScheduleEntity entity)
        {
            if (entity.ScheduleName == JobScheduleNameEnum.TriggerTimingStartupProcess.ToString())
            {
                AddJobOfTriggerTimingStartupProcess(entity);
            }
            else if (entity.ScheduleName == JobScheduleNameEnum.TerminateOverdueProcessInstance.ToString())
            {
                AddJobOfTerminateOverdueProcessInstance(entity);
            }
            else if (entity.ScheduleName == JobScheduleNameEnum.TerminateOverdueActivityInstance.ToString())
            {
                AddJobOfTerminateOverdueActivityInstance(entity);
            }
            else if (entity.ScheduleName == JobScheduleNameEnum.SendTaskEMail.ToString())
            {
                AddJobOfTaskEMailSending(entity);
            }
            else
            {
                throw new ApplicationException("不存在的作业名称！");
            }

            //update schedule entity statuls
            var scheduleService = new SchedulerService();
            scheduleService.UpdateJobScheduleStatus(entity, JobScheduleStatusEnum.Running);
        }

        /// <summary>
        /// add recurring job of terminate overdue process
        /// </summary>
        /// <param name="entity">job schedule entity</param>
        private void AddJobOfTerminateOverdueProcessInstance(JobScheduleEntity entity)
        {
            RecurringJob.AddOrUpdate<SchedulerService>(entity.ScheduleGUID,
                s => s.TerminateOverdueProcessInstance(), 
                entity.CronExpression);
        }

        /// <summary>
        /// add recurring job of terminate overdue process
        /// </summary>
        /// <param name="entity">job schedule entity</param>
        private void AddJobOfTerminateOverdueActivityInstance(JobScheduleEntity entity)
        {
            RecurringJob.AddOrUpdate<SchedulerService>(entity.ScheduleGUID,
                s => s.TerminateOverdueActivityInstance(),
                entity.CronExpression);
        }

        /// <summary>
        /// add recurring job of timing startup process
        /// </summary>
        /// <param name="entity">job schedule entity</param>
        private void AddJobOfTriggerTimingStartupProcess(JobScheduleEntity entity)
        {
            var schedulerService = new SchedulerService();
            List<ProcessEntity> processList = schedulerService.GetStartupTimingProcessList().ToList();

            foreach (var p in processList)
            {
                if (p.StartType == (short)ProcessStartTypeEnum.Timer && !String.IsNullOrEmpty(p.StartExpression))
                {
                    //job id should be unique
                    //job id combination of schedule guid and processid
                    string newJobId = string.Format("{0}#{1}", entity.ScheduleGUID, p.ID);
                    RecurringJob.AddOrUpdate<SchedulerService>(newJobId,
                        s => s.TriggerTimingStartupProcess(p),
                        p.StartExpression,
                        TimeZoneInfo.Local);
                }
            }
        }

        /// <summary>
        /// add recurring job of sending task email
        /// </summary>
        /// <param name="entity">job schedule entity</param>
        private void AddJobOfTaskEMailSending(JobScheduleEntity entity)
        {
            //get whether or not sending email when task in read state
            if (JobAdminDefine.EMailSendUtility_SendEMailFlag == 1)
            {
                var wfService = new WorkflowService();
                var processList = wfService.GetProcessListSimple();
                var emailService = new EMailService();
                var userList = emailService.GetUserList();

                RecurringJob.AddOrUpdate<EMailService>(entity.ScheduleGUID,
                    s => s.SendTaskEMail(processList, userList),
                    entity.CronExpression);
            }
        }

        /// <summary>
        /// remove recurring job 
        /// </summary>
        /// <param name="entity">job schedule entity</param>
        public void RemoveJobFromQueue(JobScheduleEntity entity)
        {
            if (entity.ScheduleName == JobScheduleNameEnum.TriggerTimingStartupProcess.ToString())
            {
                RemoveJobOfTriggerTimingStartupProcess(entity);
            }
            else if (entity.ScheduleName == JobScheduleNameEnum.TerminateOverdueProcessInstance.ToString())
            {
                RemoveJobOfTerminateOverdueProcessInstance(entity);
            }
            else if (entity.ScheduleName == JobScheduleNameEnum.TerminateOverdueActivityInstance.ToString())
            {
                RemoveJobOfTerminateOverdueActivityInstance(entity);
            }
            else if (entity.ScheduleName == JobScheduleNameEnum.SendTaskEMail.ToString())
            {
                RemoveJobOfTaskEMailSending(entity);
            }
            else
            {
                throw new ApplicationException("不存在的作业名称！");
            }

            //update schedule entity statuls
            var scheduleService = new SchedulerService();
            scheduleService.UpdateJobScheduleStatus(entity, JobScheduleStatusEnum.Ready);
        }

        /// <summary>
        /// remove recurring job of terminate overdue process
        /// </summary>
        /// <param name="entity">job schedule entity</param>
        private void RemoveJobOfTerminateOverdueProcessInstance(JobScheduleEntity entity)
        {
            RecurringJob.RemoveIfExists(entity.ScheduleGUID);
        }

        /// <summary>
        /// remove recurring job of terminate overdue activity
        /// </summary>
        /// <param name="entity">job schedule entity</param>
        private void RemoveJobOfTerminateOverdueActivityInstance(JobScheduleEntity entity)
        {
            RecurringJob.RemoveIfExists(entity.ScheduleGUID);
        }

        /// <summary>
        /// remove recurring job of sending task email
        /// </summary>
        /// <param name="entity">job schedule entity</param>
        private void RemoveJobOfTaskEMailSending(JobScheduleEntity entity)
        {
            RecurringJob.RemoveIfExists(entity.ScheduleGUID);
        }

        /// <summary>
        /// remove recurring job of timing startup process
        /// </summary>
        /// <param name="entity">job schedule entity</param>
        private void RemoveJobOfTriggerTimingStartupProcess(JobScheduleEntity entity)
        {
            var schedulerService = new SchedulerService();
            List<ProcessEntity> processList = schedulerService.GetStartupTimingProcessList().ToList();

            foreach (var p in processList)
            {
                //job id should be unique
                //job id combination of schedule guid and processid
                string newJobId = string.Format("{0}#{1}", entity.ScheduleGUID, p.ID);
                RecurringJob.RemoveIfExists(newJobId);
            }
        }

        /// <summary>
        /// query job by id
        /// (multiple id compsite a name)
        /// </summary>
        /// <param name="name"></param>
        public IList<JobHangFireEntity> FindQueueJobByName(string name)
        {
            IList<JobHangFireEntity> fireList = new List<JobHangFireEntity>();
            var jobList = JobStorage.Current.GetConnection().GetRecurringJobs();
            var childrenList = jobList.FindAll(j => j.Id.Contains(name));
            foreach (var job in childrenList)
            {
                var fire = new JobHangFireEntity
                {
                    ID = job.Id,
                    Cron = job.Cron,
                    LastJobID = job.LastJobId,
                    LastJobState = job.LastJobState,
                    CreateAt = job.CreatedAt
                };
                fireList.Add(fire);
            }
            return fireList;
        }

        /// <summary>
        /// check job exist
        /// </summary>
        /// <param name="name">job id</param>
        /// <returns></returns>
        public bool IsExistQueueJobByName(string name)
        {
            IList<JobHangFireEntity> fireList = new List<JobHangFireEntity>();
            var jobList = JobStorage.Current.GetConnection().GetRecurringJobs();
            var childrenList = jobList.FindAll(j => j.Id.Contains(name));
            var isExist = childrenList.Count() > 0;
            return isExist;
        }
    }
}
