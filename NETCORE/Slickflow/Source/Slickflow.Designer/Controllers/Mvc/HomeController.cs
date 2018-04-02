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
        public IActionResult Index()
        {
            //ViewBag.ProcessID = base.Request.QueryString["ID"];
            return View();
        }

        public IActionResult Dialog()
        {
            return View();
        }

        public IActionResult Demo()
        {
            return View();
        }
    }
}