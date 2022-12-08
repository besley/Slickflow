using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Service;
using Slickflow.Engine.Core.Result;
using Slickflow.HrsService.Entity;
using Slickflow.HrsService.Interface;
using Slickflow.HrsService.Utility;

namespace Slickflow.HrsService.Service
{
    public class HrsLeaveService : ServiceBase, IHrsLeaveService, IWfServiceRegister
    {
        #region IWorkflowRegister members
        public WfAppRunner WfAppRunner
        {
            get;
            set;
        }

        public void RegisterWfAppRunner(WfAppRunner runner)
        {
            WfAppRunner = runner;
        }
        #endregion

        public HrsLeaveEntity GetByID(int id)
        {
            var entity = QuickRepository.GetById<HrsLeaveEntity>(id);
            return entity;
        }

        public List<HrsLeaveEntity> QueryLeave(HrsLeaveQuery query)
        {
            var sqlQuery = (from leave in QuickRepository.GetAll<HrsLeaveEntity>()
                            where leave.CreatedUserID == query.CreatedUserID
                            orderby leave.ID descending
                            select leave);    
            var list = sqlQuery.ToList();
            return list;
        }

        public List<HrsLeaveEntity> GetPaged(HrsLeaveQuery query, out int count)
        {
            throw new NotImplementedException();
        }

        public void Submit(HrsLeaveEntity entity)
        {
            var runner = WfAppRunner;

            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                entity.CreatedDateTime = System.DateTime.Now;
                entity.ToDate = entity.FromDate.AddDays(Decimal.ToDouble(entity.Days));
                var appInstanceID = QuickRepository.Insert<HrsLeaveEntity>(session.Connection, entity, session.Transaction);

                //保存流程中关于业务信息的关联信息
                runner.AppInstanceID = appInstanceID.ToString();
                runner.AppInstanceCode = string.Format("A4L-{0}", appInstanceID.ToString());

                var wfService = new WorkflowService();
                var wfResult = wfService.StartProcess(session.Connection, runner, session.Transaction);
                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    session.Commit();
                }
                else
                {
                    session.Rollback();
                    throw new ApplicationException(wfResult.Message);
                }
            }
            catch(System.Exception ex)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        public void Update(HrsLeaveEntity entity)
        {
            QuickRepository.Update<HrsLeaveEntity>(entity);
        }
    }
}
