using System.Collections.Generic;
using System.Linq;
using Slickflow.Data;

namespace Slickflow.Module.Resource
{
    /// <summary>
    /// Role User Manager
    /// 角色用户管理类
    /// </summary>
    internal class RoleUserManager : ManagerBase
    {
        /// <summary>
        /// Get users by role code
        /// 根据角色编码查询用户
        /// </summary>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        internal List<User> GetUserListByRoleCode(string roleCode)
        {
            var strSQL = @"SELECT 
                                U.ID as UserID,
                                U.UserName
                           FROM SysUser U
                           INNER JOIN SysRoleUser RU
                                ON U.ID = RU.UserID
                           INNER JOIN SysRole R
                                ON R.ID = RU.RoleID
                           WHERE R.RoleCode = @roleCode
                           ORDER BY U.ID";
            var list = Repository.Query<User>(strSQL, new { roleCode = roleCode }).ToList();
            return list;
        }

        /// <summary>
        /// Get Role User Tree
        /// 获取角色用户树
        /// </summary>
        /// <returns></returns>
        internal List<RoleUserView> GetRoleUserTree()
        {
            var strSQL = @"SELECT 
                                R.ID as RoleID,
                                R.RoleName as RoleName,
                                R.RoleCode as RoleCode,
                                U.ID as UserID,
                                U.UserName
                           FROM SysUser U
                           INNER JOIN SysRoleUser RU
                                ON U.ID = RU.UserID
                           INNER JOIN SysRole R
                                ON R.ID = RU.RoleID
                           ORDER BY R.ID,
                                U.ID";
            var list = Repository.Query<RoleUserView>(strSQL, null).ToList();
            return list;
        }

        /// <summary>
        /// Get user entity
        /// 获取用户
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        internal UserEntity GetUserEntity(string userID)
        {
            var userEntity = Repository.GetById<UserEntity>(userID);
            return userEntity;
        }

        /// <summary>
        /// Get roleuserview by role id
        /// 根据角色ID获取用户角色视图
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        internal List<RoleUserView> GetUserByRole(int roleID)
        {
            var sql = @"SELECT 
                            A.ID AS RoleID, 
                            A.RoleCode, 
                            A.RoleName,
                            C.ID AS UserID,
                            C.UserName
                        FROM SysRole A
                        INNER JOIN SysRoleUser B 
                            ON A.ID = B.RoleID
                        INNER JOIN SysUser C
                            ON B.UserID = C.ID
                        WHERE A.ID = @roleID
                        ORDER BY A.ID, C.ID                        
                        ";
            var list = Repository.Query<RoleUserView>(sql, new
            {
                roleID = roleID
            }).ToList<RoleUserView>();
            return list;
        }

        /// <summary>
        /// Retrieve character user list data based on character iD list
        /// 根据角色iD列表获取角色用户列表数据
        /// </summary>
        /// <param name="idsin"></param>
        /// <returns></returns>
        internal List<RoleUserView> GetUserByRoleIDs(string[] idsin)
        {
            var sql = @"SELECT 
                            A.ID AS RoleID, 
                            A.RoleCode, 
                            A.RoleName,
                            C.ID AS UserID,
                            C.UserName
                        FROM SysRole A
                        INNER JOIN SysRoleUser B 
                            ON A.ID = B.RoleID
                        INNER JOIN SysUser C
                            ON B.UserID = C.ID
                        WHERE A.ID IN @roleIDs
                        ORDER BY A.ID, C.ID                        
                        ";
            var list = Repository.Query<RoleUserView>(sql, new
            {
                roleIDs = idsin
            }).ToList<RoleUserView>();
            return list;
        }

        /// <summary>
        /// Retrieve user list based on role ids
        /// 根据角色获取用户列表
        /// </summary>
        /// <param name="roleIDs"></param>
        /// <returns></returns>
        internal List<User> GetUserListByRoles(string[] idsin)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetUserListByRoles(idsin, session);
            }
        }

        /// <summary>
        /// Retrieve user list based on role ids
        /// 根据角色获取用户列表
        /// </summary>
        /// <param name="roleIDs"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        internal List<User> GetUserListByRoles(string[] idsin, IDbSession session)
        {
            var sql = @"SELECT DISTINCT 
                            C.ID AS UserID,
                            C.UserName
                        FROM SysRole A
                        INNER JOIN SysRoleUser B 
                            ON A.ID = B.RoleID
                        INNER JOIN SysUser C
                            ON B.UserID = C.ID
                        WHERE A.ID IN @roleIDs
                        ORDER BY C.ID
                        ";

            var list = Repository.Query<User>(session.Connection,
                sql,
                new
                {
                    roleIDs = idsin
                },
                session.Transaction).ToList<User>();
            return list;
        }

        /// <summary>
        /// Retrieve user list based on role id
        /// 根据角色ID获取用户列表
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        internal List<User> GetUserListByRole(string roleID)
        {
            var sql = @"SELECT 
                            C.ID AS UserID,
                            C.UserName
                        FROM SysRole A
                        INNER JOIN SysRoleUser B 
                            ON A.ID = B.RoleID
                        INNER JOIN SysUser C
                            ON B.UserID = C.ID
                        WHERE A.ID = @roleID
                        ORDER BY A.ID, C.ID                        
                        ";
            var list = Repository.Query<User>(sql, new
            {
                roleID = roleID
            }).ToList<User>();
            return list;
        }


        /// <summary>
        /// Encapsulate the front-end role list based on the role user view
        /// 根据角色用户视图，封装前端角色列表
        /// </summary>
        /// <param name="roleUserView"></param>
        /// <returns></returns>
        internal IList<Role> GetRoleListByRoleUserView(List<RoleUserView> roleUserView)
        {
            int roleID = 0;
            Role role = null;
            User user = null;
            List<Role> newRoleList = new List<Role>();
            foreach (var item in roleUserView)
            {
                if (item.RoleID != roleID)
                {
                    role = new Role();
                    role.ID = item.RoleID.ToString();
                    role.RoleCode = item.RoleCode;
                    role.RoleName = item.RoleName;
                    role.UserList = new List<User>();
                    user = new User();
                    user.UserID = item.UserID.ToString();
                    user.UserName = item.UserName;
                    role.UserList.Add(user);

                    newRoleList.Add(role);
                    roleID = item.RoleID;
                }
                else
                {
                    user = new User();
                    user.UserID = item.UserID.ToString();
                    user.UserName = item.UserName;
                    role.UserList.Add(user);
                }
            }
            return newRoleList;
        }
    }
}
