using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Service;
using Slickflow.WebUtility;

namespace Slickflow.WebApi.Controllers
{
    /// <summary>
    /// Activity Test Controller
    /// </summary>
    public class WfActivityTestController : Controller
    {
        /// <summary>
        ///  Get First Activity
        /// </summary>
        [HttpPost]
        public ResponseResult<IList<NodeView>> GetFirstActivity([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<IList<NodeView>>.Default();

            var wfService = new WorkflowService();
            var step = wfService.GetFirstActivityRoleUserTree(runner);
            result = ResponseResult<IList<NodeView>>.Success(step);
            return result;
        }
    }
}