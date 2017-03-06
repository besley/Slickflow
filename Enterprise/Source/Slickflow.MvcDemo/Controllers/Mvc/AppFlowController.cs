using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Slickflow.MvcDemo.Controllers.Mvc
{
    /// <summary>
    /// 业务流程应用控制器
    /// </summary>
    public class AppFlowController : BaseMvcController
    {
        /// <summary>
        /// 显示流程图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult KGraph(string id)
        {
            ViewBag.ProcessGUID = id;
            return View();
        }
    }
}