using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Slickflow.Designer.Controllers.Mvc
{
    public class HomeController : BaseMvcController
    {
        // GET: Home
        [AllowAnonymous]
        public ActionResult Index()
        {
            //ViewBag.RoleName = "admin";
            return View();
        }

        public ActionResult Dialog()
        {
            return View();
        }
    }
}