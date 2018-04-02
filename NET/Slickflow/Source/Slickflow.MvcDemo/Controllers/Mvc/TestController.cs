using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.IO;

namespace Slickflow.MvcDemo.Controllers.Mvc
{
    /// <summary>
    /// 任务的执行者对象
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
    /// 执行者列表类
    /// </summary>
    public class PerformerList : List<Performer>
    {
        public PerformerList()
        {
        }
    }

    /// <summary>
    /// 流程执行人(业务应用的办理者)
    /// </summary>
    public class WfAppRunner
    {
        public string AppName { get; set; }
        public string AppInstanceID { get; set; }
        public string AppInstanceCode { get; set; }
        public string ProcessGUID { get; set; }
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

        [HttpPost]
        [AllowAnonymous]
        public string GoNext()
        {

            Request.InputStream.Position = 0;
            var input = new StreamReader(Request.InputStream).ReadToEnd();
            var appRunner = JsonConvert.DeserializeObject<WfAppRunner>(input);

            return appRunner.UserName;
        }
    }
}