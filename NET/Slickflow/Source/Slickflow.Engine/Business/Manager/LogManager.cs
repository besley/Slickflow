using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ServiceStack.Text;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;


namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// 日志处理记录类
    /// </summary>
    public class LogManager : ManagerBase
    {
        #region 获取日志数据
        ///// <summary>
        ///// 获取日志记录（分页）B
        ///// </summary>
        ///// <param name="query"></param>
        ///// <param name="activityState"></param>
        ///// <returns></returns>
        //private IEnumerable<LogEntity> GetLogsPaged(LogQueryEntity query, out int allRowsCount)
        //{
        //    IDbConnection conn = SessionFactory.CreateConnection();
        //    string orderBySql = "ORDER BY LogID DESC";

        //    //如果数据记录数为0，则不用查询列表
        //    allRowsCount = LogRepository.Count<LogEntity>(string.Empty, conn);
        //    if (allRowsCount == 0)
        //    {
        //        return null;
        //    }

        //    //查询列表数据并返回结果集
        //    var list = LogRepository.GetPage<LogEntity>(query.PageIndex, query.PageSize, out allRowsCount,
        //        conn,
        //        null);

        //    return list;
        //}
        #endregion

        #region 新增、更新和删除流程数据
        /// <summary>
        /// 记录流程异常日志
        /// </summary>
        /// <param name="entity"></param>
        public void Record(string title, 
            LogEventType eventType, 
            LogPriority priority, 
            object extraObject,
            System.Exception e)
        {
            try
            {
                var log = new LogEntity();
                log.EventTypeID = (int)eventType;
                log.Priority = (int)priority;
                log.Severity = priority.ToString().ToUpper();
                log.Title = title;
                log.Timestamp = DateTime.Now;
                log.Message = e.Message;
                if (e.StackTrace != null)
                {
                    log.StackTrace = e.StackTrace.Length > 4000 ? e.StackTrace.Substring(0, 4000) : e.StackTrace;
                }
                if (e.InnerException != null)
                {
                    log.InnerStackTrace = e.InnerException.StackTrace.Length > 4000 ? 
                        e.InnerException.StackTrace.Substring(0, 4000) : e.InnerException.StackTrace;
                }

                if (extraObject != null)
                {
                    var jsonData = JsonSerializer.SerializeToString(extraObject);
                    log.RequestData = jsonData.Length > 2000 ? jsonData.Substring(0, 2000) : jsonData;
                }

                //线程池添加日志记录
                ThreadPool.QueueUserWorkItem(new WaitCallback(Insert), log);
            }
            catch
            {
                //如果记录日志发生异常，不做处理
                ;
            }
        }

        /// <summary>
        /// 插入流程日志数据
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(object entity)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                var log = (LogEntity)entity;
                session.BeginTrans();
                Repository.Insert<LogEntity>(session.Connection, log, session.Transaction);
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
        public static void RecordLog(string title, 
            LogEventType eventType, 
            LogPriority priority, 
            object extraObject,
            System.Exception e)
        {
            LogManager lm = new LogManager();
            lm.Record(title, eventType, priority, extraObject, e);
        }
        #endregion
    }
}
