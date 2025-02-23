using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace Slickflow.MvcDemo.Controllers.Mvc
{
    /// <summary>
    /// Performer for tasks
    /// </summary>
    public class Performer
    {
        public Performer(string userID, string userName)
        {
            UserID = userID;
            UserName = userName;
        }

        public string UserID
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Performer list
    /// </summary>
    public class PerformerList : List<Performer>
    {
        public PerformerList()
        {
        }
    }

    /// <summary>
    /// Workflow Application Runner
    /// </summary>
    public class WfAppRunner
    {
        public string AppName { get; set; }
        public string AppInstanceID { get; set; }
        public string AppInstanceCode { get; set; }
        public string ProcessID { get; set; }
        public string Version { get; set; }
        public string FlowStatus { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public IDictionary<string, PerformerList> NextActivityPerformers { get; set; }
    }

    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }
    }
}