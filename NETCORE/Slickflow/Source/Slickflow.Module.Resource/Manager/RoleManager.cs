using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Module.Resource.Data;
using Slickflow.Module.Resource.Entity;

namespace Slickflow.Module.Resource.Manager
{
    /// <summary>
    /// 角色管理类
    /// </summary>
    internal class RoleManager
    {
        /// <summary>
        /// 获取所有角色数据
        /// </summary>
        /// <returns>角色列表</returns>
        internal List<RoleEntity> GetAll()
        {
            using (var session = DbFactory.CreateSession())
            {
                var list = session.GetRepository<RoleEntity>().GetDbSet()
                    .OrderBy(e => e.RoleName)
                    .ToList();
                return list;
            }
        }
    }
}