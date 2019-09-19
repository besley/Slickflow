using Microsoft.AspNetCore.Mvc;
using Slickflow.Engine.Service;
using Slickflow.Scheduler.Web.Utility;
using Slickflow.Scheduler.Service;

namespace Slickflow.Scheduler.Web.Controllers.WebApi
{
    /// <summary>
    /// 调度控制器
    /// </summary>
    public class SchedulerController : Controller
    {
        [HttpGet]
        public string Hello()
        {
            return "Hello World!";
        }

        /// <summary>
        /// 流程逾期结束
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult TerminateOverdueProcessInstance()
        {
            var result = ResponseResult.Default();
            try
            {
                var scheService = new SchedulerService();
                scheService.TerminateOverdueProcessInstance();
                result = ResponseResult.Success(); 
            }
            catch(System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 定时启动流程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult TriggerTimingStartupProcess()
        {
            var result = ResponseResult.Default();
            try
            {
                var scheService = new SchedulerService();
                var processList = scheService.GetStartupTimingProcessList();

                var message = scheService.TriggerTimingStartupProcess(processList[0]);
                result = ResponseResult.Success(message);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }

        [HttpPost]
        public ResponseResult SendTaskEMail()
        {
            var result = ResponseResult.Default();
            try
            {
                var wfService = new WorkflowService();
                var processList = wfService.GetProcessListSimple();
                var msgService = new EMailService();
                var userList = msgService.GetUserList();
                msgService.SendTaskEMail(processList, userList);

                result = ResponseResult.Success("ok");
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }
    }
}
