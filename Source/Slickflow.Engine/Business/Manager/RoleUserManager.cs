using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Business.Manager
{
    public class RoleUserManager : ManagerBase
    {
        public List<RoleUserItem> GetRoleUserTree()
        {
            var strSQL = @"SELECT 
                                R.ID as RoleID,
                                R.RoleName as RoleName,
                                R.RoleCode as RoleCode,
                                U.ID as UserID,
                                U.UserName
                           FROM SysUser U
                           INNER JOIN SysRoleUser RU
                                ON U.ID = RU.UserID
                           INNER JOIN SysRole R
                                ON R.ID = RU.RoleID
                           ORDER BY R.ID,
                                U.ID";
            List<RoleUserItem> list = Repository.Query<RoleUserItem>(strSQL, null).ToList();
            return list;
        }
    }
}
