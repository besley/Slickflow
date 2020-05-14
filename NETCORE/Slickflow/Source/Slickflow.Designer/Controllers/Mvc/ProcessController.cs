using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace Slickflow.Designer.Controllers.Mvc
{
    /// <summary>
    /// 流程定义控制器
    /// </summary>
    public class ProcessController : Controller
    {
        // GET: Process
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult XmlContent()
        {
            return View();
        }

        public ActionResult Import()
        {
            return View();
        }
    }
}