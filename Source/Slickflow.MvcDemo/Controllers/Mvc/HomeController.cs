using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SlickOne.WebUtility;
using Slickflow.MvcDemo.Models;

namespace Slickflow.MvcDemo.Controllers.Mvc
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
    }
}