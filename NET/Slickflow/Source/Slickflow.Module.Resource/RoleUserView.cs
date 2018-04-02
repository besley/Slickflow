using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Module.Resource
{
    /// <summary>
    /// 角色用户视图对象类
    /// </summary>
    public class RoleUserView
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
