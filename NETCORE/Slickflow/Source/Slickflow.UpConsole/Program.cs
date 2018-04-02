using System;
using System.Data;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Service;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Data;


namespace Slickflow.UpConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //var message = StartProcessTest();
            //Console.WriteLine(message);
            DataTest();

            Console.ReadLine();
        }

        static void DataTest()
        {
            var strConn = "Data Source = 127.0.0.1; Initial Catalog = WfDBCore; Integrated Security = False; User ID = sa; Password = sasa; Connect Timeout = 15; Encrypt = False; TrustServerCertificate = False";
            ConnectionString.Value = strConn;

            using (var session = new DbSession(new WfDbContext()))
            {
                var logRepository = session.GetRepository<LogEntity>();
                var log = new LogEntity
                {
                    Message = "test",
                    Priority = 0,
                    Severity = "High",
                    EventTypeID = 10,
                    Title = "test",
                    Timestamp = System.DateTime.Now
                };
                var entity2 = logRepository.Insert(log);

                session.SaveChanges();
                var newID = entity2.ID;

                Console.WriteLine(string.Format("the new id:{0}", entity2.ID));
            }
        }

        /// <summary>
        /// {"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d"}
        /// </summary>
        static string StartProcessTest()
        {
            var message = string.Empty;
            try
            {
                var wfService = new WorkflowService();
                var starter = new WfAppRunner
                {
                    UserID = "10",
                    UserName = "Long",
                    AppName = "SamplePrice",
                    AppInstanceID = "100",
                    ProcessGUID = "072af8c3-482a-4b1c-890b-685ce2fcc75d"
                };

                var result = wfService.StartProcess(starter);
                message = result.Message;
            }
            catch (System.Exception)
            {
                throw;
            }
            return message;
        }
    }
}
