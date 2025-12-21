using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Slickflow.BizAppService.Entity;
using Slickflow.BizAppService.Interface;
using Slickflow.BizAppService.Service;
using Slickflow.WebUtility;

namespace Slickflow.MvcDemo.Controllers.WebApi
{
    /// <summary>
    /// Process business step record
    /// Example code for developers' reference
    /// 流程业务步骤记录
    /// 示例代码，供开发人员参考
    /// </summary>
    public class AppFlowController : Controller
    {
        #region Interface
        private IAppFlowService _service;
        protected IAppFlowService AppFlowService
        {
            get
            {
                if (_service == null)
                {
                    _service = new AppFlowService();
                }
                return _service;
            }
        }
        #endregion

        /// <summary>
        /// Query Data Paged
        /// </summary>
        [HttpPost]
        public ResponseResult<List<AppFlowEntity>> QueryPaged([FromBody] AppFlowQuery query)
        {
            var result = ResponseResult<List<AppFlowEntity>>.Default();
            try
            {
                var count = 0;
                var list = AppFlowService.GetPaged(query, out count);
                result = ResponseResult<List<AppFlowEntity>>.Success(list);
                result.TotalRowsCount = count;
                result.TotalPages = (count + query.PageSize - 1) / query.PageSize;
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<AppFlowEntity>>.Error(
                    string.Format("Get Paged Data {0} Failed，Exception:{1}", "AppFlow", ex.Message)
                );
            }
            return result;
        }
    }
}
