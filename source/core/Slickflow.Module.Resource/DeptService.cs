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
        IList<User> GetUserListByDeptRank(string[] roleIDs, string curUserId, int receiverType);
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
        public IList<User> GetUserListByDeptRank(string[] roleIds, string curUserId, int receiverType)
        {
            using (var session = SessionFactory.CreateSession())
            {
                //默认取节点上定义的角色用户列表
                //Default to retrieve the list of role users defined on the node
                if (receiverType == 1) return GetSuperiorList(roleIds, int.Parse(curUserId), session);
                else if (receiverType == 2) return GetCompeerList(roleIds, int.Parse(curUserId), session);
                else if (receiverType == 3) return GetSubordinateList(roleIds, int.Parse(curUserId), session);
                else return null;
            }
        }

        /// <summary>
        /// Get the list of superiors
        /// 获取上司列表
        /// </summary>
        private IList<User> GetSuperiorList(string[] roleIds, int curUserId, IDbSession session)
        {
            var sql = @"SELECT 
                            U.id AS UserId,
                            U.user_name AS UserName
                        FROM sys_user U
                        INNER JOIN sys_employee_manager EM
                           ON U.id = EM.manager_user_id
                        INNER JOIN sys_role_user RU
                           ON U.id = RU.user_id
                        WHERE RU.role_id in @roleIds
                            AND EM.employee_user_id = @curUserId";
            var list = Repository.Query<User>(session.Connection,
                sql,
                new
                {
                    roleIds = roleIds,
                    curUserId = curUserId
                },
                session.Transaction).ToList();
            return list;
        }

        /// <summary>
        /// Get colleague list
        /// 获取同事列表
        /// </summary>
        private IList<User> GetCompeerList(string[] roleIds, int curUserId, IDbSession session)
        {
            var sql = @"SELECT 
            	            U.id AS UserId,
            	            U.user_name AS UserName
                        FROM sys_user U
                        INNER JOIN sys_employee_manager EM
            	            ON U.id = EM.employee_user_id
                        INNER JOIN sys_role_user RU
                            ON U.id = RU.user_id
                        WHERE RU.role_id in @roleIds
                            AND EM.manager_user_id IN
            	                (
            		                SELECT 
            			                manager_user_id
            		                FROM sys_employee_manager
            		                WHERE employee_user_id = @curUserId
            	                )";
            var list = Repository.Query<User>(session.Connection,
                sql,
                new
                {
                    roleIds = roleIds,
                    curUserId = curUserId
                },
                session.Transaction).ToList();
            return list;
        }

        /// <summary>
        /// Get the list of subordinates
        /// 获取下属列表
        /// </summary>
        private IList<User> GetSubordinateList(string[] roleIds, int curUserId, IDbSession session)
        {
            var sql = @"SELECT 
            	            U.id AS UserId,
            	            U.user_name AS UserName
                        FROM sys_user U
                        INNER JOIN sys_employee_manager EM
            	            ON U.id = EM.employee_user_id
                        INNER JOIN sys_role_user RU
                            ON U.id = RU.user_id
                        WHERE RU.role_id in @roleIds 
                            AND EM.manager_user_id = @curUserId";
            var list = Repository.Query<User>(session.Connection,
                sql,
                new
                {
                    roleIds = roleIds,
                    curUserId = curUserId
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
        public IList<User> GetUserListByDeptRank(string[] roleIds, string curUserId, int receiverType)
        {
            //查询Transition上有前置定义的接收者类型的用户集合
            //Query the set of users with pre-defined receiver types on Transition
            var param = new DynamicParameters();
            param.Add("@roleIds", String.Join(",", roleIds));
            param.Add("@curUserId", int.Parse(curUserId));
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
