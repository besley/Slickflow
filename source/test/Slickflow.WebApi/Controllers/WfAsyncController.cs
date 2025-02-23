using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SlickOne.WebUtility;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Service;

namespace Slickflow.WebApi.Controllers
{
    //webapi: http://localhost/sfapi/api/wfsequence/
    //数据库表: WfProcess
    //普通顺序流程基本测试(顺序,返签,退回,撤销等测试)
    //流程记录ID：3
    //流程名称：报价流程

    //Database table: WfProcess
    //Basic testing of ordinary sequential processes (testing of sequence, return, cancellation, etc.)
    //Process record ID: 3
    //Process Name: Quotation Process
    //Process GUID: 072af8c3-482a-4b1c-890b-685ce2fcc75d
    //startup process:
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessID":"072af8c3-482a-4b1c-890b-685ce2fcc75d"}

    //run process app:
    //业务员提交办理节点：
    //下一步是“板房签字”办理节点
    //Salesperson submits processing nodes:
    //The next step is to handle the node of "signing the board house"
    //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessID":"072af8c3-482a-4b1c-890b-685ce2fcc75d","UserID":"10","UserName":"Long","NextActivityPerformers":{"eb833577-abb5-4239-875a-5f2e2fcb6d57":[{"UserID":10,"UserName":"Long"}]}}

    /// <summary>
    /// Async Controller
    /// </summary>
    public class WfAsyncController : Controller
    {
        [HttpPost]
        public ResponseResult StartProcessAsync01([FromBody] WfAppRunner runner)
        {
            var wfService = new WorkflowService();
            var task = wfService.StartProcessAsync(runner);
            string time = string.Format("webapi time:{0}", System.DateTime.Now.ToString());
            System.Diagnostics.Debug.WriteLine(time);

            task.Wait();

            if (task.Result.Status == WfExecutedStatus.Success)
                return ResponseResult.Success(task.Result.Status);
            else
                return ResponseResult.Error(task.Result.Message);
        }

        [HttpPost]
        public async Task<ResponseResult> StartProcessAsync02([FromBody] WfAppRunner runner)
        {
            var wfService = new WorkflowService();
            var task = wfService.StartProcessAsync(runner);
            string time = string.Format("webapi time:{0}", System.DateTime.Now.ToString());
            System.Diagnostics.Debug.WriteLine(time);

            var result = await task;

            if (result.Status == WfExecutedStatus.Success)
                return ResponseResult.Success(task.Status);
            else
                return ResponseResult.Error(result.Message);
        }

        [HttpPost]
        public ResponseResult StartProcessAsync03([FromBody] WfAppRunner runner)
        {
            var wfService = new WorkflowService();
            var task = wfService.StartProcessAsync(runner);
            string time = string.Format("webapi time:{0}", System.DateTime.Now.ToString());
            System.Diagnostics.Debug.WriteLine(time);

            if (task.Result.Status == WfExecutedStatus.Success)
                return ResponseResult.Success(task.Result.Status);
            else
                return ResponseResult.Error(task.Result.Message);
        }

    }
}