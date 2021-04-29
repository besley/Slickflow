using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace Slickflow.Designer.Controllers.Mvc
{
    /// <summary>
    /// 流程模型查看页面
    /// </summary>
    public class ExampleController : Controller
    {
        // GET: Diagram
        
        public ActionResult Index()
        {
            return View();
        }
    }
}