using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Slickflow.Data;

namespace Slickflow.Module.Resource
{
    /// <summary>
    /// Department Service Interface
    /// 部门组织机构接口
    /// </summary>
    public interface IDeptService
    {
        IList<User> GetUserListByDeptRank(string[] roleIDs, string curUserID, int receiverType);
    }

    /// <summary>
    /// Department Organizational Structure Creation Factory Category
    /// 部门组织机构创建工厂类
    /// </summary>
    public class DeptServiceFactory
    {
        /// <summary>
        /// Method of creation
        /// Users can extend the implementation class based on their organizational data
        /// Two types of queries have been implemented here“
        /// 1) Table query
        /// 2) Query of stored procedures
        /// 3) Other extensions
        /// 创建方法
        /// 用户可以根据自己的组织机构数据扩展实现类
        /// 此处实现了两种查询“
        /// 1) 表查询
        /// 2) 存储过程查询
        /// 3) 其它扩展
        /// </summary>
        /// <returns></returns>
        public static IDeptService CreateDeptService()
        {
            IDeptService deptService = new DeptServiceQuery();
            //IDeptService deptService = new DeptServiceSP();
            return deptService;
        }
    }

    /// <summary>
    /// Implementing user level queries using SQL table statements
    /// SQL表语句实现用户级别查询
    /// </summary>
    public class DeptServiceQuery: ManagerBase, IDeptService
    {
        /// <summary>
        /// Retrieve data from the role user tree based on the recipient type
        /// Receiver Type parameter description
        /// Default: 0
        /// Superior: 1
        /// Colleagues: 2
        /// Subordinates: 3
        /// 根据接收者类型，来获取角色用户树的数据
        /// receiverType 参数说明
        /// 默认:0
        /// 上司:1
        /// 同事:2
        /// 下属:3
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="userID"></param>
        /// <param name="receiverType"></param>
        /// <returns></returns>
        public IList<User> GetUserListByDeptRank(string[] roleIDs, string curUserID, int receiverType)
        {
            using (var session = SessionFactory.CreateSession())
            {
                //默认取节点上定义的角色用户列表
                //Default to retrieve the list of role users defined on the node
                if (receiverType == 1) return GetSuperiorList(roleIDs, int.Parse(curUserID), session);
                else if (receiverType == 2) return GetCompeerList(roleIDs, int.Parse(curUserID), session);
                else if (receiverType == 3) return GetSubordinateList(roleIDs, int.Parse(curUserID), session);
                else return null;
            }
        }

        /// <summary>
        /// Get the list of superiors
        /// 获取上司列表
        /// </summary>
        /// <param name="roleIDs"></param>
        /// <param name="curUserID"></param>
        /// <param name="sesson"></param>
        /// <returns></returns>
        private IList<User> GetSuperiorList(string[] roleIDs, int curUserID, IDbSession session)
        {
            var sql = @"SELECT 
                            U.ID AS UserID,
                            U.UserName
                        FROM SysUser U
                        INNER JOIN SysEmployeeManager EM
                           ON U.ID = EM.MgrUserID
                        INNER JOIN SysRoleUser RU
                           ON U.ID = RU.UserID
                        WHERE RU.RoleID in @roleIDs
                            AND EM.EmpUserID = @curUserID";
            var list = Repository.Query<User>(session.Connection,
                sql,
                new
                {
                    roleIDs = roleIDs,
                    curUserID = curUserID
                },
                session.Transaction).ToList();
            return list;
        }

        /// <summary>
        /// Get colleague list
        /// 获取同事列表
        /// </summary>
        /// <param name="roleIDs"></param>
        /// <param name="curUserID"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        private IList<User> GetCompeerList(string[] roleIDs, int curUserID, IDbSession session)
        {
            var sql = @"SELECT 
            	            U.ID AS UserID,
            	            U.UserName
                        FROM SysUser U
                        INNER JOIN SysEmployeeManager EM
            	            ON U.ID = EM.EmpUserID
                        INNER JOIN SysRoleUser RU
                            ON U.ID = RU.UserID
                        WHERE RU.RoleID in @roleIDs
                            AND EM.MgrUserID IN
            	                (
            		                SELECT 
            			                MgrUserID
            		                FROM SysEmployeeManager
            		                WHERE EmpUserID = @curUserID
            	                )";
            var list = Repository.Query<User>(session.Connection,
                sql,
                new
                {
                    roleIDs = roleIDs,
                    curUserID = curUserID
                },
                session.Transaction).ToList();
            return list;
        }

        /// <summary>
        /// Get the list of subordinates
        /// 获取下属列表
        /// </summary>
        /// <param name="roleIDs"></param>
        /// <param name="curUserID"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        private IList<User> GetSubordinateList(string[] roleIDs, int curUserID, IDbSession session)
        {
            var sql = @"SELECT 
            	            U.ID AS UserID,
            	            U.UserName
                        FROM SysUser U
                        INNER JOIN SysEmployeeManager EM
            	            ON U.ID = EM.EmpUserID
                        INNER JOIN SysRoleUser RU
                            ON U.ID = RU.UserID
                        WHERE RU.RoleID in @roleIDs 
                            AND EM.MgrUserID = @curUserID";
            var list = Repository.Query<User>(session.Connection,
                sql,
                new
                {
                    roleIDs = roleIDs,
                    curUserID = curUserID
                },
                session.Transaction).ToList();
            return list;
        }
    }

    /// <summary>
    /// Implementing user level queries through stored procedures
    /// 存储过程实现用户级别查询
    /// </summary>
    public class DeptServiceSP: ManagerBase, IDeptService
    {
        /// <summary>
        /// Retrieve data from the role user tree based on the recipient type
        /// Receiver Type parameter description
        /// Default: 0
        /// Superior: 1
        /// Colleagues: 2
        /// Subordinates: 3
        /// 根据接收者类型，来获取角色用户树的数据
        /// receiverType 参数说明
        /// 默认:0
        /// 上司:1
        /// 同事:2
        /// 下属:3
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="userID"></param>
        /// <param name="receiverType"></param>
        /// <returns></returns>
        public IList<User> GetUserListByDeptRank(string[] roleIDs, string curUserID, int receiverType)
        {
            //查询Transition上有前置定义的接收者类型的用户集合
            //Query the set of users with pre-defined receiver types on Transition
            var param = new DynamicParameters();
            param.Add("@roleIDs", String.Join(",", roleIDs));
            param.Add("@curUserID", int.Parse(curUserID));
            param.Add("@receiverType", receiverType);

            List<User> receiverList = null;
            using (var conn = SessionFactory.CreateConnection())
            {
                receiverList = Repository.ExecProcQuery<User>(conn,
                    "pr_sys_DeptUserListRankQuery", param).ToList<User>();
            }

            //返回前置Transition定义级别关系人员列表
            //Return the list of user with defined level relationships in the pre transition definition
            IList<User> userList = null;
            if (receiverList != null && receiverList.Count > 0)
                userList = receiverList;

            return userList;
        }
    }
}
