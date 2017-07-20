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
            ViewBag.AppInstanceID = Request.QueryString["AppInstanceID"];
            ViewBag.ProcessGUID = Request.QueryString["ProcessGUID"];

            return View();
        }

        public ActionResult Dialog()
        {
            return View();
        }

        public ActionResult Demo()
        {
            return View();
        }
    }
}