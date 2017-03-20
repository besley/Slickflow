using System;

namespace Slickflow.WebDemoV2._0.Entity
{
    //SysRoleUser
    public class SysRoleUserEntity
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
        /// RoleID
        /// </summary>		
        private int _roleid;
        public int RoleID
        {
            get { return _roleid; }
            set { _roleid = value; }
        }

        /// <summary>
        /// UserID
        /// </summary>		
        private int _userid;
        public int UserID
        {
            get { return _userid; }
            set { _userid = value; }
        }


    }
}