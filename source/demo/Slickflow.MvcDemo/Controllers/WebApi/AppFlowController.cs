using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;
using SlickOne.WebUtility;
using Slickflow.BizAppService.Entity;
using Slickflow.BizAppService.Interface;
using Slickflow.BizAppService.Service;

namespace Slickflow.MvcDemo.Controllers.WebApi
{
    /// <summary>
    /// 流程业务步骤记录
    /// 示例代码，请勿直接作为生产项目代码使用。
    /// </summary>
    public class AppFlowController : Controller
    {
        #region 基本数据操作
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
        /// 数据分页显示
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
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
                    string.Format("获取{0}分页数据失败，错误{1}", "AppFlow", ex.Message)
                );
            }
            return result;
        }
    }
}
