using System.Collections.Generic;
using System.Linq;
using Slickflow.Data;

namespace Slickflow.Module.Resource
{
    /// <summary>
    /// 角色用户管理类
    /// </summary>
    internal class RoleUserManager : ManagerBase
    {
        /// <summary>
        /// 根据角色编码查询用户
        /// </summary>
        /// <param name="roleCode">角色代码</param>
        /// <returns>用户列表</returns>
        internal List<User> GetUserListByRoleCode(string roleCode)
        {
            //var strSQL = @"SELECT 
            //                    U.ID as UserID,
            //                    U.UserName
            //               FROM SysUser U
            //               INNER JOIN SysRoleUser RU
            //                    ON U.ID = RU.UserID
            //               INNER JOIN SysRole R
            //                    ON R.ID = RU.RoleID
            //               WHERE R.RoleCode = @roleCode
            //               ORDER BY U.ID";
            //var list = Repository.Query<User>(strSQL, new { roleCode = roleCode }).ToList();
            var sqlQuery = (from a in Repository.GetAll<RoleEntity>()
                            join b in Repository.GetAll<RoleUserEntity>() 
                                on a.ID equals b.RoleID
                            join c in Repository.GetAll<UserEntity>() 
                                on b.UserID equals c.ID
                            where a.RoleCode == roleCode
                            orderby a.ID, c.ID
                            select new User
                            {
                                UserID = c.ID.ToString(),
                                UserName = c.UserName
                            });
            var list = sqlQuery.ToList<User>();
            return list;
        }

        /// <summary>
        /// 获取角色用户树
        /// </summary>
        /// <returns></returns>
        internal List<RoleUserView> GetRoleUserTree()
        {
            //var strSQL = @"SELECT 
            //                    R.ID as RoleID,
            //                    R.RoleName as RoleName,
            //                    R.RoleCode as RoleCode,
            //                    U.ID as UserID,
            //                    U.UserName
            //               FROM SysUser U
            //               INNER JOIN SysRoleUser RU
            //                    ON U.ID = RU.UserID
            //               INNER JOIN SysRole R
            //                    ON R.ID = RU.RoleID
            //               ORDER BY R.ID,
            //                    U.ID";
            //List<RoleUserView> list = Repository.Query<RoleUserView>(strSQL, null).ToList();
            var sqlQuery = (from a in Repository.GetAll<RoleEntity>()
                            join b in Repository.GetAll<RoleUserEntity>() 
                                on a.ID equals b.RoleID
                            join c in Repository.GetAll<UserEntity>() 
                                on b.UserID equals c.ID
                            orderby a.ID, c.ID
                            select new RoleUserView
                            {
                                RoleID = a.ID,
                                RoleCode = a.RoleCode,
                                RoleName = a.RoleName,
                                UserID = c.ID,
                                UserName = c.UserName
                            });
            var list = sqlQuery.ToList<RoleUserView>();
            return list;
        }

        /// <summary>
        /// 获取用户邮件地址
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns>用户实体对象</returns>
        internal UserEntity GetUserEntity(string userID)
        {
            var userEntity = Repository.GetById<UserEntity>(userID);
            return userEntity;
        }

        /// <summary>
        /// 根据角色ID获取用户角色视图
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns>角色用户列表</returns>
        internal List<RoleUserView> GetUserByRole(int roleID)
        {
            //var sql = @"SELECT 
            //                A.ID AS RoleID, 
            //                A.RoleCode, 
            //                A.RoleName,
            //                C.ID AS UserID,
            //                C.UserName
            //            FROM SysRole A
            //            INNER JOIN SysRoleUser B 
            //                ON A.ID = B.RoleID
            //            INNER JOIN SysUser C
            //                ON B.UserID = C.ID
            //            WHERE A.ID = @roleID
            //            ORDER BY A.ID, C.ID                        
            //            ";
            //var list = Repository.Query<RoleUserView>(sql, new
            //{
            //    roleID = roleID
            //}).ToList<RoleUserView>();
            var sqlQuery = (from a in Repository.GetAll<RoleEntity>()
                            join b in Repository.GetAll<RoleUserEntity>() 
                                on a.ID equals b.RoleID
                            join c in Repository.GetAll<UserEntity>() 
                                on b.UserID equals c.ID
                            where a.ID == roleID
                            orderby a.ID, c.ID
                            select new RoleUserView
                            {
                                RoleID = a.ID,
                                RoleCode = a.RoleCode,
                                RoleName = a.RoleName,
                                UserID = c.ID,
                                UserName = c.UserName
                            });
            var list = sqlQuery.ToList<RoleUserView>();
            return list;
        }

        /// <summary>
        /// 根据角色iD列表获取角色用户列表数据
        /// </summary>
        /// <param name="idsin">多个ID数组</param>
        /// <returns>角色用户列表</returns>
        internal List<RoleUserView> GetUserByRoleIDs(string[] idsin)
        {
            //var sql = @"SELECT 
            //                A.ID AS RoleID, 
            //                A.RoleCode, 
            //                A.RoleName,
            //                C.ID AS UserID,
            //                C.UserName
            //            FROM SysRole A
            //            INNER JOIN SysRoleUser B 
            //                ON A.ID = B.RoleID
            //            INNER JOIN SysUser C
            //                ON B.UserID = C.ID
            //            WHERE A.ID IN @roleIDs
            //            ORDER BY A.ID, C.ID                        
            //            ";
            //var list = Repository.Query<RoleUserView>(sql, new
            //{
            //    roleIDs = idsin
            //}).ToList<RoleUserView>();
            var sqlQuery = (from a in Repository.GetAll<RoleEntity>()
                            join b in Repository.GetAll<RoleUserEntity>() 
                                on a.ID equals b.RoleID
                            join c in Repository.GetAll<UserEntity>() 
                                on b.UserID equals c.ID
                            where idsin.Contains(a.ID.ToString())
                            orderby a.ID, c.ID
                            select new RoleUserView
                            {
                                RoleID = a.ID,
                                RoleCode = a.RoleCode,
                                RoleName = a.RoleName,
                                UserID = c.ID,
                                UserName = c.UserName
                            });
            var list = sqlQuery.ToList<RoleUserView>();
            return list;
        }

        /// <summary>
        /// 根据角色获取用户列表
        /// </summary>
        /// <param name="roleIDs">多个角色ID</param>
        /// <returns>用户列表</returns>
        internal List<User> GetUserListByRoles(string[] idsin)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetUserListByRoles(idsin, session);
            }
        }

        /// <summary>
        /// 根据角色获取用户列表
        /// </summary>
        /// <param name="roleIDs">多个角色ID</param>
        /// <param name="session">会话</param>
        /// <returns>用户列表</returns>
        internal List<User> GetUserListByRoles(string[] idsin, IDbSession session)
        {
            //var sql = @"SELECT DISTINCT 
            //                C.ID AS UserID,
            //                C.UserName
            //            FROM SysRole A
            //            INNER JOIN SysRoleUser B 
            //                ON A.ID = B.RoleID
            //            INNER JOIN SysUser C
            //                ON B.UserID = C.ID
            //            WHERE A.ID IN @roleIDs
            //            ORDER BY C.ID
            //            ";

            //var list = Repository.Query<User>(session.Connection,
            //    sql,
            //    new
            //    {
            //        roleIDs = idsin
            //    },
            //    session.Transaction).ToList<User>();
            var sqlQuery = (from a in Repository.GetAll<RoleEntity>(session.Connection, session.Transaction)
                            join b in Repository.GetAll<RoleUserEntity>(session.Connection, session.Transaction) 
                                on a.ID equals b.RoleID
                            join c in Repository.GetAll<UserEntity>(session.Connection, session.Transaction) 
                                on b.UserID equals c.ID
                            where idsin.Contains(a.ID.ToString())
                            orderby a.ID, c.ID
                            select new User
                            {
                                UserID = c.ID.ToString(),
                                UserName = c.UserName
                            });
            var list = sqlQuery.ToList<User>();
            return list;
        }

        /// <summary>
        /// 根据角色ID获取用户列表
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns>用户列表</returns>
        internal List<User> GetUserListByRole(string roleID)
        {
            //var sql = @"SELECT 
            //                C.ID AS UserID,
            //                C.UserName
            //            FROM SysRole A
            //            INNER JOIN SysRoleUser B 
            //                ON A.ID = B.RoleID
            //            INNER JOIN SysUser C
            //                ON B.UserID = C.ID
            //            WHERE A.ID = @roleID
            //            ORDER BY A.ID, C.ID                        
            //            ";
            //var list = Repository.Query<User>(sql, new
            //{
            //    roleID = roleID
            //}).ToList<User>();
            var sqlQuery = (from a in Repository.GetAll<RoleEntity>()
                            join b in Repository.GetAll<RoleUserEntity>() 
                                on a.ID equals b.RoleID
                            join c in Repository.GetAll<UserEntity>() 
                                on b.UserID equals c.ID
                            where a.ID == int.Parse(roleID)
                            orderby a.ID, c.ID
                            select new User
                            {
                                UserID = c.ID.ToString(),
                                UserName = c.UserName
                            });
            var list = sqlQuery.ToList<User>();
            return list;
        }
       

        /// <summary>
        /// 根据角色用户视图，封装前端角色列表
        /// </summary>
        /// <param name="roleUserView">角色用户视图</param>
        /// <returns>角色列表</returns>
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
