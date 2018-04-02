using System;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Slickflow.WebDemo.Data;
using Slickflow.WebDemo.Common;
using Slickflow.WebDemo.Entity;


namespace Slickflow.WebDemo.Business
{
    public class WorkFlows
    {
        #region SysRole
        /// <summary>
        /// 获取系统角色
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSysRole()
        {
            return WorkFlowManager.GetSysRole();
        }
        #endregion


        #region SysRoleUser

        /// <summary>
        /// 根据角色获取系统用户
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public static DataTable GetSysRoleUser(int roleId)
        {
            return WorkFlowManager.GetSysRoleUser(roleId);
        }
        #endregion


        #region SysUser
        /// <summary>
        /// 得到用户对象实体
        /// </summary>
        /// <param name="ID">用户ID</param>
        /// <returns></returns>
        public static SysUserEntity GetSysUserModel(int ID)
        {
            return WorkFlowManager.GetSysUserModel(ID);
        }

        /// <summary>
        /// 获取用户集合
        /// </summary>
        /// <param name="roleID">角色ID集合</param>
        /// <returns></returns>
        public static DataTable GetSysUserByRoleIdList(string roleIdList)
        {
            return WorkFlowManager.GetSysUserByRoleIdList(roleIdList);
        }

        /// <summary>
        /// 获取用户集合
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        public static DataTable GetSysUser(int roleID)
        {
            return WorkFlowManager.GetSysUser(roleID);
        }

        /// <summary>
        /// 获取用户集合
        /// </summary>
        /// <param name="sqlWhere">查询条件</param>
        /// <returns></returns>
        public static DataTable GetSysUser(string sqlWhere)
        {
            return WorkFlowManager.GetSysUser(sqlWhere);
        }

        #endregion


        #region HrsLeave
        /// <summary>
        /// 增加一条请假数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int AddHrsLeave(HrsLeaveEntity model)
        {
            return WorkFlowManager.AddHrsLeave(model);
        }
        /// <summary>
        /// 更新请假活动步骤数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool UpdateHrsLeave(HrsLeaveEntity model)
        {
            return WorkFlowManager.UpdateHrsLeave(model);
        }

        /// <summary>
        /// 得到一个请假对象实体
        /// </summary>
        /// <param name="ID">请假ID</param>
        /// <returns></returns>
        public static HrsLeaveEntity GetHrsLeaveModel(int ID)
        {
            return WorkFlowManager.GetHrsLeaveModel(ID);
        }

        /// <summary>
        /// 查询请假列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetHrsLeave()
        {
            return GetHrsLeave(string.Empty);
        }

        /// <summary>
        /// 查询请假列表
        /// </summary>
        /// <param name="sqlWhere">查询条件</param>
        /// <returns></returns>
        public static DataTable GetHrsLeave(string sqlWhere)
        {
            return WorkFlowManager.GetHrsLeave(sqlWhere);
        }

        /// <summary>
        /// 查询请假流程实例
        /// </summary>
        /// <param name="sqlWhere">查询条件</param>
        /// <returns></returns>
        public static DataTable GetHrsLeaveProcessInstance()
        {
            return GetHrsLeaveProcessInstance(string.Empty);
        }

        /// <summary>
        /// 查询请假流程实例
        /// </summary>
        /// <param name="sqlWhere">查询条件</param>
        /// <returns></returns>
        public static DataTable GetHrsLeaveProcessInstance(string sqlWhere)
        {
            return WorkFlowManager.GetHrsLeaveProcessInstance(sqlWhere);
        }
        #endregion

        #region HrsLeaveOpinion
        /// <summary>
        /// 新增一条业务流程数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int AddHrsLeaveOpinion(HrsLeaveOpinionEntity model)
        {
            model.ChangedTime = DateTime.Now;
            return WorkFlowManager.AddHrsLeaveOpinion(model);
        }


        /// <summary>
        /// 得到业务公共实体
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static HrsLeaveOpinionEntity GetHrsLeaveOpinionModel(int ID)
        {
            return WorkFlowManager.GetHrsLeaveOpinionModel(ID);
        }

        /// <summary>
        /// 得到业务公共实体
        /// </summary>
        /// <param name="AppInstanceID"></param>
        /// <returns></returns>
        public static HrsLeaveOpinionEntity GetHrsLeaveOpinionByAppInstanceID(string AppInstanceID)
        {
            return WorkFlowManager.GetHrsLeaveOpinionByAppInstanceID(AppInstanceID);
        }



        /// <summary>
        /// 查询业务流程
        /// </summary>
        /// <param name="sqlWhere">查询条件</param>
        /// <returns></returns>
        public static DataTable GetHrsLeaveOpinion(string sqlWhere)
        {
            return WorkFlowManager.GetHrsLeaveOpinion(sqlWhere);
        }

         /// <summary>
        /// 查询请假处理过程
        /// </summary>
        /// <param name="AppInstanceID">应用实例ID</param>
        /// <returns></returns>
        public static DataTable GetHrsLeaveOpinionListByAppInstanceID(string AppInstanceID)
        {
            return WorkFlowManager.GetHrsLeaveOpinionListByAppInstanceID(AppInstanceID);
        }

        #endregion


        #region BizAppFlow
        /// <summary>
        /// 新增一条业务流程数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int AddBizAppFlow(BizAppFlowEntity model)
        {
            return WorkFlowManager.AddBizAppFlow(model);
        }
        /// <summary>
        /// 得到业务公共实体
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static BizAppFlowEntity GetBizAppFlowModel(int ID)
        {
            return WorkFlowManager.GetBizAppFlowModel(ID);
        }

        /// <summary>
        /// 得到业务公共实体
        /// </summary>
        /// <param name="AppInstanceID"></param>
        /// <returns></returns>
        public static BizAppFlowEntity GetBizAppFlowModelByAppInstanceID(string AppInstanceID)
        {
            return WorkFlowManager.GetBizAppFlowModelByAppInstanceID(AppInstanceID);
        }

        /// <summary>
        /// 查询业务流程
        /// </summary>
        /// <returns></returns>
        public static DataTable GetBizAppFlow()
        {
            return GetBizAppFlow(string.Empty);
        }

        /// <summary>
        /// 查询业务流程
        /// </summary>
        /// <param name="sqlWhere">查询条件</param>
        /// <returns></returns>
        public static DataTable GetBizAppFlow(string sqlWhere)
        {
            return WorkFlowManager.GetBizAppFlow(sqlWhere);
        }

        /// <summary>
        /// 查询业务流程
        /// </summary>
        /// <param name="AppInstanceID">应用实例ID</param>
        /// <returns></returns>
        public static DataTable GetBizAppFlowByAppInstanceID(string AppInstanceID)
        {
            return WorkFlowManager.GetBizAppFlowByAppInstanceID(AppInstanceID);
        }
        #endregion

    }
}