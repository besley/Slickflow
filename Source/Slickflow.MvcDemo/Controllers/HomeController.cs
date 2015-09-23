using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PlatOne.WebUtility;

namespace Slickflow.MvcDemo.Controllers
{
    /// <summary>
    /// 主页控制器
    /// </summary>
    public class HomeController : MvcControllerBase
    {
        //
        // GET: /Home/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}
