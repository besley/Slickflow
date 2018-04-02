using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace Slickflow.Designer.Controllers.Mvc
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    public class TestController : Controller
    {
        // GET: Test
        public IActionResult Index()
        {
            return View();
        }
    }
}