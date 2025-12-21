using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Module.Resource
{
    /// <summary>
    /// Resource Service Interface
    /// 资源服务接口
    /// </summary>
    public interface IResourceService
    {
        IList<Role> GetRoleAll();
        IList<User> GetUserListByRoles(string[] roleIds);
        IList<Role> FillUsersIntoRoles(string[] roleIds);
        IList<User> GetUserAll();
        User GetUserById(string id);
        IList<User> GetUserListByRole(string roleId);
        IList<User> GetUserListByRoleCode(string roleCode);
        IList<User> GetUserListByRoleReceiverType(string[] roleIds, string curUserId, int receiverType);
        Role GetRoleByCode(string roleCode);
    }
}
