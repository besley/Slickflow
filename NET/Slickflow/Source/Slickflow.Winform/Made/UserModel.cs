using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Module.Resource;

namespace Slickflow.Winform
{
    public class UserModel
    {
        /// <summary>
        /// 根据角色编码获取用户列表
        /// </summary>
        /// <param name="entity"></param>
        public List<User> GetUsersByRoleCode(string roleCode)
        {
            IResourceService resService = new ResourceService();
            return resService.GetUserListByRoleCode(roleCode).ToList();
        }
    }
}
