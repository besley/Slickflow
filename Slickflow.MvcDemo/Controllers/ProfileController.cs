using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Slickflow.MvcDemo.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Text;
using System.Collections;
using System.IO;
using System.Security.AccessControl;
using System.Globalization;
namespace Slickflow.MvcDemo.Controllers
{

    [Authorize()]
    public class ProfileController : Controller
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Profile
        public ActionResult Index()
        {
            ApplicationUser au = new ApplicationUser();
            au = UserManager.FindById(Convert.ToInt32(User.Identity.GetUserId()));
            return View(au);
        }
        /// <summary>
        /// 单一文件上传
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadFile()
        {
            string filePath = "";
            try
            {
                StringBuilder info = new StringBuilder();
                Hashtable extTable = new Hashtable();
                //支持的上传格式
                extTable.Add("image", "gif,jpg,jpeg,png,bmp");
                HttpPostedFileBase hpf = Request.Files["file"];
                //代表没有文件传过来
                if (hpf.ContentLength == 0)
                    return Content("没有上传的图片");
                string fileName = hpf.FileName;
                string fileExtensionName = Path.GetFileName(fileName);
                //if (string.IsNullOrEmpty(fileExtensionName) || Array.IndexOf(((String)extTable[fileExtensionName]).Split(','), fileExtensionName.Substring(1).ToLower()) == -1)
                //{
                //    return Content("非图片格式，请选择图片，然后开始上传");
                //}
                string savePath = HttpContext.Server.MapPath("../File/images/");
                string newFilePath = savePath;
                string ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
                savePath += ymd + "/";
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                string newFileName = DateTime.Now.ToString("yyyyMMdd_ffff", DateTimeFormatInfo.InvariantInfo) + fileExtensionName;
                filePath = savePath + newFileName;
                hpf.SaveAs(filePath);
                //获取上传文件扩展名，如果是非图片，给出提示
                //文件最大容量单位kb
                HttpContext.Response.AddHeader("Content-type", "text/html;charest=UFT-8");
                HttpContext.Response.End();
            }
            catch (Exception ex) { }
            return Json(filePath);
        }
        /// <summary>
        /// 多文件上传
        /// </summary>
        /// <returns></returns>

        public ActionResult UploadFileMultiply()
        {
            return View();
        }
    }
}