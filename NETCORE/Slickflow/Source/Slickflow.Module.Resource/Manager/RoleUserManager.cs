using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions;
using Slickflow.Data;
using Slickflow.Module.Resource.Data;
using Slickflow.Module.Resource.Entity;

namespace Slickflow.Module.Resource.Manager
{
    /// <summary>
    /// 角色用户管理类
    /// </summary>
    internal class RoleUserManager
    {
        /// <summary>
        /// 根据角色编码查询用户
        /// </summary>
        /// <param name="roleCode">角色代码</param>
        /// <returns>用户列表</returns>
        internal List<User> GetUserListByRoleCode(string roleCode)
        {
            //var strSQL = @"SELECT 
            //                U.ID as UserID,
            //                U.UserName
            //           FROM SysUser U
            //           INNER JOIN SysRoleUser RU
            //                ON U.ID = RU.UserID
            //           INNER JOIN SysRole R
            //                ON R.ID = RU.RoleID
            //           WHERE R.RoleCode = @roleCode
            //           ORDER BY U.ID";
            using (var session = DbFactory.CreateSession())
            {
                var userDbSet = session.GetRepository<UserEntity>().GetDbSet();
                var roleUserDbSet = session.GetRepository<RoleUserEntity>().GetDbSet();
                var roleDbSet = session.GetRepository<RoleEntity>().GetDbSet();
                var list = (from u in userDbSet
                            join ru in roleUserDbSet on u.ID equals ru.UserID
                            join r in roleDbSet on ru.RoleID equals r.ID
                            where r.RoleCode == roleCode
                            orderby u.ID
                            select u)
                            .ToList();
                var userList = list.Select(e => new User { UserID = e.ID.ToString(), UserName = e.UserName  }).ToList();
                return userList;
            }
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
            //               ORDER BY R.ID, U.ID";
            using (var session = DbFactory.CreateSession())
            {
                var userDbSet = session.GetRepository<UserEntity>().GetDbSet();
                var roleUserDbSet = session.GetRepository<RoleUserEntity>().GetDbSet();
                var roleDbSet = session.GetRepository<RoleEntity>().GetDbSet();
                var list = (from u in userDbSet
                            join ru in roleUserDbSet on u.ID equals ru.UserID
                            join r in roleDbSet on ru.RoleID equals r.ID
                            orderby r.ID, u.ID
                            select new RoleUserView{
                                RoleID =r.ID,
                                RoleName =r.RoleName,
                                RoleCode =r.RoleCode,
                                UserID =u.ID,
                                UserName =u.UserName
                            })
                            .ToList();
                return list;
            }
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
            //            ORDER BY A.ID, C.ID";
            using (var session = DbFactory.CreateSession())
            {
                var userDbSet = session.GetRepository<UserEntity>().GetDbSet();
                var roleUserDbSet = session.GetRepository<RoleUserEntity>().GetDbSet();
                var roleDbSet = session.GetRepository<RoleEntity>().GetDbSet();
                var list = (from u in userDbSet
                            join ru in roleUserDbSet on u.ID equals ru.UserID
                            join r in roleDbSet on ru.RoleID equals r.ID
                            where r.ID == roleID
                            orderby r.ID, u.ID
                            select new RoleUserView
                            {
                                RoleID = r.ID,
                                RoleName = r.RoleName,
                                RoleCode = r.RoleCode,
                                UserID = u.ID,
                                UserName = u.UserName
                            })
                            .ToList();
                return list;
            }
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
            //            ORDER BY A.ID, C.ID";
            using (var session = DbFactory.CreateSession())
            {
                var userDbSet = session.GetRepository<UserEntity>().GetDbSet();
                var roleUserDbSet = session.GetRepository<RoleUserEntity>().GetDbSet();
                var roleDbSet = session.GetRepository<RoleEntity>().GetDbSet();
                var list = (from u in userDbSet
                            join ru in roleUserDbSet on u.ID equals ru.UserID
                            join r in roleDbSet on ru.RoleID equals r.ID
                            where idsin.Contains(r.ID.ToString())
                            orderby r.ID, u.ID
                            select new RoleUserView
                            {
                                RoleID = r.ID,
                                RoleName = r.RoleName,
                                RoleCode = r.RoleCode,
                                UserID = u.ID,
                                UserName = u.UserName
                            })
                            .ToList();
                return list;
            }
        }

        /// <summary>
        /// 根据角色获取用户列表
        /// </summary>
        /// <param name="roleIDs">多个角色ID</param>
        /// <returns>用户列表</returns>
        internal List<User> GetUserListByRoles(string[] roleIDs)
        {
            using (var session = DbFactory.CreateSession())
            {
                return GetUserListByRoles(roleIDs, session);
            }
        }

        /// <summary>
        /// 根据角色获取用户列表
        /// </summary>
        /// <param name="roleIDs">多个角色ID</param>
        /// <param name="session">会话</param>
        /// <returns>用户列表</returns>
        private List<User> GetUserListByRoles(string[] roleIDs, IDbSession session)
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
            //            ORDER BY C.ID";
            var userDbSet = session.GetRepository<UserEntity>().GetDbSet();
            var roleUserDbSet = session.GetRepository<RoleUserEntity>().GetDbSet();
            var roleDbSet = session.GetRepository<RoleEntity>().GetDbSet();
            var list = (from u in userDbSet
                        join ru in roleUserDbSet on u.ID equals ru.UserID
                        join r in roleDbSet on ru.RoleID equals r.ID
                        where roleIDs.Contains(r.ID.ToString())
                        orderby u.ID
                        select new User
                        {
                            UserID = u.ID.ToString(),
                            UserName = u.UserName
                        })
                        .Distinct()
                        .ToList();
            return list;
        }

        /// <summary>
        /// 根据角色ID获取用户列表
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns>用户列表</returns>
        internal List<User> GetUserListByRole(string roleID)
        {
            using (var session = DbFactory.CreateSession())
            {
                return GetUserListByRole(roleID, session);
            }
        }

        /// <summary>
        /// 根据角色ID获取用户列表
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="session">数据上下文</param>
        /// <returns>用户列表</returns>
        private List<User> GetUserListByRole(string roleID, IDbSession session)
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
            //            ORDER BY A.ID, C.ID";
            var userDbSet = session.GetRepository<UserEntity>().GetDbSet();
            var roleUserDbSet = session.GetRepository<RoleUserEntity>().GetDbSet();
            var roleDbSet = session.GetRepository<RoleEntity>().GetDbSet();
            var list = (from u in userDbSet
                        join ru in roleUserDbSet on u.ID equals ru.UserID
                        join r in roleDbSet on ru.RoleID equals r.ID
                        where r.ID == int.Parse(roleID)
                        orderby u.ID
                        select new User
                        {
                            UserID = u.ID.ToString(),
                            UserName = u.UserName
                        })
                            .ToList();
            return list;
        }

        /// <summary>
        /// 根据接收者类型，来获取角色用户树的数据
        /// receiverType 参数说明
        /// 默认:0
        /// 上司:1
        /// 同事:2
        /// 下属:3
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="userID">用户ID</param>
        /// <param name="receiverType">接收者类型</param>
        /// <returns>用户列表</returns>
        internal List<User> GetUserListByRole(string[] roleIDs, string curUserID, int receiverType)
        {
            //查询Transition上有前置定义的接收者类型的用户集合
            using (var session = DbFactory.CreateSession())
            {
                //默认取节点上定义的角色用户列表
                if (receiverType == 0) return GetUserListByRoles(roleIDs, session);
                else if (receiverType == 1) return GetSuperiorList(roleIDs, int.Parse(curUserID), session);
                else if (receiverType == 2) return GetCompeerList(roleIDs, int.Parse(curUserID), session);
                else if (receiverType == 3) return GetSubordinateList(roleIDs, int.Parse(curUserID), session);
                else return null;
            }
        }

        /// <summary>
        /// 获取上司列表
        /// </summary>
        /// <param name="roleIDs">角色列表</param>
        /// <param name="curUserID">当前用户Id</param>
        /// <param name="sesson">数据会话</param>
        /// <returns>用户列表</returns>
        private List<User> GetSuperiorList(string[] roleIDs, int curUserID, IDbSession session)
        {
            //var sql = @"SELECT 
            //                U.ID AS UserID,
            //                U.UserName
            //            FROM SysUser U
            //            INNER JOIN SysEmployeeManager EM
            //               ON U.ID = EM.MgrUserID
            //            INNER JOIN SysRoleUser RU
            //               ON U.ID = RU.UserID
            //            INNER JOIN @tblRoleIDS R
            //               ON R.ID = RU.RoleID
            //            WHERE EM.EmpUserID = @curUserID";
            var userDbSet = session.GetRepository<UserEntity>().GetDbSet();
            var roleUserDbSet = session.GetRepository<RoleUserEntity>().GetDbSet();
            var empMgrDbSet = session.GetRepository<EmpMgrEntity>().GetDbSet();
            var list = (from u in userDbSet
                        join ru in roleUserDbSet on u.ID equals ru.UserID
                        join em in empMgrDbSet on u.ID equals em.MgrUserID 
                        where em.EmpUserID == curUserID && roleIDs.Contains(ru.RoleID.ToString())
                        select new User {
                            UserID = u.ID.ToString(),
                            UserName = u.UserName
                        })
                        .ToList();
            return list;
        }

        /// <summary>
        /// 获取同事列表
        /// </summary>
        /// <param name="roleIDs">角色列表</param>
        /// <param name="curUserID">当前用户Id</param>
        /// <param name="session">会话</param>
        /// <returns>用户列表</returns>
        private List<User> GetCompeerList(string[] roleIDs, int curUserID, IDbSession session)
        {
            //        var sql = @"SELECT 
            //	U.ID AS UserID,
            //	U.UserName
            //FROM SysUser U
            //INNER JOIN SysEmployeeManager EM
            //	ON U.ID = EM.EmpUserID
            //INNER JOIN SysRoleUser RU
            //    ON U.ID = RU.UserID
            //INNER JOIN @tblRoleIDS R
            //    ON R.ID = RU.RoleID
            //WHERE EM.MgrUserID IN
            //	(
            //		SELECT 
            //			MgrUserID
            //		FROM SysEmployeeManager
            //		WHERE EmpUserID = @curUserID
            //	)";
            var userDbSet = session.GetRepository<UserEntity>().GetDbSet();
            var roleUserDbSet = session.GetRepository<RoleUserEntity>().GetDbSet();
            var empMgrDbSet = session.GetRepository<EmpMgrEntity>().GetDbSet();
            var list = (from u in userDbSet
                        join ru in roleUserDbSet on u.ID equals ru.UserID
                        join em in empMgrDbSet on u.ID equals em.EmpUserID
                        join em2 in empMgrDbSet on em.MgrUserID equals em2.MgrUserID
                        where em2.EmpUserID == curUserID && roleIDs.Contains(ru.RoleID.ToString())
                        select new User
                        {
                            UserID = u.ID.ToString(),
                            UserName = u.UserName
                        })
                        .ToList();
            return list;
        }

        /// <summary>
        /// 获取下属列表
        /// </summary>
        /// <param name="roleIDs">角色列表</param>
        /// <param name="curUserID">当前用户Id</param>
        /// <param name="session">会话</param>
        /// <returns>用户列表</returns>
        private List<User> GetSubordinateList(string[] roleIDs, int curUserID, IDbSession session)
        {
            //        var sql = @"SELECT 
            //	U.ID AS UserID,
            //	U.UserName
            //FROM SysUser U
            //INNER JOIN SysEmployeeManager EM
            //	ON U.ID = EM.EmpUserID
            //INNER JOIN SysRoleUser RU
            //    ON U.ID = RU.UserID
            //INNER JOIN @tblRoleIDS R
            //    ON R.ID = RU.RoleID
            //WHERE EM.MgrUserID = @curUserID";
            var userDbSet = session.GetRepository<UserEntity>().GetDbSet();
            var roleUserDbSet = session.GetRepository<RoleUserEntity>().GetDbSet();
            var empMgrDbSet = session.GetRepository<EmpMgrEntity>().GetDbSet();
            var list = (from u in userDbSet
                        join ru in roleUserDbSet on u.ID equals ru.UserID 
                        join em in empMgrDbSet on u.ID equals em.EmpUserID 
                        where em.MgrUserID == curUserID && roleIDs.Contains(ru.RoleID.ToString())
                        select new User
                        {
                            UserID = u.ID.ToString(),
                            UserName = u.UserName
                        })
                        .ToList();
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
