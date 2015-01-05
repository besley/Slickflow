using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Service
{
    public class RoleService
    {
        public List<RoleEntity> GetRoleAll()
        {
            var roleManager = new RoleManager();
            return roleManager.GetAll();
        }
    }
}
