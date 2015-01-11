using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Service;

namespace Slickflow.UnitTest
{
    [TestClass]
    public class ProcessRunningTest
    {
        /// <summary>
        /// 一个流程的完整测试（开始 -> 运行 -> 撤销 -> 运行 -> 退回 -> 运行 -> 结束 -> 返签 -> 运行 -> 结束）
        /// </summary>
        [TestMethod]
        public void StartupRunEnd()
        {
            IDbConnection conn = new SqlConnection(DBConfig.ConnectionString);
            conn.Open();
            
            
            ////StarterA:
            ////{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d"}
            var initiator = new WfAppRunner();
            initiator.AppName = "SamplePrice";
            initiator.AppInstanceID = 100;
            initiator.ProcessGUID = Guid.Parse("072af8c3-482a-4b1c-890b-685ce2fcc75d");
            initiator.UserID = 10;
            initiator.UserName = "Long";

            IWorkflowService service = new WorkflowService();

            //流程开始->业务员提交
            IDbTransaction trans = conn.BeginTransaction();
            try
            {
                service.StartProcess(conn, initiator, trans);
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                trans.Dispose();
            }

            //业务员提交->板房签字
            var banFangNodeGuid = "fc8c71c5-8786-450e-af27-9f6a9de8560f";
            PerformerList pList = new PerformerList();
            pList.Add(new Performer(20, "Zhang"));

            initiator.NextActivityPerformers = new Dictionary<Guid, PerformerList>();
            initiator.NextActivityPerformers.Add(Guid.Parse(banFangNodeGuid), pList);

            trans = conn.BeginTransaction();
            try
            {
                service.RunProcessApp(conn, initiator, trans);
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                trans.Dispose();
            }

            //板房签字->业务员签字
            //登录用户身份
            initiator.UserID = 20;
            initiator.UserName = "Zhang";

            var salesGuid = "39c71004-d822-4c15-9ff2-94ca1068d745";
            pList.Clear();
            pList.Add(new Performer(10, "Long"));

            initiator.NextActivityPerformers.Clear();
            initiator.NextActivityPerformers.Add(Guid.Parse(salesGuid), pList);
            trans = conn.BeginTransaction();
            try
            {
                service.RunProcessApp(conn, initiator, trans);
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                trans.Dispose();
            }

            //业务员签字->结束
            //登录用户身份
            initiator.UserID = 10;
            initiator.UserName = "Lhang";

            var endGuid = "b70e717a-08da-419f-b2eb-7a3d71f054de";
            pList.Clear();
            pList.Add(new Performer(10, "Long"));

            initiator.NextActivityPerformers.Clear();
            initiator.NextActivityPerformers.Add(Guid.Parse(endGuid), pList);
            trans = conn.BeginTransaction();
            try
            {
                service.RunProcessApp(conn, initiator, trans);
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                trans.Dispose();
            }

            if (conn.State == ConnectionState.Open)
                conn.Close();
        }
    }
}
