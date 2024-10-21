using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using SlickOne.WebUtility;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;


namespace Slickflow.WebApi.Controllers
{
    public class WfCacheTestController : Controller
    {
        /// <summary>
        /// 重置流程缓存
        /// 测试示例：
        /// {"ProcessGUID":"5d6a7d6f-daa2-482d-8303-87b3b9f59a6a", "Version": "1"}
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult ResetCache(WfAppRunner runner)
        {
            IWorkflowService wfService = new WorkflowService();
            wfService.ResetCache(runner.ProcessGUID, runner.Version);

            return ResponseResult.Success();
        }
    }
}
