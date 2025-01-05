using System;
using System.Collections.Generic;
using Slickflow.Data;

namespace Slickflow.Module.Resource
{
    /// <summary>
    /// Resource Manager - Organizational Structure Service
    /// 资源管理器--组织架构服务
    /// </summary>
    public partial class ResourceService : IResourceService
    {
        /// <summary>
        /// Get All Roles
        /// 获取全部角色
        /// </summary>
        /// <returns></returns>
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
        /// Get All Users
        /// 获取所有用户数据
        /// </summary>
        /// <returns></returns>
        public IList<User> GetUserAll()
        {
            var um = new UserManager();
            var itemList = um.GetAll();
            IList<User> userList = new List<User>();    
            foreach (var item in itemList)
            {
                var user = new User
                {
                    UserID = item.ID.ToString(),
                    UserName = item.UserName,
                    EMail = item.EMail
                };
                userList.Add(user);     
            }
            return userList;
        }

        /// <summary>
        /// Get User by id
        /// 获取用户数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUserById(string id)
        {
            User user = null;
            var um = new UserManager();
            var userEntity = um.GetById(int.Parse(id));
            if (userEntity != null)
            {
                user = new User
                {
                    UserID = userEntity.ID.ToString(),
                    UserName = userEntity.UserName,
                    EMail = userEntity.EMail
                };
            }
            return user;
        }

        /// <summary>
        /// Retrieve user data list based on multiple role IDs
        /// 根据多个角色ID获取用户数据列表
        /// </summary>
        /// <param name="roleIDs"></param>
        /// <returns></returns>
        public IList<User> GetUserListByRoles(string[] roleIDs)
        {
            var rum = new RoleUserManager();
            return rum.GetUserListByRoles(roleIDs);
        }

        /// <summary>
        /// Retrieve user list based on roles
        /// 根据角色获取用户列表
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public IList<User> GetUserListByRole(string roleID)
        {
            var rum = new RoleUserManager();
            var userList = rum.GetUserListByRole(roleID);
            return userList;
        }

        /// <summary>
        /// Retrieve user list based on role code
        /// 根据角色代码获取用户列表
        /// </summary>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        public IList<User> GetUserListByRoleCode(string roleCode)
        {
            var rum = new RoleUserManager();
            var userList = rum.GetUserListByRoleCode(roleCode);
            return userList;
        }

        /// <summary>
        /// Retrieve data from the role user tree based on the recipient type
        /// 根据接收者类型，来获取角色用户树的数据
        /// </summary>
        /// <param name="roleIDs"></param>
        /// <param name="curUserID"></param>
        /// <param name="receiverType"></param>
        /// <returns></returns>
        public IList<User> GetUserListByRoleReceiverType(string[] roleIDs, string curUserID, int receiverType)
        {
            IList<User> userList = null;
            using (var session = SessionFactory.CreateSession())
            {
                if (receiverType == 0)
                {
                    //直接根据角色查询
                    //Directly query based on roles
                    var rum = new RoleUserManager();
                    userList = rum.GetUserListByRoles(roleIDs, session);
                }
                else
                {
                    //根据用户级别关系查询
                    // 上司:1, 同事:2, 下属:3
                    //Query based on user level relationships
                    //Superior: 1, Colleagues: 2, Subordinates: 3
                    IDeptService deptService = DeptServiceFactory.CreateDeptService();
                    userList = deptService.GetUserListByDeptRank(roleIDs, curUserID, receiverType);
                }
            }
            return userList;
        }

        /// <summary>
        /// Retrieve the user list under the role and bind it to the role attributes
        /// 获取角色下的用户列表，并绑定在角色属性上
        /// </summary>
        /// <param name="roleIDs"></param>
        /// <returns></returns>
        public IList<Role> FillUsersIntoRoles(string[] roleIDs)
        {
            var rum = new RoleUserManager();
            var ruvList = rum.GetUserByRoleIDs(roleIDs);

            var newRoleList = rum.GetRoleListByRoleUserView(ruvList);
            return newRoleList;
        }

        /// <summary>
        /// Obtain roles based on role codes
        /// 根据角色代码获取角色
        /// </summary>
        /// <param name="roleCode"></param>
        /// <returns></returns>
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
