using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;
using Slickflow.Scheduler.Common;
using Slickflow.Scheduler.Data;
using Slickflow.Scheduler.Entity;

namespace Slickflow.Scheduler.Service
{
    /// <summary>
    /// Job Scheduler Service
    /// </summary>
    public class SchedulerService : ISchedulerService
    {
        /// <summary>
        /// Terminiated overdue process instance
        /// </summary>
        public void TerminateOverdueProcessInstance()
        {
            //query expired process instance record
            int[] idsin;
            using (var session = DbFactory.CreateSession())
            {
                var piDbSet = session.GetRepository<ProcessInstanceEntity>().GetDbSet();
                var sqlQuery = (from pi in piDbSet
                                where pi.OverdueDateTime < System.DateTime.Now
                                    && pi.OverdueTreatedDateTime == null
                                    && (pi.ProcessState == 1 || pi.ProcessState == 2 || pi.ProcessState == 5)
                                select pi.ID);
                var piList = sqlQuery.ToList();
                idsin = piList.ToArray();
            }

            if (idsin.Length == 0) return;

            //there is overdue process instance to be waited to get treatment
            using (var session = DbFactory.CreateSession())
            {
                try
                {
                    session.BeginTrans();
                    var list = session.GetRepository<ProcessInstanceEntity>()
                        .Query(e => idsin.Contains(e.ID))
                        .ToList();

                    list.ForEach(p => {
                        p.ProcessState = (short)ProcessStateEnum.Terminated;
                        p.OverdueDateTime = System.DateTime.Now;
                        p.EndedDateTime = System.DateTime.Now;
                        p.EndedByUserID = "ADMIN-1001";
                        p.EndedByUserName = "JOB-ADMINISTRATOR";
                    });

                    //insert job record
                    var jobEntity = new JobEntity
                    {
                        RefClass = JobRefClassEnum.Process.ToString(),
                        RefIDs = string.Join(",", idsin),
                        ScheduleName = JobScheduleNameEnum.TerminateOverdueProcessInstance.ToString(),
                        Status = 1,
                        Message = "SUCCESS",
                        CreatedDateTime = System.DateTime.Now,
                        CreatedByUserID = "ADMIN-1001",
                        CreatedByUserName = "JOB-ADMINISTRATOR"
                    };
                    session.GetRepository<JobEntity>().Insert(jobEntity);
                    session.SaveChanges();

                    session.Commit();
                }
                catch(System.Exception ex)
                {
                    session.Rollback();
                }
            }
        }

        
        /// <summary>
        /// get timing startup process list
        /// </summary>
        /// <returns></returns>
        public IList<ProcessEntity> GetStartupTimingProcessList()
        {
            //var selSql = @"SELECT *
            //               FROM WfProcess
            //               WHERE StartUp=1";
            using (var session = DbFactory.CreateSession())
            {
                var pDbSet = session.GetRepository<ProcessEntity>().GetDbSet();
                var list = (from p in pDbSet
                            where p.StartType == 1
                            select p).ToList();
                return list;
            }
        }

        /// <summary>
        /// trigger the process startup
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string TriggerTimingStartupProcess(ProcessEntity entity)
        {
            var appRunner = new WfAppRunner();
            appRunner.AppName = "定时启动作业";
            appRunner.AppInstanceID = "9000";
            appRunner.UserID = "-1000";
            appRunner.UserName = "自动作业执行员";
            appRunner.ProcessGUID = entity.ProcessGUID;
            appRunner.Version = entity.Version;
            
            var wfService = new WorkflowService();
            var result = wfService.StartProcess(appRunner);
            return result.Message;
        }
    }
}
