using System;
using System.Diagnostics;
using System.Threading;
using Slickflow.Data;

namespace Slickflow.Module.Logging
{
    /// <summary>
    /// Log Service
    /// 日志服务类
    /// </summary>
    public class LogService : ILogService
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

        #region Add Record 
        /// <summary>
        /// 插入方法
        /// </summary>
        /// <param name="entity">实体对象</param>
        public void Record(object entity)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                var log = (JobLogEntity)entity;
                session.BeginTrans();
                QuickRepository.Insert<JobLogEntity>(session.Connection, log, session.Transaction);
                session.Commit();
            }
            catch (System.Exception)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }
        #endregion

        #region Static Method
        /// <summary>
        /// Record error log
        /// 作业日志记录
        /// </summary>
        /// <param name="jobType">job type</param>
        /// <param name="jobName">job name</param>
        /// <param name="jobKey">job key</param>
        /// <param name="refClass">application type</param>
        /// <param name="refIDs">application id</param>
        /// <param name="status">status</param>
        /// <param name="message">message</param>
        public static void RecordErrorLog(JobTypeEnum jobType,
            string jobName,
            string jobKey,
            string refClass,
            string refIDs,
            short status,
            string message,
            string stackTrace,
            string innerStackTrace,
            string requestData)
        {
            var entity = new JobLogEntity
            {
                JobType = jobType.ToString(),
                JobName = jobName,
                JobKey = jobKey,
                RefClass = refClass,
                RefIDs = refIDs,
                Status = status,
                Message = message,
                StackTrace = stackTrace,
                InnerStackTrace = innerStackTrace,
                RequestData = requestData,
                CreatedDateTime = System.DateTime.UtcNow,
                CreatedUserId = LogAdminDefine.ADMIN_USER_ID,
                CreatedUserName = LogAdminDefine.ADMIN_USER_NAME
            };

            LogService ls = new LogService();
            ThreadPool.QueueUserWorkItem(new WaitCallback(ls.Record), entity);
        }

        /// <summary>
        /// Record info log
        /// </summary>
        /// <param name="jobType">job type</param>
        /// <param name="jobName">job name</param>
        /// <param name="jobKey">job key</param>
        /// <param name="refClass">application type</param>
        /// <param name="refIDs">application id</param>
        /// <param name="status">status</param>
        /// <param name="message">message</param>
        public static void RecordInfoLog(JobTypeEnum jobType,
            string jobName,
            string jobKey,
            string refClass,
            string refIDs,
            short status,
            string message,
            string requestData)
        {
            var entity = new JobLogEntity
            {
                JobType = jobType.ToString(),
                JobName = jobName,
                JobKey = jobKey,
                RefClass = refClass,
                RefIDs = refIDs,
                Status = status,
                Message = message,
                RequestData = requestData,
                CreatedDateTime = System.DateTime.UtcNow,
                CreatedUserId = LogAdminDefine.ADMIN_USER_ID,
                CreatedUserName = LogAdminDefine.ADMIN_USER_NAME
            };

            LogService ls = new LogService();
            ThreadPool.QueueUserWorkItem(new WaitCallback(ls.Record), entity);
        }
        #endregion
    }
}
