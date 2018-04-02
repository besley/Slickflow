using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Slickflow.Designer.Controllers.Mvc
{
    /// <summary>
    /// 流程图查看页面
    /// </summary>
    public class DiagramController : Controller
    {
        // GET: Diagram
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.AppInstanceID = Request.QueryString["AppInstanceID"];
            ViewBag.ProcessGUID = Request.QueryString["ProcessGUID"];
            return View();
        }
    }
}