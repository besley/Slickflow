using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Slickflow.MvcDemo.Controllers
{
    [Authorize]
    public class SlickflowController : Controller
    {
        // GET: Slickflow
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MySlickflow()
        {
            return View();
        }
    }
}