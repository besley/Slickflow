using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack.Text;
using Slickflow.Data;
using Slickflow.Scheduler.Entity;
using Slickflow.Scheduler.Config;

namespace Slickflow.Scheduler.Manager
{

    /// <summary>
    /// 作业日志记录
    /// </summary>
    internal class JobLogManager
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

        #region 新增作业日志记录
        /// <summary>
        /// 作业日志记录
        /// </summary>
        /// <param name="jobType">作业类型</param>
        /// <param name="refIDs">关键ID列表</param>
        /// <param name="jobName">作业名称</param>
        /// <param name="status">状态</param>
        /// <param name="message">消息</param>
        /// <param name="stackTrace">详细错误</param>
        private void Record(string jobType,
            string refIDs,
            string jobName,
            short status,
            string message,
            string stackTrace)
        {
            try
            {
                var entity = new JobLogEntity
                {
                    RefClass = jobType,
                    RefIDs = refIDs,
                    ScheduleName = jobName,
                    Status = status,
                    Message = message,
                    StackTrace = stackTrace,
                    CreatedDateTime = System.DateTime.Now,
                    CreatedByUserID = JobAdminDefine.ADMIN_USER_ID,
                    CreatedByUserName = JobAdminDefine.ADMIN_USER_NAME
                };
                ThreadPool.QueueUserWorkItem(new WaitCallback(Insert), entity);
            }
            catch
            {
                ;
            }
        }

        /// <summary>
        /// 插入方法
        /// </summary>
        /// <param name="entity">实体对象</param>
        internal void Insert(object entity)
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

        #region 静态方法
        /// <summary>
        /// 作业日志记录
        /// </summary>
        /// <param name="jobType">作业类型</param>
        /// <param name="refIDs">关键ID列表</param>
        /// <param name="jobName">作业名称</param>
        /// <param name="status">状态</param>
        /// <param name="message">消息</param>
        internal static void RecordLog(string jobType,
            string refIDs,
            string jobName,
            short status,
            string message,
            string stackTrace = null)
        {
            JobLogManager jlm = new JobLogManager();
            jlm.Record(jobType, refIDs, jobName, status, message, stackTrace);
        }
        #endregion
    }
}

