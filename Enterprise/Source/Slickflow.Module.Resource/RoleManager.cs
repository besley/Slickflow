using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;

namespace Slickflow.Module.Resource
{
    /// <summary>
    /// 角色管理类
    /// </summary>
    internal class RoleManager : ManagerBase
    {
        /// <summary>
        /// 获取所有角色数据
        /// </summary>
        /// <returns></returns>
        internal List<RoleEntity> GetAll()
        {
            var strSQL = @"SELECT 
                                R.*
                           FROM SysRole R
                           ORDER BY R.RoleName";
            List<RoleEntity> list = Repository.Query<RoleEntity>(strSQL, null).ToList();
            return list;
        }
    }
}