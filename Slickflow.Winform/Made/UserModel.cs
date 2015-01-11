using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Winform
{
    public class UserModel
    {
        /// <summary>
        /// 根据角色编码获取用户列表
        /// </summary>
        /// <param name="entity"></param>
        public List<UserEntity> GetUsersByRoleCode(string roldCode)
        {
            var rm = new RoleManager();
            return rm.GetByRoleCode(roldCode);
        }
    }
}
