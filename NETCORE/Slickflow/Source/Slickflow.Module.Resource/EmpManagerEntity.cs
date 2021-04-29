
namespace Slickflow.Module.Resource
{
    /// <summary>
    /// 员工实体对象
    /// </summary>
    [Table("SysEmployeeManager")]
    public class EmpManagerEntity
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public int EmpUserID { get; set; }
        public int ManagerID { get; set; }
        public int MgrUserID { get; set; }
    }
}
