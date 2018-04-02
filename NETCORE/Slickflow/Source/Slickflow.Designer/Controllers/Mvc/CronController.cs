using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace Slickflow.Designer.Controllers.Mvc
{
    /// <summary>
    /// Cron Expression Editor
    /// </summary>
    public class CronController : Controller
    {
        // GET: Cron
        public IActionResult Edit()
        {
            return View();
        }
    }
}