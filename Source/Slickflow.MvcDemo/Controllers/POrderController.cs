using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Slickflow.MvcDemo.Controllers
{
    /// <summary>
    /// 生产订单流程控制器
    /// </summary>
    public class POrderController : Controller
    {
        //
        // GET: /POrder/
        public ActionResult Index()
        {
            return View();
        }
	}
}