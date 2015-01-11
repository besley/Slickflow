using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;
using Slickflow.Admin.Entity;

namespace Slickflow.Admin.Manager
{
    /// <summary>
    /// 角色管理类
    /// </summary>
    public class RoleManager : ManagerBase
    {
        /// <summary>
        /// 根据角色编码查询用户
        /// </summary>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        public List<UserEntity> GetByRoleCode(string roleCode)
        {
            var strSQL = @"SELECT 
                                U.*
                           FROM SysUser U
                           INNER JOIN SysRoleUser RU
                                ON U.ID = RU.UserID
                           INNER JOIN SysRole R
                                ON R.ID = RU.RoleID
                           WHERE R.RoleCode = @roleCode
                           ORDER BY U.ID";
            List<UserEntity> list = Repository.Query<UserEntity>(strSQL, new { roleCode = roleCode }).ToList();
            return list;
        }

        /// <summary>
        /// 获取所有角色数据
        /// </summary>
        /// <returns></returns>
        public List<RoleEntity> GetAll()
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