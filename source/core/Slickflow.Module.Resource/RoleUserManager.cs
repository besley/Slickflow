using Dapper;
using Slickflow.Data;
using System;
using System.Collections.Generic;
using System.Linq;

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
                                U.id AS UserId,
                                U.user_name AS UserName
                           FROM sys_user U
                           INNER JOIN sys_role_user RU
                                ON U.id = RU.user_id
                           INNER JOIN sys_role R
                                ON R.id = RU.role_id
                           WHERE R.role_code = @roleCode
                           ORDER BY U.id";
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
                                R.id as RoleId,
                                R.role_name as RoleName,
                                R.role_code as RoleCode,
                                U.id as UserId,
                                U.user_name
                           FROM sys_user U
                           INNER JOIN sys_role_user RU
                                ON U.id = RU.user_id
                           INNER JOIN sys_role R
                                ON R.id = RU.role_id
                           ORDER BY R.id,
                                U.id";
            var list = Repository.Query<RoleUserView>(strSQL, null).ToList();
            return list;
        }

        /// <summary>
        /// Get user entity
        /// 获取用户
        /// </summary>
        internal UserEntity GetUserEntity(string userId)
        {
            var userEntity = Repository.GetById<UserEntity>(userId);
            return userEntity;
        }

        /// <summary>
        /// Get roleuserview by role id
        /// 根据角色ID获取用户角色视图
        /// </summary>
        internal List<RoleUserView> GetUserByRole(int roleId)
        {
            var sql = @"SELECT 
                            R.id AS RoleId, 
                            R.role_code AS RoleCode, 
                            R.role_name AS RoleName,
                            U.id AS UserId,
                            U.user_name AS UserName
                        FROM sys_role R
                        INNER JOIN sys_role_user RU 
                            ON R.id = RU.role_id
                        INNER JOIN sys_user U
                            ON RU.user_id = U.id
                        WHERE R.id = @roleId
                        ORDER BY R.id, U.id                       
                        ";
            var list = Repository.Query<RoleUserView>(sql, new
            {
                roleId = roleId
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
            //var sql = @"SELECT 
            //                R.id AS RoleId, 
            //                R.role_code AS RoleCode, 
            //                R.role_name AS RoleName,
            //                U.id AS UserId,
            //                U.user_name AS UserName
            //            FROM sys_role R
            //            INNER JOIN sys_role_user RU 
            //                ON R.id = RU.role_id
            //            INNER JOIN sys_user U
            //                ON RU.user_id = U.id
            //            WHERE R.id IN @roleIds
            //            ORDER BY R.id, U.id                       
            //            ";

            // 将字符串数组转换为整数数组
            int[] roleIds;
            try
            {
                roleIds = idsin.Select(id => int.Parse(id)).ToArray();
            }
            catch (FormatException)
            {
                // 如果包含非数字，返回空列表或抛出具体异常
                throw new ArgumentException("角色Id必须为有效的整数");
            }

            var sql = @"SELECT 
                            R.id AS RoleId, 
                            R.role_code AS RoleCode, 
                            R.role_name AS RoleName,
                            U.id AS UserId,
                            U.user_name AS UserName
                        FROM sys_role R
                        INNER JOIN sys_role_user RU 
                            ON R.id = RU.role_id
                        INNER JOIN sys_user U
                            ON RU.user_id = U.id
                        WHERE R.id = ANY(@roleIds)
                        ORDER BY R.id, U.id                       
                        ";
            var list = Repository.Query<RoleUserView>(sql, 
                new { roleIds } // 使用整数数组
            ).ToList<RoleUserView>();
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
            //var sql = @"SELECT DISTINCT 
            //                U.id AS UserId,
            //                U.user_name AS UserName
            //            FROM sys_role R
            //            INNER JOIN sys_role_user RU 
            //                ON R.id = RU.role_id
            //            INNER JOIN sys_user U
            //                ON RU.user_id = U.id
            //            WHERE R.id IN @roleIDs
            //            ORDER BY U.id
            //            ";

            //var list = Repository.Query<User>(session.Connection,
            //    sql,
            //    new
            //    {
            //        roleIds = idsin
            //    },
            //    session.Transaction).ToList<User>();
            //return list;
            if (idsin == null || !idsin.Any())
                return new List<User>();

            // 将字符串数组转换为整数数组
            int[] roleIds;
            try
            {
                roleIds = idsin.Select(id => int.Parse(id)).ToArray();
            }
            catch (FormatException)
            {
                // 如果包含非数字，返回空列表或抛出具体异常
                throw new ArgumentException("角色Id必须为有效的整数");
            }

            var sql = @"SELECT DISTINCT 
                    U.id AS UserId,
                    U.user_name AS UserName
                FROM sys_role R
                INNER JOIN sys_role_user RU 
                    ON R.id = RU.role_id
                INNER JOIN sys_user U
                    ON RU.user_id = U.id
                WHERE R.id = ANY(@roleIds)
                ORDER BY U.id";

            var list = Repository.Query<User>(session.Connection,
                sql,
                new { roleIds }, // 使用整数数组
                session.Transaction).ToList<User>();
            return list;
        }

        /// <summary>
        /// Retrieve user list based on role id
        /// 根据角色ID获取用户列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        internal List<User> GetUserListByRole(string roleId)
        {
            int intRoleId = int.Parse(roleId);
            var sql = @"SELECT 
                            U.id AS UserId,
                            U.user_name AS UserName
                        FROM sys_role R
                        INNER JOIN sys_role_user RU 
                            ON R.id = RU.role_id
                        INNER JOIN sys_user U
                            ON RU.user_id = U.Id
                        WHERE R.id = @roleId
                        ORDER BY R.id, U.id                        
                        ";
            var list = Repository.Query<User>(sql, new
            {
                roleId = intRoleId
            }).ToList<User>();
            return list;
        }


        /// <summary>
        /// Encapsulate the front-end role list based on the role user view
        /// 根据角色用户视图，封装前端角色列表
        /// </summary>
        internal IList<Role> GetRoleListByRoleUserView(List<RoleUserView> roleUserView)
        {
            int roleId = 0;
            Role role = null;
            User user = null;
            List<Role> newRoleList = new List<Role>();
            foreach (var item in roleUserView)
            {
                if (item.RoleId != roleId)
                {
                    role = new Role();
                    role.Id = item.RoleId.ToString();
                    role.RoleCode = item.RoleCode;
                    role.RoleName = item.RoleName;
                    role.UserList = new List<User>();
                    user = new User();
                    user.UserId = item.UserId.ToString();
                    user.UserName = item.UserName;
                    role.UserList.Add(user);

                    newRoleList.Add(role);
                    roleId = item.RoleId;
                }
                else
                {
                    user = new User();
                    user.UserId = item.UserId.ToString();
                    user.UserName = item.UserName;
                    role.UserList.Add(user);
                }
            }
            return newRoleList;
        }
    }
}
