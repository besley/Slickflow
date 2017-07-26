using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlClient;

using Slickflow.WebDemo.Common;
using Slickflow.WebDemo.Entity;


namespace Slickflow.WebDemo.Data
{
    public class WorkFlowManager
    {
        #region SysRole
        /// <summary>
        /// 获取系统角色
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSysRole()
        {
            string strSql = "select * from SysRole";
            return SQLHelper.ExecuteDataset(strSql).Tables[0];
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
            string strSql = string.Format("select * from SysRoleUser where 1=1 and RoleID={0}", roleId);
            return SQLHelper.ExecuteDataset(strSql).Tables[0];
        }
        #endregion

        #region SysUser

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public static SysUserEntity GetSysUserModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID, UserName  ");
            strSql.Append("  from SysUser ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;

            DataSet ds = SQLHelper.ExecuteDataset(strSql.ToString(), parameters);
            return GetSysUserModel(ds.Tables[0]);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        private static SysUserEntity GetSysUserModel(DataTable dt)
        {
            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
            {
                SysUserEntity model = new SysUserEntity();
                if (dt.Rows[0]["ID"].ToString() != "")
                {
                    model.ID = int.Parse(dt.Rows[0]["ID"].ToString());
                }
                model.UserName = dt.Rows[0]["UserName"].ToString();
                return model;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 获取用户集合
        /// </summary>
        /// <param name="roleID">角色ID集合</param>
        /// <returns></returns>
        public static DataTable GetSysUserByRoleIdList(string roleIdList)
        {
            string strSql = string.Format("select u.* from SysUser u where 1=1 and u.ID in(select r.UserID from SysRoleUser r where r.RoleID in ({0}))", roleIdList);
            return SQLHelper.ExecuteDataset(strSql).Tables[0];

        }

        /// <summary>
        /// 获取用户集合
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        public static DataTable GetSysUser(int roleID)
        {
            string strSql = string.Format("select u.* from SysUser u where 1=1 and u.ID in(select r.UserID from SysRoleUser r where r.RoleID={0})", roleID);
            return SQLHelper.ExecuteDataset(strSql).Tables[0];
        }

        /// <summary>
        /// 获取用户集合
        /// </summary>
        /// <param name="sqlWhere">查询条件</param>
        /// <returns></returns>
        public static DataTable GetSysUser(string sqlWhere)
        {
            string strSql = string.Format("select * from SysUser  where 1=1 {0}", sqlWhere);
            return SQLHelper.ExecuteDataset(strSql).Tables[0];
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HrsLeave(");
            strSql.Append("LeaveType,Days,FromDate,ToDate,CurrentActivityText,Status,CreatedUserID,CreatedUserName,CreatedDate");
            strSql.Append(") values (");
            strSql.Append("@LeaveType,@Days,@FromDate,@ToDate,@CurrentActivityText,@Status,@CreatedUserID,@CreatedUserName,@CreatedDate");
            strSql.Append(") ");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
			            new SqlParameter("@LeaveType", SqlDbType.SmallInt,2) ,            
                        new SqlParameter("@Days", SqlDbType.Decimal,18) ,            
                        new SqlParameter("@FromDate", SqlDbType.DateTime,3) ,            
                        new SqlParameter("@ToDate", SqlDbType.DateTime,3) ,            
                        new SqlParameter("@CurrentActivityText", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Status", SqlDbType.Int,4) ,            
                        new SqlParameter("@CreatedUserID", SqlDbType.Int,4) ,            
                        new SqlParameter("@CreatedUserName", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@CreatedDate", SqlDbType.DateTime,3)             
                };
            int idx = 0;
            parameters[idx++].Value = model.LeaveType;
            parameters[idx++].Value = model.Days;
            parameters[idx++].Value = model.FromDate;
            parameters[idx++].Value = model.ToDate;
            parameters[idx++].Value = model.CurrentActivityText;
            parameters[idx++].Value = model.Status;
            parameters[idx++].Value = model.CreatedUserID;
            parameters[idx++].Value = model.CreatedUserName;
            parameters[idx++].Value = model.CreatedDate;
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }


        /// <summary>
        /// 更新请假活动步骤数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool UpdateHrsLeave(HrsLeaveEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update HrsLeave set ");

            strSql.Append(" DepManagerRemark = @DepManagerRemark , ");
            strSql.Append(" DirectorRemark = @DirectorRemark , ");
            strSql.Append(" DeputyGeneralRemark = @DeputyGeneralRemark , ");
            strSql.Append(" GeneralManagerRemark = @GeneralManagerRemark , ");
            strSql.Append(" CurrentActivityText = @CurrentActivityText ");
            strSql.Append(" where ID=@ID ");

            SqlParameter[] parameters = {
			            new SqlParameter("@ID", SqlDbType.Int,4) ,            
                        new SqlParameter("@DepManagerRemark", SqlDbType.NVarChar,50),
                        new SqlParameter("@DirectorRemark", SqlDbType.NVarChar,50),
                        new SqlParameter("@DeputyGeneralRemark", SqlDbType.NVarChar,50),
                        new SqlParameter("@GeneralManagerRemark", SqlDbType.NVarChar,50),
                        new SqlParameter("@CurrentActivityText", SqlDbType.NVarChar,50)
            };
            int idx = 0;
            parameters[idx++].Value = model.ID;
            parameters[idx++].Value = model.DepManagerRemark;
            parameters[idx++].Value = model.DirectorRemark;
            parameters[idx++].Value = model.DeputyGeneralRemark;
            parameters[idx++].Value = model.GeneralManagerRemark;
            parameters[idx++].Value = model.CurrentActivityText;
            int rows = SQLHelper.ExecuteNonQuery(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        /// <summary>
        /// 得到一个请假对象实体
        /// </summary>
        /// <param name="ID">请假ID</param>
        /// <returns></returns>
        public static HrsLeaveEntity GetHrsLeaveModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID, LeaveType, Days, FromDate, ToDate, CurrentActivityText, Status, CreatedUserID, CreatedUserName, CreatedDate,DepManagerRemark,DirectorRemark,DeputyGeneralRemark,GeneralManagerRemark  ");
            strSql.Append("  from HrsLeave ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;

            DataSet ds = SQLHelper.ExecuteDataset(strSql.ToString(), parameters);
            return GetHrsLeaveModel(ds.Tables[0]);
        }



        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        private static HrsLeaveEntity GetHrsLeaveModel(DataTable dt)
        {
            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
            {
                HrsLeaveEntity model = new HrsLeaveEntity();
                if (dt.Rows[0]["ID"].ToString() != "")
                {
                    model.ID = int.Parse(dt.Rows[0]["ID"].ToString());
                }
                if (dt.Rows[0]["LeaveType"].ToString() != "")
                {
                    model.LeaveType = int.Parse(dt.Rows[0]["LeaveType"].ToString());
                }
                if (dt.Rows[0]["Days"].ToString() != "")
                {
                    model.Days = decimal.Parse(dt.Rows[0]["Days"].ToString());
                }
                if (dt.Rows[0]["FromDate"].ToString() != "")
                {
                    model.FromDate = DateTime.Parse(dt.Rows[0]["FromDate"].ToString());
                }
                if (dt.Rows[0]["ToDate"].ToString() != "")
                {
                    model.ToDate = DateTime.Parse(dt.Rows[0]["ToDate"].ToString());
                }
                model.CurrentActivityText = dt.Rows[0]["CurrentActivityText"].ToString();
                if (dt.Rows[0]["Status"].ToString() != "")
                {
                    model.Status = int.Parse(dt.Rows[0]["Status"].ToString());
                }
                if (dt.Rows[0]["CreatedUserID"].ToString() != "")
                {
                    model.CreatedUserID = int.Parse(dt.Rows[0]["CreatedUserID"].ToString());
                }
                model.CreatedUserName = dt.Rows[0]["CreatedUserName"].ToString();
                if (dt.Rows[0]["CreatedDate"].ToString() != "")
                {
                    model.CreatedDate = DateTime.Parse(dt.Rows[0]["CreatedDate"].ToString());
                }

                model.DepManagerRemark = dt.Rows[0]["DepManagerRemark"].ToString();
                model.DirectorRemark = dt.Rows[0]["DirectorRemark"].ToString();
                model.DeputyGeneralRemark = dt.Rows[0]["DeputyGeneralRemark"].ToString();
                model.GeneralManagerRemark = dt.Rows[0]["GeneralManagerRemark"].ToString();

                return model;
            }
            else
            {
                return null;
            }
        }




        /// <summary>
        /// 查询请假列表
        /// </summary>
        /// <param name="sqlWhere">查询条件</param>
        /// <returns></returns>
        public static DataTable GetHrsLeave(string sqlWhere)
        {
            string strSql = string.Format("select * from HrsLeave where 1=1 {0}", sqlWhere);
            return SQLHelper.ExecuteDataset(strSql).Tables[0];
        }


        /// <summary>
        /// 查询请假流程实例
        /// </summary>
        /// <param name="sqlWhere">查询条件</param>
        /// <returns></returns>
        public static DataTable GetHrsLeaveProcessInstance(string sqlWhere)
        {
            string strSql = string.Format(" SELECT i.*,h.* FROM WfProcessInstance i LEFT JOIN HrsLeave h ON i.AppInstanceID = CONVERT(VARCHAR(50), h.ID) where 1=1 {0} order by i.ID DESC ", sqlWhere);
            return SQLHelper.ExecuteDataset(strSql).Tables[0];
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HrsLeaveOpinion(");
            strSql.Append("AppInstanceID,ActivityName,ActivityID,Remark,ChangedTime,ChangedUserID,ChangedUserName");
            strSql.Append(") values (");
            strSql.Append("@AppInstanceID,@ActivityName,@ActivityID,@Remark,@ChangedTime,@ChangedUserID,@ChangedUserName");
            strSql.Append(") ");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                        new SqlParameter("@AppInstanceID", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@ActivityName", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ActivityID", SqlDbType.VarChar,50) ,     
                        new SqlParameter("@Remark", SqlDbType.NVarChar,1000) ,            
                        new SqlParameter("@ChangedTime", SqlDbType.DateTime) ,            
                        new SqlParameter("@ChangedUserID", SqlDbType.VarChar,50) ,  
                        new SqlParameter("@ChangedUserName", SqlDbType.NVarChar,50)             
            };
            int idx = 0;
            parameters[idx++].Value = model.AppInstanceID;
            parameters[idx++].Value = model.ActivityName;
            parameters[idx++].Value = model.ActivityID;
            parameters[idx++].Value = model.Remark;
            parameters[idx++].Value = model.ChangedTime;
            parameters[idx++].Value = model.ChangedUserID;
            parameters[idx++].Value = model.ChangedUserName;

            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }

        }


        /// <summary>
        /// 得到业务公共实体
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static HrsLeaveOpinionEntity GetHrsLeaveOpinionModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID, AppName, AppInstanceID, ActivityName, Remark, ChangedTime, ChangedUserID, ChangedUserName  ");
            strSql.Append("  from HrsLeaveOpinion ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;

            DataSet ds = SQLHelper.ExecuteDataset(strSql.ToString(), parameters);
            return GetHrsLeaveOpinionModel(ds.Tables[0]);
        }

        /// <summary>
        /// 得到业务公共实体
        /// </summary>
        /// <param name="AppInstanceID"></param>
        /// <returns></returns>
        public static HrsLeaveOpinionEntity GetHrsLeaveOpinionByAppInstanceID(string AppInstanceID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID, AppName, AppInstanceID, ActivityName, Remark, ChangedTime, ChangedUserID, ChangedUserName  ");
            strSql.Append("  from HrsLeaveOpinion ");
            strSql.Append(" where AppInstanceID=@AppInstanceID");
            SqlParameter[] parameters = {
					new SqlParameter("@AppInstanceID", SqlDbType.VarChar, 50)
			};
            parameters[0].Value = AppInstanceID;

            DataSet ds = SQLHelper.ExecuteDataset(strSql.ToString(), parameters);
            return GetHrsLeaveOpinionModel(ds.Tables[0]);
        }



        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        private static HrsLeaveOpinionEntity GetHrsLeaveOpinionModel(DataTable dt)
        {
            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
            {
                HrsLeaveOpinionEntity model = new HrsLeaveOpinionEntity();
                if (dt.Rows[0]["ID"].ToString() != "")
                {
                    model.ID = int.Parse(dt.Rows[0]["ID"].ToString());
                }

                model.AppInstanceID = dt.Rows[0]["AppInstanceID"].ToString();
                model.ActivityName = dt.Rows[0]["ActivityName"].ToString();
                model.ActivityID = dt.Rows[0]["ActivityID"].ToString();
                model.Remark = dt.Rows[0]["Remark"].ToString();
                if (dt.Rows[0]["ChangedTime"].ToString() != "")
                {
                    model.ChangedTime = DateTime.Parse(dt.Rows[0]["ChangedTime"].ToString());
                }
                if (dt.Rows[0]["ChangedUserID"].ToString() != "")
                {
                    model.ChangedUserID = dt.Rows[0]["ChangedUserID"].ToString();
                }
                model.ChangedUserName = dt.Rows[0]["ChangedUserName"].ToString();

                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 查询业务流程
        /// </summary>
        /// <param name="sqlWhere">查询条件</param>
        /// <returns></returns>
        public static DataTable GetHrsLeaveOpinion(string sqlWhere)
        {
            string strSql = string.Format("select * from HrsLeaveOpinion where 1=1 {0}", sqlWhere);
            return SQLHelper.ExecuteDataset(strSql).Tables[0];
        }

        /// <summary>
        /// 查询请假处理过程
        /// </summary>
        /// <param name="AppInstanceID">应用实例ID</param>
        /// <returns></returns>
        public static DataTable GetHrsLeaveOpinionListByAppInstanceID(string AppInstanceID)
        {
            string strSql = string.Format("select * from HrsLeaveOpinion where 1=1  and AppInstanceID='{0}'", AppInstanceID);
            return SQLHelper.ExecuteDataset(strSql).Tables[0];
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BizAppFlow(");
            strSql.Append("AppName,AppInstanceID,ActivityName,Remark,ChangedTime,ChangedUserID,ChangedUserName");
            strSql.Append(") values (");
            strSql.Append("@AppName,@AppInstanceID,@ActivityName,@Remark,@ChangedTime,@ChangedUserID,@ChangedUserName");
            strSql.Append(") ");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
			            new SqlParameter("@AppName", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@AppInstanceID", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@ActivityName", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Remark", SqlDbType.NVarChar,1000) ,            
                        new SqlParameter("@ChangedTime", SqlDbType.DateTime) ,            
                        new SqlParameter("@ChangedUserID", SqlDbType.Int,4) ,            
                        new SqlParameter("@ChangedUserName", SqlDbType.NVarChar,50)             
            };
            int idx = 0;
            parameters[idx++].Value = model.AppName;
            parameters[idx++].Value = model.AppInstanceID;
            parameters[idx++].Value = model.ActivityName;
            parameters[idx++].Value = model.Remark;
            parameters[idx++].Value = model.ChangedTime;
            parameters[idx++].Value = model.ChangedUserID;
            parameters[idx++].Value = model.ChangedUserName;

            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }

        }


        /// <summary>
        /// 得到业务公共实体
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static BizAppFlowEntity GetBizAppFlowModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID, AppName, AppInstanceID, ActivityName, Remark, ChangedTime, ChangedUserID, ChangedUserName  ");
            strSql.Append("  from BizAppFlow ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;

            DataSet ds = SQLHelper.ExecuteDataset(strSql.ToString(), parameters);
            return GetBizAppFlowModel(ds.Tables[0]);
        }

        /// <summary>
        /// 得到业务公共实体
        /// </summary>
        /// <param name="AppInstanceID"></param>
        /// <returns></returns>
        public static BizAppFlowEntity GetBizAppFlowModelByAppInstanceID(string AppInstanceID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID, AppName, AppInstanceID, ActivityName, Remark, ChangedTime, ChangedUserID, ChangedUserName  ");
            strSql.Append("  from BizAppFlow ");
            strSql.Append(" where AppInstanceID=@AppInstanceID");
            SqlParameter[] parameters = {
					new SqlParameter("@AppInstanceID", SqlDbType.VarChar, 50)
			};
            parameters[0].Value = AppInstanceID;

            DataSet ds = SQLHelper.ExecuteDataset(strSql.ToString(), parameters);
            return GetBizAppFlowModel(ds.Tables[0]);
        }



        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        private static BizAppFlowEntity GetBizAppFlowModel(DataTable dt)
        {
            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
            {
                BizAppFlowEntity model = new BizAppFlowEntity();
                if (dt.Rows[0]["ID"].ToString() != "")
                {
                    model.ID = int.Parse(dt.Rows[0]["ID"].ToString());
                }
                model.AppName = dt.Rows[0]["AppName"].ToString();
                model.AppInstanceID = dt.Rows[0]["AppInstanceID"].ToString();
                model.ActivityName = dt.Rows[0]["ActivityName"].ToString();
                model.Remark = dt.Rows[0]["Remark"].ToString();
                if (dt.Rows[0]["ChangedTime"].ToString() != "")
                {
                    model.ChangedTime = DateTime.Parse(dt.Rows[0]["ChangedTime"].ToString());
                }
                if (dt.Rows[0]["ChangedUserID"].ToString() != "")
                {
                    model.ChangedUserID = dt.Rows[0]["ChangedUserID"].ToString();
                }
                model.ChangedUserName = dt.Rows[0]["ChangedUserName"].ToString();

                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 查询业务流程
        /// </summary>
        /// <param name="sqlWhere">查询条件</param>
        /// <returns></returns>
        public static DataTable GetBizAppFlow(string sqlWhere)
        {
            string strSql = string.Format("select * from BizAppFlow where 1=1 {0}", sqlWhere);
            return SQLHelper.ExecuteDataset(strSql).Tables[0];
        }

        /// <summary>
        /// 查询业务流程
        /// </summary>
        /// <param name="AppInstanceID">应用实例ID</param>
        /// <returns></returns>
        public static DataTable GetBizAppFlowByAppInstanceID(string AppInstanceID)
        {
            string strSql = string.Format("select * from BizAppFlow where 1=1  and AppInstanceID='{0}'", AppInstanceID);
            return SQLHelper.ExecuteDataset(strSql).Tables[0];
        }

        #endregion



    }
}