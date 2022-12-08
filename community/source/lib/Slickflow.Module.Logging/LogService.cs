using System;
using System.Threading;
using Slickflow.Data;

namespace Slickflow.Module.Logging
{
    /// <summary>
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

        #region 新增作业日志记录
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

        #region 静态方法
        /// <summary>
        /// 作业日志记录
        /// </summary>
        /// <param name="jobType">作业类型</param>
        /// <param name="jobName">作业名称</param>
        /// <param name="jobKey">作业主键</param>
        /// <param name="refClass">应用类别</param>
        /// <param name="refIDs">关键ID列表</param>
        /// <param name="status">状态</param>
        /// <param name="message">消息</param>
        public static void RecordLog(JobTypeEnum jobType,
            string jobName,
            string jobKey,
            string refClass,
            string refIDs,
            short status,
            string message,
            string stackTrace = null)
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
                CreatedDateTime = System.DateTime.Now,
                CreatedByUserID = LogAdminDefine.ADMIN_USER_ID,
                CreatedByUserName = LogAdminDefine.ADMIN_USER_NAME
            };

            LogService ls = new LogService();
            ThreadPool.QueueUserWorkItem(new WaitCallback(ls.Record), entity);
        }
        #endregion
    }
}
