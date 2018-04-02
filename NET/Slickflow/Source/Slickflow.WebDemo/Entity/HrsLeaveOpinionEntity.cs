using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Slickflow.WebDemo.Entity
{
    public class HrsLeaveOpinionEntity
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
        /// AppInstanceID
        /// </summary>		
        private string _appinstanceid;
        public string AppInstanceID
        {
            get { return _appinstanceid; }
            set { _appinstanceid = value; }
        }

        /// <summary>
        /// ActivityName
        /// </summary>		
        private string _activityname;
        public string ActivityName
        {
            get { return _activityname; }
            set { _activityname = value; }
        }

        /// <summary>
        /// ActivityID
        /// </summary>		
        private string _activityid;
        public string ActivityID
        {
            get { return _activityid; }
            set { _activityid = value; }
        }

        /// <summary>
        /// Remark
        /// </summary>		
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        /// <summary>
        /// ChangedTime
        /// </summary>		
        private DateTime _changedtime;
        public DateTime ChangedTime
        {
            get { return _changedtime; }
            set { _changedtime = value; }
        }

        /// <summary>
        /// ChangedUserID
        /// </summary>		
        private string _changeduserid;
        public string ChangedUserID
        {
            get { return _changeduserid; }
            set { _changeduserid = value; }
        }

        /// <summary>
        /// ChangedUserName
        /// </summary>		
        private string _changedusername;
        public string ChangedUserName
        {
            get { return _changedusername; }
            set { _changedusername = value; }
        }
    }
}