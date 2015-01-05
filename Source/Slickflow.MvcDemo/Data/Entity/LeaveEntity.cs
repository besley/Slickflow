using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Slickflow.Engine.Business.Entity;
namespace Slickflow.MvcDemo.Data.Entity
{
    [Table("HrsLeave")]
    public class LeaveEntity
    {
        /// <summary>
        /// ID
        /// </summary>		
        private int _id;
      
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// LeaveType
        /// </summary>		
        private int _leavetype;
        public int LeaveType
        {
            get { return _leavetype; }
            set { _leavetype = value; }
        }

        /// <summary>
        /// 请假天数
        /// </summary>		
        private decimal _days;
        public decimal Days
        {
            get { return _days; }
            set { _days = value; }
        }

        /// <summary>
        /// 请假开始时间
        /// </summary>		
        private DateTime _fromdate;
        public DateTime FromDate
        {
            get { return _fromdate; }
            set { _fromdate = value; }
        }

        /// <summary>
        /// 请假结束时间
        /// </summary>		
        private DateTime _todate;
        public DateTime ToDate
        {
            get { return _todate; }
            set { _todate = value; }
        }

        /// <summary>
        /// 正在办理的节点
        /// </summary>		
        private string _currentactivitytext;
        public string CurrentActivityText
        {
            get { return _currentactivitytext; }
            set { _currentactivitytext = value; }
        }




        private int _status;
        /// <summary>
        ///    NotStart = 0, 未启动，流程记录为空
        ///    Ready = 1, 准备状态
        ///    Running = 2, 运行状态
        ///    Completed = 4, 完成
        ///    Suspended = 5,挂起
        ///    Canceled = 6,取消
        ///    Discarded = 7终止
        /// </summary>
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// 请假人
        /// </summary>		
        private int _createduserid;
        public int CreatedUserID
        {
            get { return _createduserid; }
            set { _createduserid = value; }
        }

        /// <summary>
        /// 请假人名称
        /// </summary>		
        private string _createdusername;
        public string CreatedUserName
        {
            get { return _createdusername; }
            set { _createdusername = value; }
        }

        /// <summary>
        /// 申请日期
        /// </summary>		
        private DateTime _createddate;
        public DateTime CreatedDate
        {
            get { return _createddate; }
            set { _createddate = value; }
        }


        /// <summary>
        /// 部门经理意见
        /// </summary>		
        private string _DepManagerRemark;
        public string DepManagerRemark
        {
            get { return _DepManagerRemark; }
            set { _DepManagerRemark = value; }
        }

        /// <summary>
        /// 主管总监意见
        /// </summary>		
        private string _DirectorRemark;
        public string DirectorRemark
        {
            get { return _DirectorRemark; }
            set { _DirectorRemark = value; }
        }

        /// <summary>
        /// 副总经理意见
        /// </summary>		
        private string _DeputyGeneralRemark;
        public string DeputyGeneralRemark
        {
            get { return _DeputyGeneralRemark; }
            set { _DeputyGeneralRemark = value; }
        }

        /// <summary>
        /// 总监理办理意见
        /// </summary>		
        private string _GeneralManagerRemark;
        public string GeneralManagerRemark
        {
            get { return _GeneralManagerRemark; }
            set { _GeneralManagerRemark = value; }
        }
    }
}