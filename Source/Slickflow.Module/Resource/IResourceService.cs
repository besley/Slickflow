using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Module.Resource
{
    /// <summary>
    /// 资源服务接口
    /// </summary>
    public interface IResourceService
    {
        IList<Role> GetRoleAll();
        IList<User> GetUserListByRoles(string[] roleIDs);
        IList<Role> FillUsersIntoRoles(string[] roleIDs);
        IList<User> GetUserListByRole(string roleID);
        IList<User> GetUserListByRoleCode(string roleCode);
        IList<User> GetUserListByRole(string roleID, string curUserID, int receiverType);
    }
}
