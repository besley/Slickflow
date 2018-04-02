using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slickflow.Module.Resource.Entity
{
    /// <summary>
    /// 角色用户视图对象类
    /// </summary>
    public class RoleUserView
    {
        public int ID { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
