using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace Slickflow.Designer.Controllers.Mvc
{
    /// <summary>
    /// 活动属性控制器
    /// </summary>
    public class ActivityController : Controller
    {
        // GET: Activity
        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Gateway()
        {
            return View();
        }

        public IActionResult SubProcess()
        {
            return View();
        }

        public IActionResult Event()
        {
            return View();
        }

        public IActionResult EndEvent()
        {
            return View();
        }
    }
}