using System.Collections.Generic;
using System.Linq;
using Slickflow.Data;

namespace Slickflow.Module.Resource
{
    /// <summary>
    /// 角色管理类
    /// </summary>
    internal class UserManager : ManagerBase
    {
        /// <summary>
        /// 获取所有用户数据
        /// </summary>
        /// <returns></returns>
        internal List<UserEntity> GetAll()
        {
            var strSQL = @"SELECT 
                                U.*
                           FROM SysUser U
                           ORDER BY U.UserName";
            List<UserEntity> list = Repository.Query<UserEntity>(strSQL, null).ToList();
            return list;
        }

        /// <summary>
        /// 根据用户名称获取用户
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <returns>角色实体</returns>
        internal UserEntity GetByName(string userName)
        {
            var strSQL = @"SELECT
                                U.*
                           FROM SysUser U
                           WHERE UserName=@userName";
            List<UserEntity> list = Repository.Query<UserEntity>(strSQL, new { userName = userName }).ToList();
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