using System;

namespace Slickflow.WebDemo.Entity
{
    //SysRole
    public class SysRoleEntity
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
        /// RoleCode
        /// </summary>		
        private string _rolecode;
        public string RoleCode
        {
            get { return _rolecode; }
            set { _rolecode = value; }
        }

        /// <summary>
        /// RoleName
        /// </summary>		
        private string _rolename;
        public string RoleName
        {
            get { return _rolename; }
            set { _rolename = value; }
        }


    }
}