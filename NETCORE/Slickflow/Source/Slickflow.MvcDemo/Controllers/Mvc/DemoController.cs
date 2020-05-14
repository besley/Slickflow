using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace Slickflow.MvcDemo.Controllers.Mvc
{
    public class DemoController : Controller
    {
        //
        // GET: /Demo/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NextStep()
        {
            return View();
        }

        public ActionResult JsTree()
        {
            return View();
        }

        public ActionResult ZTree()
        {
            return View();
        }

        public ActionResult Dialog()
        {
            return View();
        }
	}
}