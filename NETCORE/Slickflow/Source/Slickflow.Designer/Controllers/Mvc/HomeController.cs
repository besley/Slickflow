using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace Slickflow.Designer.Controllers.Mvc
{
    /// <summary>
    /// 主页面控制器
    /// </summary>
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.ProcessID = Request.Query["ID"];
            return View();
        }

        public ActionResult Dialog()
        {
            return View();
        }

        public ActionResult Demo()
        {
            return View();
        }
    }
}