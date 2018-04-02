using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slickflow.Module.Resource.Entity
{
    /// <summary>
    /// 员工实体对象
    /// </summary>
    [Table("SysEmployeeManager")]
    public class EmpMgrEntity
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public int EmpUserID { get; set; }
        public int ManagerID { get; set; }
        public int MgrUserID { get; set; }
    }
}
