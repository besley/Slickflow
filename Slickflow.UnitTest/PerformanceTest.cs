using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Slickflow.UnitTest
{
    [TestClass]
    public class PerformanceTest
    {
        /// <summary>
        /// 并行运行测试（内部Workflow Thread）
        /// </summary>
        [TestMethod]
        public void StartupParalleled()
        {
            IDbConnection conn = new SqlConnection(DBConfig.ConnectionString);
            conn.Open();

            //StarterA:
            //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d"}
            var starterA = new WfAppRunner();
            starterA.ProcessGUID = Guid.Parse("072af8c3-482a-4b1c-890b-685ce2fcc75d");
            starterA.UserID = 10;
            starterA.UserName = "Long";

            var runnerA = new WfAppRunner();
            runnerA.ProcessGUID = Guid.Parse("072af8c3-482a-4b1c-890b-685ce2fcc75d");
            runnerA.UserID = 10;
            runnerA.UserName = "Long";

            PerformerList pList = new PerformerList();
            pList.Add(new Performer(20, "Xiao"));

            runnerA.NextActivityPerformers = new Dictionary<Guid, PerformerList>();
            runnerA.NextActivityPerformers.Add(Guid.Parse("fc8c71c5-8786-450e-af27-9f6a9de8560f"), pList);

            IWorkflowService serviceA;
            IDbTransaction trans = null;

            //STARTUP 2000 TIMES
            //for (var i = 0; i < 2000; i++)
            //{
            //    serviceA = new WorkflowService();
            //    starterA.AppName = "price";
            //    starterA.AppInstanceID = i;
            //    try
            //    {
            //        trans = conn.BeginTransaction();
            //        serviceA.StartProcess(conn, starterA, trans);
            //        trans.Commit();
            //    }
            //    catch
            //    {
            //        trans.Rollback();
            //        throw;
            //    }
            //    finally
            //    {
            //        trans.Dispose();
            //    }
            //}

            ////RUN process 2000 TIMES
            for (var i = 0; i < 2000; i++)
            {
                serviceA = new WorkflowService();
                runnerA.AppInstanceID = i;
                runnerA.AppName = "price";
                try
                {
                    trans = conn.BeginTransaction();
                    serviceA.RunProcessApp(conn, runnerA, trans);
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
            }

            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

         public void SetTaskRead2000()
        {
            WfAppRunner taskRunner = new WfAppRunner();
            taskRunner.TaskID = 18014;
            taskRunner.UserID = 10;
            taskRunner.UserName = "Long";

            IWorkflowService serviceA;
            for (var i = 0; i < 2000; i++)
            {
                serviceA = new WorkflowService();
                serviceA.SetTaskRead(taskRunner);
            }
        }
    }
}
