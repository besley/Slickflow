using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PlatOne.WebUtility;
using PlatOne.Auth.Business.Entity;
using PlatOne.Auth.Service;

namespace SfDemo.WebApi.Controllers
{
    /// <summary>
    /// 用户权限控制器
    /// 示例代码，请勿直接作为生产项目代码使用。
    /// </summary>
    public class AuthController : ApiControllerBase
    {
        /// <summary>
        /// 获取角色下的用户数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<ResourceEntity>> GetResourceByUser(int id)
        {
            var result = ResponseResult<List<ResourceEntity>>.Default();
            try
            {
                var authService = new AuthorizationService();
                var resourceList = authService.GetResourceByUser(id).ToList();

                result = ResponseResult<List<ResourceEntity>>.Success(resourceList, "成功获取用户资源数据！");
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<ResourceEntity>>.Error(string.Format(
                    "获取用户资源数据失败, 异常信息:{0}",
                    ex.Message));
            }
            return result;
        }
        
    }
}
