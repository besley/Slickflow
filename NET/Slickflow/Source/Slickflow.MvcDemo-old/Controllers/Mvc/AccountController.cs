using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Slickflow.MvcDemo.Controllers.Mvc
{
    /// <summary>
    /// 用户登录控制器
    /// </summary>
    public class AccountController : Controller
    {
        // GET: Account
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
    }
}