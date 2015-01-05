using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Slickflow.MvcDemo.Models;
using System.Threading.Tasks;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Service;
using Slickflow.MvcDemo.Data;
using Slickflow.MvcDemo.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;

namespace Slickflow.MvcDemo.Controllers
{




    [Authorize]
    /// <summary>
    /// Leave 在这里姑且当请假用
    /// </summary>
    public class LeaveController : Controller
    {
        #region  用户角色权限
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }
        public LeaveController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        public LeaveController() { }
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
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        #endregion

        // GET: Leave
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增申请单
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            //默认选中事假，为方便起见应该都读取xml文件
            items.Add(new SelectListItem { Text = "病假", Value = "1", Selected = true });
            items.Add(new SelectListItem { Text = "事假", Value = "2" });
            items.Add(new SelectListItem { Text = "丧假", Value = "3" });
            items.Add(new SelectListItem { Text = "产假", Value = "4" });
            items.Add(new SelectListItem { Text = "工伤假", Value = "5" });
            items.Add(new SelectListItem { Text = "婚假", Value = "6" });
            items.Add(new SelectListItem { Text = "年休假", Value = "7" });
            items.Add(new SelectListItem { Text = "其他假", Value = "8" });
            ViewBag.LeaveType = new SelectList(items, "Value", "Text");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(LeaveViewModel leave)
        {
            string processGUID = leave.ProcessGUID;
            //验证不通过，重新填写表单
            if (!ModelState.IsValid)
            {
                //有待优化
                List<SelectListItem> items = new List<SelectListItem>();
                //默认选中事假，为方便起见应该都读取xml文件
                items.Add(new SelectListItem { Text = "病假", Value = "1", Selected = true });
                items.Add(new SelectListItem { Text = "事假", Value = "2" });
                items.Add(new SelectListItem { Text = "丧假", Value = "3" });
                items.Add(new SelectListItem { Text = "产假", Value = "4" });
                items.Add(new SelectListItem { Text = "工伤假", Value = "5" });
                items.Add(new SelectListItem { Text = "婚假", Value = "6" });
                items.Add(new SelectListItem { Text = "年休假", Value = "7" });
                items.Add(new SelectListItem { Text = "其他假", Value = "8" });
                ViewBag.LeaveType = new SelectList(items, "Value", "Text");
                return View(items);
            }
            IWorkflowService service = new WorkflowService();
            //流程开始第一步
            ActivityEntity firstActivity = service.GetFirstActivity(leave.ProcessGUID);
            //该处较上一版本有变化，上一版本为GUID类型
            string firstActivityGUID = firstActivity.ActivityGUID;
            IList<NodeView> nextActivity = service.GetNextActivity(leave.ProcessGUID, firstActivityGUID, GetCondition("days-" + leave.Days));
            //表示有下一位审批者
            if (nextActivity.Count() > 0)
            {
                //下一步角色ID审批者
                string outerId = nextActivity[0].Roles[0].ID.ToString();
                //这里只取第一个审批者，WebDemo 是弹窗形式选择
                IEnumerable<int> userId = RoleManager.FindById(Convert.ToInt32(outerId)).Users.Select(t => t.UserId);
                ApplicationUser user = await UserManager.FindByIdAsync(Convert.ToInt32(userId.ToList()[0]));
                //提交请假信息
                LeaveEntity leaveE = new LeaveEntity()
                {
                    FromDate = leave.BeginTime,
                    ToDate = leave.EndTime,
                    Days = leave.Days,
                    LeaveType = leave.LeaveType,
                    CurrentActivityText = "",
                    Status = 0,
                    CreatedUserID = Convert.ToInt32(User.Identity.GetUserId()),
                    CreatedUserName = User.Identity.Name,
                    CreatedDate = DateTime.Now
                };
                HrsLeaveResult result = new WorkFlowManager().Insert(leaveE);
                if (result.Successed)
                {
                    WfAppRunner initiator = new WfAppRunner();
                    initiator.AppName = "请假流程";
                    initiator.AppInstanceID = result.ResultIdentities.ToString();
                    initiator.ProcessGUID = processGUID;
                    initiator.UserID = outerId.ToString();
                    initiator.UserName = user.RealName;
                    initiator.Conditions = GetCondition(string.Format("days-{0}", leave.Days));
                    WfExecutedResult startedResult = service.StartProcess(initiator);


                    if (startedResult.Status != WfExecutedStatus.Success)
                    {
                        this.Content("<script>alert('" + startedResult.Message + "');</script>");
                    }

                    //送往下一步
                    PerformerList pList = new PerformerList();
                    pList.Add(new Performer(outerId.ToString(), user.RealName));

                    initiator.NextActivityPerformers = new Dictionary<String, PerformerList>();

                    initiator.NextActivityPerformers.Add(nextActivity[0].ActivityGUID, pList);

                    WfExecutedResult runAppResult = service.RunProcessApp(initiator);
                    if (runAppResult.Status != WfExecutedStatus.Success)
                    {
                        this.Content("<script>alert('" + runAppResult.Message + "');</script>");
                    }


                    ////保存业务数据
                    //BizAppFlowEntity AppFlowEntity = new Entity.BizAppFlowEntity();
                    //AppFlowEntity.AppName = "流程发起";
                    //AppFlowEntity.AppInstanceID = instanceId.ToString();
                    //AppFlowEntity.ActivityName = "流程发起";
                    //AppFlowEntity.Remark = string.Format("申请人:{0}-{1}", LoginUserID, LoginUserName);
                    //AppFlowEntity.ChangedTime = now;
                    //AppFlowEntity.ChangedUserID = LoginUserID;
                    //AppFlowEntity.ChangedUserName = LoginUserName;

                    //WorkFlows.AddBizAppFlow(AppFlowEntity);

                    //base.RegisterStartupScript("", "<script>alert('流程发起成功');window.location.href='FlowList.aspx';</script>");



                }


            }
            else
            {
                //显示前台错误
            }
            string setp = Request.Form["step"];
            //该代码会导致错误
            return RedirectToAction("Add");
        }



        /// <summary>
        /// 构造流转条件
        /// </summary>
        /// <param name="conditions">条件字符串</param>
        /// <returns></returns>
        protected Dictionary<string, string> GetCondition(string conditions)
        {
            //根据条件取得下一步骤
            Dictionary<string, string> condition = new Dictionary<string, string>();
            IList<string> role = UserManager.GetRoles(Convert.ToInt32(User.Identity.GetUserId()));
            for (int i = 0; i < role.Count; i++)
            {
                condition.Add("RoleID", RoleManager.FindByName(role[i].ToString()).Id.ToString());//角色ID
            }

            if (!string.IsNullOrEmpty(conditions))
            {
                string[] conditionItemArray = conditions.Split(',');
                foreach (string conditionItems in conditionItemArray)
                {
                    if (!string.IsNullOrEmpty(conditionItems))
                    {
                        string[] conditionItem = conditionItems.Split('-');
                        if (conditionItem.Length == 2)
                        {
                            string key = conditionItem[0];
                            string value = conditionItem[1];

                            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                            {
                                condition.Add(key, value);
                            }
                        }
                    }
                }
            }
            return condition;
        }
        /// <summary>
        /// override Content method
        /// </summary>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <param name="contentEncoding"></param>
        /// <returns></returns>
        protected override ContentResult Content(string content, string contentType, System.Text.Encoding contentEncoding)
        {
            return base.Content(content, contentType, contentEncoding);
        }
    }
}