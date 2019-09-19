using System;
using System.Collections.Generic;
using System.Linq;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;
using Slickflow.Scheduler.Common;
using Slickflow.Scheduler.Config;
using Slickflow.Scheduler.Entity;
using Slickflow.Scheduler.Manager;

namespace Slickflow.Scheduler.Service
{
    /// <summary>
    /// Job Scheduler Service
    /// </summary>
    public class SchedulerService : ISchedulerService
    {
        #region Repository
        private Repository quickRepository = null;
        private Repository QuickRepository
        {
            get
            {
                if (quickRepository == null)
                    quickRepository = new Repository();
                return quickRepository;
            }
        }
        #endregion

        /// <summary>
        /// get timing startup process list
        /// </summary>
        /// <returns></returns>
        public IList<ProcessEntity> GetStartupTimingProcessList()
        {
            var selSql = @"SELECT 
                                *
                           FROM WfProcess WITH (NOLOCK)
                           WHERE StartType=1
                                AND IsUsing=1";
            var processList = QuickRepository.Query<ProcessEntity>(selSql).ToList<ProcessEntity>();
            return processList;
        }


        /// <summary>
        /// get job schedule list
        /// </summary>
        /// <returns></returns>
        public IList<JobScheduleEntity> GetJobScheduleList()
        {
            var selSql = @"SELECT 
                                *
                           FROM WhJobSchedule
                           ORDER BY Status DESC";
            var scheduleList = QuickRepository.Query<JobScheduleEntity>(selSql).ToList<JobScheduleEntity>();
            var queueService = new QueueService();
            foreach (var s in scheduleList)
            {
                var isJobExist = queueService.IsExistQueueJobByName(s.ScheduleGUID);
                var status = isJobExist == true ? 1 : 0;
                if (s.Status != status)
                {
                    var enumValue = (JobScheduleStatusEnum)Enum.Parse(typeof(JobScheduleStatusEnum), status.ToString());
                    UpdateJobScheduleStatus(s, enumValue);
                    s.Status = (short)status;
                }
            }
            return scheduleList;
        }

        /// <summary>
        /// Terminiated overdue process instance
        /// </summary>
        public void TerminateOverdueProcessInstance()
        {
            //query expired process instance record
            var selSql = @"SELECT 
                                ID
                           FROM WfProcessInstance WITH (NOLOCK)
                           WHERE OverdueDateTime<@overdueDateTime
                                AND OverdueTreatedDateTime IS NULL
                                AND (ProcessState=1 OR ProcessState=2 OR ProcessState=5)";      //ready, running and suspended
            var piList = QuickRepository.Query<ProcessInstanceEntity>(selSql, 
                new {
                    overdueDateTime =System.DateTime.Now
                }).ToList();
            var idsin = piList.Select(pi => pi.ID).ToList().ToArray();

            if (idsin.Length == 0) return;
                      
            //there is overdue process instance to be waited to get treatment
            var session = SessionFactory.CreateSession();
            var transaction = session.BeginTrans();
            try
            {
                //update overdue process instance into terminal status
                var updSql = @"UPDATE WfProcessInstance
                        SET ProcessState=@processState,
                            OverdueTreatedDateTime = @overdueTreatedDateTime,
                            EndedDateTime=@endedDateTime,
                            EndedByUserID=@endedByUserID,
                            EndedByUserName=@endedByUserName
                        WHERE ID IN @processInstanceIDs";
                QuickRepository.Execute(session.Connection, updSql, 
                    new {
                        processState = (short)ProcessStateEnum.Terminated,
                        overdueTreatedDateTime = System.DateTime.Now,
                        endedDateTime = System.DateTime.Now,
                        endedByUserID = JobAdminDefine.ADMIN_USER_ID,
                        endedByUserName = JobAdminDefine.ADMIN_USER_NAME,
                        processInstanceIDs = idsin
                    }, 
                    transaction);

                session.Commit();

                //insert job record
                JobLogManager.RecordLog(JobRefClassEnum.ProcessInstance.ToString(),
                    string.Join(",", idsin),
                    JobScheduleNameEnum.TerminateOverdueProcessInstance.ToString(),
                    1,
                    JobAdminDefine.ADMIN_MESSAGE_SUCCESS);
            }
            catch(System.Exception ex)
            {
                session.Rollback();
                //insert job record
                JobLogManager.RecordLog(JobRefClassEnum.ProcessInstance.ToString(),
                    string.Join(",", idsin),
                    JobScheduleNameEnum.TerminateOverdueProcessInstance.ToString(),
                    -1,
                    ex.Message,
                    ex.StackTrace);
                throw;
            }
            finally
            {
                session.Dispose();
            }            
        }

        /// <summary>
        /// Terminiated overdue process instance
        /// </summary>
        public void TerminateOverdueActivityInstance()
        {
            //query expired process instance record
            var selSql = @"SELECT 
                                A.ID 
                           FROM WfActivityInstance A WITH (NOLOCK) 
                           INNER JOIN WfProcessInstance B WITH (NOLOCK) 
                                ON A.ProcessInstanceID=B.ID 
                           WHERE A.OverdueDateTime<@overdueDateTime 
                                AND A.OverdueTreatedDateTime IS NULL 
                                AND (A.ActivityState = 1 OR A.ActivityState=2)  
                                AND (B.ProcessState=1 OR B.ProcessState=2)";    //process is ready, running and suspended
                                
            var aiList = QuickRepository.Query<ActivityInstanceEntity>(selSql,
                new
                {
                    overdueDateTime = System.DateTime.Now
                }).ToList();
            if (aiList.Count() > 0)
            {
                foreach (var a in aiList)
                {
                    RunProcessAppAuto(a.ID);
                }
            }
        }

        /// <summary>
        ///run process app automatically
        /// </summary>
        /// <param name="activityInstanceID">activity instance id</param>
        public void RunProcessAppAuto(int activityInstanceID)
        {
            var wfService = new WorkflowService();
            var taskView = wfService.GetFirstRunningTask(activityInstanceID);
            var nodeViewList = wfService.GetNextActivityTree(taskView.TaskID);

            //there is overdue activity instance to be waited to get treatment
            //update overdue activity instance into terminate status
            //activity state :9--Terminated
            var updSql = @"UPDATE WfActivityInstance
                        SET ActivityState=9,
                            OverdueTreatedDateTime = @overdueTreatedDateTime,
                            EndedDateTime=@endedDateTime,
                            EndedByUserID=@endedByUserID,
                            EndedByUserName=@endedByUserName
                        WHERE ID=@activityInstanceID";

            WfAppRunner runner = new WfAppRunner();
            foreach (var node in nodeViewList)
            {
                var performerList = wfService.GetPerformerList(node);       //get performer list from node
                Dictionary<string, PerformerList> dict = new Dictionary<string, PerformerList>();
                dict.Add(node.ActivityGUID, performerList);

                runner.AppName = taskView.AppName;
                runner.AppInstanceID = taskView.AppInstanceID;
                runner.ProcessGUID = taskView.ProcessGUID;
                runner.UserID = taskView.AssignedToUserID;
                runner.UserName = taskView.AssignedToUserName;
                runner.TaskID = taskView.TaskID;

                runner.NextActivityPerformers = dict;

                IDbSession session = SessionFactory.CreateSession();
                try
                {
                    var result = wfService.RunProcessApp(session.Connection, runner, session.Transaction);
                    if (result.Status == WfExecutedStatus.Success)
                    {
                        QuickRepository.Execute(session.Connection, updSql,
                        new
                        {
                            overdueTreatedDateTime = System.DateTime.Now,
                            endedDateTime = System.DateTime.Now,
                            endedByUserID = taskView.AssignedToUserID,
                            endedByUserName = taskView.AssignedToUserName,
                            activityInstanceID = activityInstanceID
                        },
                        session.Transaction);

                        session.Commit();

                        JobLogManager.RecordLog(JobRefClassEnum.ActivityInstance.ToString(), 
                            activityInstanceID.ToString(), 
                            JobScheduleNameEnum.TerminateOverdueActivityInstance.ToString(), 
                            1, 
                            JobAdminDefine.ADMIN_MESSAGE_SUCCESS);
                    }
                    else
                    {
                        JobLogManager.RecordLog(JobRefClassEnum.ActivityInstance.ToString(),
                            activityInstanceID.ToString(),
                            JobScheduleNameEnum.TerminateOverdueActivityInstance.ToString(),
                            -1,
                            result.Message); 
                    }
                }
                catch (System.Exception ex)
                {
                    session.Rollback();
                    JobLogManager.RecordLog(JobRefClassEnum.ActivityInstance.ToString(),
                        activityInstanceID.ToString(),
                        JobScheduleNameEnum.TerminateOverdueActivityInstance.ToString(),
                        -1,
                        ex.Message,
                        ex.StackTrace);
                }
                finally
                {
                    session.Dispose();
                }
            }
        }

        /// <summary>
        /// trigger the process startup
        /// </summary>
        /// <param name="entity">process entity</param>
        /// <returns>execution message</returns>
        public string TriggerTimingStartupProcess(ProcessEntity entity)
        {
            var message = string.Empty;
            var appRunner = new WfAppRunner {
                AppName = JobAdminDefine.ADMIN_APP_NAME,
                //AppInstanceID = JobAdminDefine.ADMIN_APP_INSTANCE_ID,
                //should get a new application instance id from other business tables, such as order table
                AppInstanceID = System.Guid.NewGuid().ToString(),       
                UserID = JobAdminDefine.ADMIN_USER_ID,
                UserName = JobAdminDefine.ADMIN_USER_NAME,
                ProcessGUID = entity.ProcessGUID,
                Version = entity.Version
            };

            try
            {
                var wfService = new WorkflowService();
                var result = wfService.StartProcess(appRunner);
                if (result.Status == WfExecutedStatus.Success)
                {
                    JobLogManager.RecordLog(JobRefClassEnum.Process.ToString(),
                        entity.ProcessGUID.ToString(),
                        JobScheduleNameEnum.TriggerTimingStartupProcess.ToString(),
                        1,
                        JobAdminDefine.ADMIN_MESSAGE_SUCCESS);
                }
                else
                {
                    JobLogManager.RecordLog(JobRefClassEnum.Process.ToString(),
                        entity.ProcessGUID,
                        JobScheduleNameEnum.TriggerTimingStartupProcess.ToString(),
                        -1,
                        result.Message);
                }
                message = result.Message;
            }
            catch(System.Exception ex)
            {
                JobLogManager.RecordLog(JobRefClassEnum.Process.ToString(),
                    entity.ProcessGUID,
                    JobScheduleNameEnum.TriggerTimingStartupProcess.ToString(),
                    -1,
                    ex.Message,
                    ex.StackTrace);
                message = ex.Message;
            }

            return message;
        }

        /// <summary>
        /// update job schedule status
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="status">status</param>
        public void UpdateJobScheduleStatus(JobScheduleEntity entity, JobScheduleStatusEnum status)
        {
            var scheduleEntity = QuickRepository.GetById<JobScheduleEntity>(entity.ID);
            scheduleEntity.Status = (short)status;
            scheduleEntity.LastUpdatedByUserID = JobAdminDefine.ADMIN_USER_ID;
            scheduleEntity.LastUpdatedByUserName = JobAdminDefine.ADMIN_USER_NAME;
            scheduleEntity.LastUpdatedDateTime = System.DateTime.Now;
            quickRepository.Update<JobScheduleEntity>(scheduleEntity);
        }
    }
}