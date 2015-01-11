using Slickflow.MvcDemo.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Slickflow.Data;
using Dapper;
using Slickflow.Engine.Business.Entity;
using System.Threading.Tasks;
using System.Data;

namespace Slickflow.MvcDemo.Data
{

    /// <summary>
    /// 尽量与WebDemo保持一致,ORM使用dapper
    /// </summary>
    public class WorkFlowManager : ManagerBase
    {

        public HrsLeaveResult Insert(LeaveEntity leave)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                leave.CreatedDate = System.DateTime.Now;
                session.BeginTrans();
                dynamic result = Repository.Insert<LeaveEntity>(session.Connection, leave, session.Transaction);
                session.Commit();


                if (result > 0)
                {
                    HrsLeaveResult.ResultIdentity = result;
                    return HrsLeaveResult.Success;
                }
                else
                {
                    return HrsLeaveResult.Failed("");
                }
            }
            catch (System.Exception exception)
            {
                session.Rollback();
                return HrsLeaveResult.Failed(exception.Message);
            }
            finally
            {
                session.Dispose();
            }
        }

        public HrsLeaveResult Insert(BizAppFlowEntity bizapp)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                var result = Repository.Insert<BizAppFlowEntity>(session.Connection, bizapp);
                if (result > 0)
                {
                    HrsLeaveResult.ResultIdentity = result;
                    return HrsLeaveResult.Success;
                }
                else
                {
                    return HrsLeaveResult.Failed("");
                }
            }
            catch (System.Exception exception)
            {
                return HrsLeaveResult.Failed(exception.Message);
            }
            finally
            {
                session.Dispose();
            }
        }
        /// <summary>
        /// 根据appinstanceid 获取请假信息数据
        /// </summary>
        /// <param name="appinstanceId">请假单主键</param>
        /// <returns></returns>
        public async Task<LeaveEntity> FindByIdAsync(int appinstanceId)
        {
            IDbSession session = SessionFactory.CreateSession();
            string sql = @"SELECT dbo.HrsLeave.ID,CASE  dbo.HrsLeave.LeaveType
                          WHEN 1 THEN '病假' WHEN 2 THEN '事假'
                          WHEN 3 THEN '丧假'
                          WHEN 4 THEN '产假'
                          WHEN 5 THEN '工伤假'
                          WHEN 6 THEN '婚假'
                          WHEN 7 THEN '年休假'
                          ELSE '其他假' end
                          AS  LeaveType ,[Days], FromDate,ToDate,CreatedDate,
                           dbo.AspNetUsers.RealName as  CreatedUserName,
                           DepManagerRemark,DeputyGeneralRemark,DirectorRemark,GeneralManagerRemark FROM dbo.HrsLeave
                           INNER JOIN dbo.AspNetUsers ON dbo.HrsLeave.CreatedUserID=dbo.AspNetUsers.Id
                           WHERE dbo.HrsLeave.id=@appinstanceId ";
            try
            {
                return await Repository.Query(session.Connection, sql, new { appinstanceId = appinstanceId }, trans: session.Transaction).FirstOrDefault();
            }
            finally{
                session.Dispose();
            }

        }

        /// <summary>
        /// 根据appinstanceid 获取请假信息数据
        /// </summary>
        /// <param name="appinstanceId">请假单主键</param>
        /// <returns></returns>
        public LeaveEntity FindById(int appinstanceId)
        {
            string sql = @"SELECT dbo.HrsLeave.ID,  dbo.HrsLeave.LeaveType
                        ,[Days], FromDate,ToDate,CreatedDate,
                           dbo.AspNetUsers.RealName as  CreatedUserName,
                           DepManagerRemark,DeputyGeneralRemark,DirectorRemark,GeneralManagerRemark FROM dbo.HrsLeave
                           INNER JOIN dbo.AspNetUsers ON dbo.HrsLeave.CreatedUserID=dbo.AspNetUsers.Id
                           WHERE dbo.HrsLeave.id=@appinstanceId ";
            return Repository.Query<LeaveEntity>(sql, new { appinstanceId = appinstanceId }).FirstOrDefault();
        }
        /// <summary>
        /// 获取我申请的
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<ProcessInstance> GetProcessInstance(string userId)
        {
            string sql = @"SELECT dbo.WfProcessInstance.AppInstanceID, dbo.WfProcessInstance.ProcessName,dbo.AspNetUsers.RealName,dbo.WfTasks.CreatedDateTime,wfprocessinstance.ProcessState,AssignedToUserName FROM dbo.WfTasks
                           INNER JOIN dbo.WfProcessInstance ON wftasks.appinstanceid=dbo.WfProcessInstance.AppInstanceID
                           INNER JOIN dbo.AspNetUsers  ON dbo.WfProcessInstance.CreatedByUserName=dbo.AspNetUsers.UserName
                           WHERE TaskState=1 AND dbo.WfProcessInstance.CreatedByUserID =@userId";
            return Repository.Query<ProcessInstance>(sql, new { userId = userId });
        }
        public bool UpdateHrsLeave(LeaveEntity leave)
        {
            //IDbSession conn = SessionFactory.CreateSession();
            IDbConnection conn = SessionFactory.CreateConnection();
            string sql = @"UPDATE [dbo].[HrsLeave]
                   SET 
                      [DepManagerRemark] = @DepManagerRemark,
                      [DirectorRemark] = @DirectorRemark,
                      [DeputyGeneralRemark] = @DeputyGeneralRemark,
                      [GeneralManagerRemark] =@GeneralManagerRemark
                 WHERE id=@Id";
            try
            {
                string s = Repository.Execute(conn, sql, new
                {
                    DepManagerRemark = leave.DepManagerRemark,
                    DirectorRemark = leave.DirectorRemark,
                    DeputyGeneralRemark = leave.DeputyGeneralRemark,
                    GeneralManagerRemark = leave.GeneralManagerRemark,
                    Id = leave.ID
                }).ToString();

                if (string.IsNullOrEmpty(s))
                {
                    return false;
                }
                else
                {

                    return true;
                }
            }
            finally
            {
                conn.Close();
            }
        }
    }
    public class HrsLeaveResult
    {
        private static readonly HrsLeaveResult _success = new HrsLeaveResult(true);

        public HrsLeaveResult(bool success)
        {
            this.Successed = success;
            this.Errors = new string[0];

        }
        public HrsLeaveResult(IEnumerable<string> errors)
        {
            if (errors == null)
            {
                errors = new string[] { "默认错误" };
            }
            this.Successed = false;
            this.Errors = errors;
        }
        public static HrsLeaveResult Failed(params string[] errors)
        {
            return new HrsLeaveResult(errors);
        }
        public IEnumerable<string> Errors { get; private set; }
        public bool Successed { get; private set; }
        public int ResultIdentities
        {
            get { return ResultIdentity; }

            set { ResultIdentity = value; }
        }

        public static int ResultIdentity { get; set; }
        public static HrsLeaveResult Success
        {
            get
            {
                return _success;
            }
        }

        //插入或结果,返回主键值


    }
}