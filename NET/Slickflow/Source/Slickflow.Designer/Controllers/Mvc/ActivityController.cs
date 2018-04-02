using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Slickflow.Designer.Controllers.Mvc
{
    /// <summary>
    /// 活动属性控制器
    /// </summary>
    public class ActivityController : Controller
    {
        // GET: Activity
        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Gateway()
        {
            return View();
        }

        public ActionResult SubProcess()
        {
            return View();
        }
    }
}