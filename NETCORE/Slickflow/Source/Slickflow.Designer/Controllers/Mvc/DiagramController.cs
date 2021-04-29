using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace Slickflow.Designer.Controllers.Mvc
{
    /// <summary>
    /// 流程图查看页面
    /// </summary>
    public class DiagramController : Controller
    {
        // GET: Diagram
        
        public ActionResult Index()
        {
            ViewBag.AppInstanceID = Request.Query["AppInstanceID"];
            ViewBag.ProcessGUID = Request.Query["ProcessGUID"];
            ViewBag.Version = Request.Query["Version"];
            return View();
        }
    }
}