using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Service
{
    /// <summary>
    /// 用户部门关系管理服务接口
    /// </summary>
    public interface IUserRoleService
    {
        PerformerList GetPerformerList(int processInstanceID, List<Role> roles);
    }
}
