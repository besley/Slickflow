using Slickflow.MvcDemo.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Slickflow.Data;
using Dapper;

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
                string sql = @"INSERT INTO [dbo].[HrsLeave]([LeaveType] ,[Days],[FromDate],[ToDate],[CreatedUserID],[CreatedUserName] )
                                                 VALUES(@LeaveType,@Days,@FromDate,@ToDate,@CreatedUserID,@CreatedUserName)                        
SELECT CAST(SCOPE_IDENTITY() as int)";
                DynamicParameters dynamic = new DynamicParameters();
                dynamic.AddDynamicParams(new
                {
                    LeaveType = leave.LeaveType,
                    Days = leave.Days,
                    FromDate = leave.FromDate,
                    ToDate = leave.ToDate,
                    CreatedUserID = leave.CreatedUserID,
                    CreatedUserName = leave.CreatedUserName
                });
                var result = Repository.InsertIdentity(session.Connection, sql, dynamic);


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