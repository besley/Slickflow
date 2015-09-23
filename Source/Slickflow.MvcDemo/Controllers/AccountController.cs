using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ServiceStack.Text;
using Newtonsoft.Json;

namespace Slickflow.MvcDemo
{
    /// <summary>
    /// 用户账号控制器
    /// </summary>
    public class AccountController : Controller
    {
        //
        // GET: /Home/
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
    }
}
