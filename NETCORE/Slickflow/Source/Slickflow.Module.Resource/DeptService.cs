using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Slickflow.Data;

namespace Slickflow.Module.Resource
{
    #region 接口及创建工厂类
    /// <summary>
    /// 部门组织机构接口
    /// </summary>
    public interface IDeptService
    {
        IList<User> GetUserListByDeptRank(string[] roleIDs, string curUserID, int receiverType);
    }

    /// <summary>
    /// 部门组织机构创建工厂类
    /// </summary>
    public class DeptServiceFactory
    {
        /// <summary>
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
    #endregion

    #region 数据库表SQL查询实现
    /// <summary>
    /// SQL表语句实现用户级别查询
    /// </summary>
    public class DeptServiceQuery: ManagerBase, IDeptService
    {
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
        public IList<User> GetUserListByDeptRank(string[] roleIDs, string curUserID, int receiverType)
        {
            using (var session = SessionFactory.CreateSession())
            {
                //默认取节点上定义的角色用户列表
                if (receiverType == 1) return GetSuperiorList(roleIDs, int.Parse(curUserID), session);
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
        private IList<User> GetSuperiorList(string[] roleIDs, int curUserID, IDbSession session)
        {
            //var sql = @"SELECT 
            //                U.ID AS UserID,
            //                U.UserName
            //            FROM SysUser U
            //            INNER JOIN SysEmployeeManager EM
            //               ON U.ID = EM.MgrUserID
            //            INNER JOIN SysRoleUser RU
            //               ON U.ID = RU.UserID
            //            WHERE RU.RoleID in @roleIDs
            //                AND EM.EmpUserID = @curUserID";
            //var list = Repository.Query<User>(session.Connection,
            //    sql,
            //    new
            //    {
            //        roleIDs = roleIDs,
            //        curUserID = curUserID
            //    },
            //    session.Transaction).ToList();
            var sqlQuery = (from u in Repository.GetAll<UserEntity>(session.Connection, session.Transaction)
                            join em in Repository.GetAll<EmpManagerEntity>(session.Connection, session.Transaction)
                                on u.ID equals em.MgrUserID
                            join ru in Repository.GetAll<RoleUserEntity>(session.Connection, session.Transaction)
                                on u.ID equals ru.UserID
                            where em.EmpUserID == curUserID
                                && roleIDs.Contains(ru.RoleID.ToString())
                            select new User { 
                                UserID = u.ID.ToString(),
                                UserName = u.UserName
                            });
            var list = sqlQuery.ToList<User>();
            return list;
        }

        /// <summary>
        /// 获取同事列表
        /// </summary>
        /// <param name="roleIDs">角色列表</param>
        /// <param name="curUserID">当前用户Id</param>
        /// <param name="session">会话</param>
        /// <returns>用户列表</returns>
        private IList<User> GetCompeerList(string[] roleIDs, int curUserID, IDbSession session)
        {
            //var sql = @"SELECT 
            //	            U.ID AS UserID,
            //	            U.UserName
            //            FROM SysUser U
            //            INNER JOIN SysEmployeeManager EM
            //	            ON U.ID = EM.EmpUserID
            //            INNER JOIN SysRoleUser RU
            //                ON U.ID = RU.UserID
            //            WHERE RU.RoleID in @roleIDs
            //                AND EM.MgrUserID IN
            //	                (
            //		                SELECT 
            //			                MgrUserID
            //		                FROM SysEmployeeManager
            //		                WHERE EmpUserID = @curUserID
            //	                )";
            //var list = Repository.Query<User>(session.Connection,
            //    sql,
            //    new
            //    {
            //        roleIDs = roleIDs,
            //        curUserID = curUserID
            //    },
            //    session.Transaction).ToList();
            // 首选列表查询EmpManager表
            var mgrQuery = (from emb in Repository.GetAll<EmpManagerEntity>(session.Connection, session.Transaction)
                            where emb.EmpUserID == curUserID
                            select new EmpManagerEntity
                            {
                                MgrUserID = emb.MgrUserID
                            });
            var mgrList = mgrQuery.ToList<EmpManagerEntity>();

            // 关联查询
            var sqlQuery = (from u in Repository.GetAll<UserEntity>(session.Connection, session.Transaction)
                            join ema in Repository.GetAll<EmpManagerEntity>(session.Connection, session.Transaction)
                                on u.ID equals ema.EmpUserID
                            join ru in Repository.GetAll<RoleUserEntity>(session.Connection, session.Transaction)
                                on u.ID equals ru.UserID
                            join emb in mgrList
                                on ema.MgrUserID equals emb.MgrUserID
                            where roleIDs.Contains(ru.RoleID.ToString())
                            select new User
                            {
                                UserID = u.ID.ToString(),
                                UserName = u.UserName
                            });
            var list = sqlQuery.ToList<User>();
            return list;
        }

        /// <summary>
        /// 获取下属列表
        /// </summary>
        /// <param name="roleIDs">角色列表</param>
        /// <param name="curUserID">当前用户Id</param>
        /// <param name="session">会话</param>
        /// <returns>用户列表</returns>
        private IList<User> GetSubordinateList(string[] roleIDs, int curUserID, IDbSession session)
        {
            //var sql = @"SELECT 
            //	            U.ID AS UserID,
            //	            U.UserName
            //            FROM SysUser U
            //            INNER JOIN SysEmployeeManager EM
            //	            ON U.ID = EM.EmpUserID
            //            INNER JOIN SysRoleUser RU
            //                ON U.ID = RU.UserID
            //            WHERE RU.RoleID in @roleIDs 
            //                AND EM.MgrUserID = @curUserID";
            //var list = Repository.Query<User>(session.Connection,
            //    sql,
            //    new
            //    {
            //        roleIDs = roleIDs,
            //        curUserID = curUserID
            //    },
            //    session.Transaction).ToList();
            var sqlQuery = (from u in Repository.GetAll<UserEntity>(session.Connection, session.Transaction)
                            join em in Repository.GetAll<EmpManagerEntity>(session.Connection, session.Transaction)
                                on u.ID equals em.EmpUserID
                            join ru in Repository.GetAll<RoleUserEntity>(session.Connection, session.Transaction)
                                on u.ID equals ru.UserID
                            where em.MgrUserID == curUserID
                                && roleIDs.Contains(ru.RoleID.ToString())
                            select new User
                            {
                                UserID = u.ID.ToString(),
                                UserName = u.UserName
                            });
            var list = sqlQuery.ToList<User>();
            return list;
        }
    }
    #endregion

    #region 存储过程查询实现
    /// <summary>
    /// 存储过程实现用户级别查询
    /// </summary>
    public class DeptServiceSP: ManagerBase, IDeptService
    {
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
        public IList<User> GetUserListByDeptRank(string[] roleIDs, string curUserID, int receiverType)
        {
            //查询Transition上有前置定义的接收者类型的用户集合
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
            IList<User> userList = null;
            if (receiverList != null && receiverList.Count > 0)
                userList = receiverList;

            return userList;
        }
        #endregion
    }
}
