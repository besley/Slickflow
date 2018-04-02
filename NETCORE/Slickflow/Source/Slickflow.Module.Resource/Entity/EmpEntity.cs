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
    [Table("SysEmployee")]
    public class EmpEntity
    {
        public int ID { get; set; }
        public int DeptID { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public int UserID { get; set; }
        public string Mobile { get; set; }
        public string EMail { get; set; }
        public int ManagerID { get; set; }
        public string Remark { get; set; }
    }
}
