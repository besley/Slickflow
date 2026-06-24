using Microsoft.AspNetCore.Mvc;
using Slickflow.Engine.Business.Entity;
using Slickflow.WebUtility;

namespace sfdapi.Controllers
{
    public class WfTrialController : Controller
    {
        [HttpPost]
        public ResponseResult<ProcessEntity> ExecuteProcessGraph([FromBody] object graph)
        {
            return ResponseResult<ProcessEntity>.Error(
                "Code Studio is an enterprise feature and is not available in the community edition.");
        }
    }
}
