using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Slickflow.WebDemoV2._0.Entity
{
    public class BizAppFlowEntity
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
        /// AppName
        /// </summary>		
        private string _appname;
        public string AppName
        {
            get { return _appname; }
            set { _appname = value; }
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