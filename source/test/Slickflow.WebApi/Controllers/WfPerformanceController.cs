using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SlickOne.WebUtility;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;

namespace Slickflow.WebApi.Controllers
{
    //webapi: http://localhost/sfapi/api/wfperformance/
    /// <summary>
    /// process test
    /// </summary>
    public class WfPerformanceController : Controller
    {
       [HttpGet]
       public ResponseResult GetTaskView()
        {
            var result = ResponseResult.Default();
            var wfService = new WorkflowService();
            var processInstanceID = 5492;
            var activityInstanceID = 46593;

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var task = wfService.GetTaskView(processInstanceID, activityInstanceID);
            stopWatch.Stop();
            var time = stopWatch.Elapsed;
            result = ResponseResult.Success(time.ToString());

            return result;
        }

        /// <summary>
        /// Get Ready Tasks
        /// {"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"SEQ-C-1099","ProcessID":"072af8c3-482a-4b1c-890b-685ce2fcc75d"}
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult GetReadyTasks([FromBody]TaskQuery query)
        {
            var result = ResponseResult.Default();
            var wfService = new WorkflowService();

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var task = wfService.GetReadyTasks(query);
            stopWatch.Stop();
            var time = stopWatch.Elapsed;
            result = ResponseResult.Success(time.ToString());

            return result;
        }
    }
}