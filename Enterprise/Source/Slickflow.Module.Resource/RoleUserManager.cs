using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using Slickflow.Data;
using Slickflow.Module.Resource;

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
            List<RoleUserView> list = Repository.Query<RoleUserView>(strSQL, null).ToList();
            return list;
        }

        /// <summary>
        /// 根据角色ID获取用户角色视图
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns>角色用户列表</returns>
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
        /// 根据角色iD列表获取角色用户列表数据
        /// </summary>
        /// <param name="idsin">多个ID数组</param>
        /// <returns>角色用户列表</returns>
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
        /// 根据角色获取用户列表
        /// </summary>
        /// <param name="roleIDs">多个角色ID</param>
        /// <returns>用户列表</returns>
        internal List<User> GetUserByRoles(string[] roleIDs)
        {
            var roles = string.Join(",", roleIDs);
            var sql = @"SELECT DISTINCT 
                            C.ID AS UserID,
                            C.UserName
                        FROM SysRole A
                        INNER JOIN SysRoleUser B 
                            ON A.ID = B.RoleID
                        INNER JOIN SysUser C
                            ON B.UserID = C.ID
                        WHERE A.ID IN(@roleID)
                        ORDER BY C.ID
                        ";

            var list = Repository.Query<User>(sql, new
            {
                roleID = roles
            }).ToList<User>();

            return list;
        }

        /// <summary>
        /// 根据角色ID获取用户列表
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns>用户列表</returns>
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
        internal List<User> GetUserListByRole(string roleID, string curUserID, int receiverType)
        {
            //默认去节点上定义的角色用户列表
            List<User> userList = GetUserListByRole(roleID);
            if (receiverType == 0)
            {
                return userList;
            }

            //查询Transition上有前置定义的接收者类型的用户集合
            var param = new DynamicParameters();
            param.Add("@curUserID", int.Parse(curUserID));
            param.Add("@receiverType", receiverType);

            List<User> receiverList = null;
            using (var conn = SessionFactory.CreateConnection())
            {
                receiverList = Repository.ExecProcQuery<User>(conn,
                    "pr_sys_DeptUserListRankQuery", param).ToList<User>();
            }

            //返回前置Transition定义级别关系人员列表
            if (receiverList != null && receiverList.Count > 0)
                userList = receiverList;

            return userList;
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
