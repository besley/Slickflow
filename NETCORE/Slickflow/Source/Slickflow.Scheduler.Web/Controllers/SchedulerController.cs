using System;
using Microsoft.AspNetCore.Mvc;
using Slickflow.Scheduler.Web.Utility;
using Slickflow.Scheduler.Common;
using Slickflow.Scheduler.Entity;
using Slickflow.Scheduler.Service;

namespace Slickflow.Scheduler.Web.Controllers
{
    /// <summary>
    /// 调度控制器
    /// </summary>
    public class SchedulerController : Controller
    {
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
    }
}
