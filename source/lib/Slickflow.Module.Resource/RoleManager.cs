using System.Collections.Generic;
using System.Linq;
using Slickflow.Data;

namespace Slickflow.Module.Resource
{
    /// <summary>
    /// Role Manager
    /// 角色管理类
    /// </summary>
    internal class RoleManager : ManagerBase
    {
        /// <summary>
        /// Get all roles
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

        /// <summary>
        /// Get role by role code
        /// 根据角色代码获取角色
        /// </summary>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        internal RoleEntity GetByCode(string roleCode)
        {
            var strSQL = @"SELECT 
                                R.*
                           FROM SysRole R 
                           WHERE RoleCode=@roleCode";
            List<RoleEntity> list = Repository.Query<RoleEntity>(strSQL, new { roleCode = roleCode }).ToList();
            if (list != null && list.Count() == 1)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }
    }
}