using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Slickflow.MvcDemo.Controllers.Mvc
{
    /// <summary>
    /// 业务流程应用控制器
    /// </summary>
    public class KGraphController : BaseMvcController
    {
        /// <summary>
        /// 显示流程图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.AppInstanceID = Request.QueryString["AppInstanceID"];
            ViewBag.ProcessGUID = Request.QueryString["ProcessGUID"];

            return View();
        }
    }
}