using System;
using System.Collections.Generic;
using Slickflow.Data;

namespace Slickflow.Module.Resource
{
    /// <summary>
    /// 资源管理器--组织架构服务
    /// </summary>
    public partial class ResourceService : IResourceService
    {
        /// <summary>
        /// 获取全部角色
        /// </summary>
        /// <returns>角色列表</returns>
        public IList<Role> GetRoleAll()
        {
            var rm = new RoleManager();
            var itemList = rm.GetAll();
            IList<Role> roleList = new List<Role>();
            foreach (var item in itemList)
            {
                var role = new Role
                {
                    ID = item.ID.ToString(),
                    RoleCode = item.RoleCode,
                    RoleName = item.RoleName
                };
                roleList.Add(role);
            }
            return roleList;
        }

        /// <summary>
        /// 根据多个角色ID获取用户数据列表
        /// </summary>
        /// <param name="roleIDs">多个角色ID</param>
        /// <returns>用户列表</returns>
        public IList<User> GetUserListByRoles(string[] roleIDs)
        {
            var rum = new RoleUserManager();
            return rum.GetUserListByRoles(roleIDs);
        }

        /// <summary>
        /// 根据角色获取用户列表
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns>用户列表</returns>
        public IList<User> GetUserListByRole(string roleID)
        {
            var rum = new RoleUserManager();
            var userList = rum.GetUserListByRole(roleID);
            return userList;
        }

        /// <summary>
        /// 根据角色代码获取用户列表
        /// </summary>
        /// <param name="roleCode">角色代码</param>
        /// <returns>用户列表</returns>
        public IList<User> GetUserListByRoleCode(string roleCode)
        {
            var rum = new RoleUserManager();
            var userList = rum.GetUserListByRoleCode(roleCode);
            return userList;
        }

        /// <summary>
        /// 根据接收者类型，来获取角色用户树的数据
        /// </summary>
        /// <param name="roleIDs">角色ID</param>
        /// <param name="curUserID">当前用户ID</param>
        /// <param name="receiverType">接收者类型</param>
        /// <returns>用户列表</returns>
        public IList<User> GetUserListByRoleReceiverType(string[] roleIDs, string curUserID, int receiverType)
        {
            IList<User> userList = null;
            using (var session = SessionFactory.CreateSession())
            {
                if (receiverType == 0)
                {
                    //直接根据角色查询
                    var rum = new RoleUserManager();
                    userList = rum.GetUserListByRoles(roleIDs, session);
                }
                else
                {
                    //根据用户级别关系查询
                    // 上司:1, 同事:2, 下属:3
                    IDeptService deptService = DeptServiceFactory.CreateDeptService();
                    userList = deptService.GetUserListByDeptRank(roleIDs, curUserID, receiverType);
                }
            }
            return userList;
        }

        /// <summary>
        /// 获取角色下的用户列表，并绑定在角色属性上
        /// </summary>
        /// <param name="roleIDs">多个角色ID</param>
        /// <returns>角色列表</returns>
        public IList<Role> FillUsersIntoRoles(string[] roleIDs)
        {
            var rum = new RoleUserManager();
            var ruvList = rum.GetUserByRoleIDs(roleIDs);

            var newRoleList = rum.GetRoleListByRoleUserView(ruvList);
            return newRoleList;
        }

        /// <summary>
        /// 根据角色代码获取角色
        /// </summary>
        /// <param name="roleCode">角色代码</param>
        /// <returns>角色实体</returns>
        public Role GetRoleByCode(string roleCode)
        {
            var rm = new RoleManager();
            var roleEntity = rm.GetByCode(roleCode);
            var role = new Role
            {
                ID = roleEntity.ID.ToString(),
                RoleName = roleEntity.RoleName,
                RoleCode = roleEntity.RoleCode
            };
            return role;
        }
    }
}
