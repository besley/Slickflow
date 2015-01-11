using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Slickflow.MvcDemo.Data;
using Slickflow.MvcDemo.Data.Entity;
namespace Slickflow.MvcDemo.Controllers
{
    [Authorize]
    public class SlickflowController : Controller
    {
        // GET: Slickflow
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 我发起的流程
        /// </summary>
        /// <returns></returns>
        public ActionResult MySlickflow()
        {
            WorkFlowManager work = new WorkFlowManager();
            IEnumerable<ProcessInstance> processInstance = work.GetProcessInstance(User.Identity.GetUserId());
            return View(processInstance);
        }

        /// <summary>
        /// 当前用户代办任务列表（一次加载所有数据）
        /// </summary>
        /// <returns></returns>
        public ActionResult FlowList()
        {
            IWorkflowService service = new WorkflowService();
            TaskQueryEntity en = new TaskQueryEntity
            {
                UserID = User.Identity.GetUserId()
            };
            IList<TaskViewEntity> taskViewList = service.GetReadyTasks(en);
            return View(taskViewList);
        }
    }
}