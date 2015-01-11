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
using Slickflow.Engine.Business.Entity;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

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
                return View();
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
                //审批用户id
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
                    initiator.UserID = User.Identity.GetUserId();
                    initiator.UserName = User.Identity.Name;
                    initiator.Conditions = GetCondition(string.Format("days-{0}", leave.Days));
                    WfExecutedResult startedResult = service.StartProcess(initiator);
                    if (startedResult.Status != WfExecutedStatus.Success)
                    {
                        //给出提示
                    }
                    //送往下一步
                    PerformerList pList = new PerformerList();
                    //这里使用真实姓名代替
                    pList.Add(new Performer(user.Id.ToString(), user.RealName));
                    initiator.NextActivityPerformers = new Dictionary<String, PerformerList>();
                    initiator.NextActivityPerformers.Add(nextActivity[0].ActivityGUID, pList);
                    WfExecutedResult runAppResult = service.RunProcessApp(initiator);
                    if (runAppResult.Status != WfExecutedStatus.Success)
                    {
                        this.Content("<script>alert('" + runAppResult.Message + "');</script>");
                    }
                    //保存业务数据
                    BizAppFlowEntity AppFlowEntity = new BizAppFlowEntity();
                    AppFlowEntity.AppName = "流程发起";
                    AppFlowEntity.AppInstanceID = result.ResultIdentities.ToString();
                    AppFlowEntity.ActivityName = "流程发起";
                    AppFlowEntity.Remark = string.Format("申请人:{0}-{1}", User.Identity.GetUserId(), User.Identity.Name);
                    AppFlowEntity.ChangedTime = DateTime.Now;
                    AppFlowEntity.ChangedUserID = User.Identity.GetUserId();
                    AppFlowEntity.ChangedUserName = User.Identity.Name;
                    HrsLeaveResult resultBiz = new WorkFlowManager().Insert(AppFlowEntity);
                    if (resultBiz.Successed)
                    {
                        //给出前台提示
                        this.Content("", "<script>alert('流程发起成功');</script>");
                    }
                }
            }
            else
            {
                //显示前台错误，人事人员审批失败
                ModelState.AddModelError("Human", "该用户暂时不可提交审批，未查询到该用户的下一位审批者");
                return View();
            }
            return RedirectToAction("MySlickflow", "Slickflow");
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
        [HttpGet]
        public ActionResult List(int id = 1, string processGuid = "", int activityInstanceID = 0)
        {
            if (id == 1)
            {
                ViewBag.Message = "非法链接";
                return View();
            }
            else
            {
                if (string.IsNullOrEmpty(processGuid))
                {
                    WorkFlowManager work = new WorkFlowManager();
                    LeaveEntity entity = work.FindById(id);
                    string s = @"<table class='table table-bordered'>"
                                + "<tr><td>部门经理</td><td>" + entity.DepManagerRemark + "</td></tr>"
                                + "<tr><td>主管总监 </td><td>" + entity.DirectorRemark + "</td></tr>"
                                + "<tr><td>副总经理</td><td>" + entity.DeputyGeneralRemark + "</td></tr>"
                                + "<tr><td>总经理</td><td>" + entity.GeneralManagerRemark + "</td> </tr></table>";
                    ViewBag.Shenpi = s;
                    return View(entity);
                }
                else
                {
                    WorkFlowManager work = new WorkFlowManager();
                    LeaveEntity entity = work.FindById(id);
                    //权限设置
                    IWorkflowService service = new WorkflowService();
                    ProcessEntity processEntity = service.GetProcessById(processGuid);
                    if (processEntity != null)
                    {
                        ActivityInstanceEntity activityInstanceEntity = service.GetActivityInstance(activityInstanceID);
                        string s = "";
                        if (activityInstanceEntity != null)
                        {
                            ActivityEntity activityEntity = service.GetActivityEntity(processGuid, activityInstanceEntity.ActivityGUID);
                            if (activityEntity != null && activityEntity.Roles != null && activityEntity.Roles.Count > 0)
                            {
                                //用户角色列表
                                IList<string> roleUser = UserManager.GetRoles(Convert.ToInt32(User.Identity.GetUserId()));

                                foreach (var role in activityEntity.Roles)
                                {
                                    //为方便起见这里只取第一个角色，开发人员可以自行调整
                                    if (role.RoleCode == roleUser[0].ToString())
                                    {
                                        switch (role.ID)
                                        {
                                            case 2:
                                                //部门经理审批
                                                s = @"<table class='table table-bordered'>" +
                                               "<tr><td>部门经理</td><td><input type='text'class='form-control' name='DepManagerRemark'></td></tr>" +
                                               "<tr><td>主管总监 </td><input type='text' class='form-control' name='DirectorRemark' style='display:none'></td></tr>" +
                                               "<tr><td>副总经理</td><td><input type='text' class='form-control' name='DeputyGeneralRemark' style='display:none'></td></tr>" +
                                               "<tr><td>总经理</td><td><input class='form-control' name='GeneralManagerRemark'  style='display:none' type='text' /> </td> </tr>"
                                                ;
                                                break;
                                            case 3:
                                                //主管总监审批
                                                s = @"<table class='table table-bordered'>" +
                                               "<tr><td>部门经理</td><td>" + entity.DepManagerRemark + "<input type='text'class='form-control'  style='display:none' name='DepManagerRemark' value='" + entity.DepManagerRemark + "'></td></tr>" +
                                               "<tr><td>主管总监 </td><input type='text' class='form-control' name='DirectorRemark' ></td></tr>" +
                                               "<tr><td>副总经理</td><td><input type='text' class='form-control' name='DeputyGeneralRemark' style='display:none'></td></tr>" +
                                               "<tr><td>总经理</td><td><input class='form-control' name='GeneralManagerRemark'  style='display:none' type='text' /> </td> </tr>"
                                                                                   ;
                                                break;
                                            case 7:
                                                //副总经理审批
                                                s = @"<table class='table table-bordered'>" +
                                               "<tr><td>部门经理</td><td>" + entity.DepManagerRemark + "<input type='text'class='form-control' style='display:none' name='DepManagerRemark' value='" + entity.DepManagerRemark + "'></td></tr>" +
                                               "<tr><td>主管总监 </td><td>" + entity.DirectorRemark + "<input type='text' class='form-control' name='DirectorRemark' value='" + entity.DirectorRemark + "' ></td></tr>" +
                                               "<tr><td>副总经理</td><td><input type='text' class='form-control' name='DeputyGeneralRemark' style='display:none'></td></tr>" +
                                               "<tr><td>总经理</td><td><input class='form-control' name='GeneralManagerRemark'  style='display:none' type='text' /> </td> </tr>";
                                                break;
                                            case 8:
                                                //总经理审批
                                                s = @"<table class='table table-bordered'>" +
  "<tr><td>部门经理</td><td>" + entity.DepManagerRemark + "<input type='text'class='form-control' style='display:none' name='DepManagerRemark' value='" + entity.DepManagerRemark + "'></td></tr>" +
  "<tr><td>主管总监 </td><td>" + entity.DirectorRemark + "<input type='text' class='form-control' name='DirectorRemark' value='" + entity.DirectorRemark + "' ></td></tr>" +
  "<tr><td>副总经理</td><td><input type='text' class='form-control' name='DeputyGeneralRemark' style='display:none' value='" + entity.DeputyGeneralRemark + "'></td></tr>" +
  "<tr><td>总经理</td><td><input class='form-control' name='GeneralManagerRemark' type='text' /> </td> </tr>"
                                      ;
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        //获取当前角色      
                        ViewBag.Shenpi = "<form action='' id='Remark'>" + s + "</table></form><button class='btn btn-primary' id='agree'>同意</button><button class='btn btn-primary' id='return'>退回</button>";
                        ViewBag.processGUID = processGuid;
                        ViewBag.days = entity.Days;
                        return View(entity);
                    }
                }
            }
            //表示出现错误
            return View();
        }
      
        [HttpPost]
        public async Task<ActionResult> Approval(string type = "agree", string processGUID = "", int instanceId = 0, double days = 0)
        {
            var resolveRequest = HttpContext.Request;
            resolveRequest.InputStream.Seek(0, System.IO.SeekOrigin.Begin);
            string jsonString = new System.IO.StreamReader(resolveRequest.InputStream).ReadToEnd();
            try
            {
                IWorkflowService service = new WorkflowService();
                WfAppRunner runner = new WfAppRunner();
                runner.AppInstanceID = instanceId.ToString();
                runner.ProcessGUID = processGUID;
                runner.UserID = User.Identity.GetUserId();
                IList<NodeView> NodeViewList = service.GetNextActivityTree(runner, GetCondition("days-" + days));
                var leave = JsonConvert.DeserializeObject<LeaveEntity>(jsonString);
                leave.ID = instanceId;
                //调用流程
                WfAppRunner initiator = new WfAppRunner();
                initiator.AppName = "请假流程";
                initiator.AppInstanceID = instanceId.ToString();
                initiator.ProcessGUID = processGUID;
                initiator.UserID = User.Identity.GetUserId();
                initiator.UserName = User.Identity.GetUserName();
                initiator.Conditions = GetCondition(string.Format("days-{0}", days)); //后续节点不用传入条件表达式

                //获取下一步审批人信息
                //下一步角色ID审批者
                PerformerList pList = new PerformerList();
                if (NodeViewList[0].Roles.Count > 0)
                {
                    string outerId = NodeViewList[0].Roles[0].ID.ToString();
                    //这里只取第一个审批者，WebDemo 是弹窗形式选择
                    //审批用户id
                    IEnumerable<int> userId = RoleManager.FindById(Convert.ToInt32(outerId)).Users.Select(t => t.UserId);
                    ApplicationUser user = await UserManager.FindByIdAsync(Convert.ToInt32(userId.ToList()[0]));
                    //送往下一步
                    
                    pList.Add(new Performer(user.Id.ToString(), user.RealName));
                }

                initiator.NextActivityPerformers = new Dictionary<String, PerformerList>();
                initiator.NextActivityPerformers.Add(NodeViewList[0].ActivityGUID, pList);
                WfExecutedResult runAppResult = service.RunProcessApp(initiator);
                if (runAppResult.Status != WfExecutedStatus.Success)
                {
                }
                ProcessEntity processEntity = service.GetProcessById(processGUID);
                if (processEntity != null)
                {
                    ActivityInstanceEntity activityInstanceEntity = service.GetActivityInstance(instanceId);
                    if (activityInstanceEntity != null)
                    {
                        //CurrentActivityText = activityInstanceEntity.ActivityName;
                    }
                }
                new WorkFlowManager().UpdateHrsLeave(leave);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            return Json(new
            {
                success = true,
                message = "执行成功"
            });
        }
    }
}