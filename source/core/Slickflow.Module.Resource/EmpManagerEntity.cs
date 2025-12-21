
namespace Slickflow.Module.Resource
{
    /// <summary>
    /// Employee Manager Entity
    /// 员工管理人员实体对象
    /// </summary>
    [Table("sys_employee_manager")]
    public class EmpManagerEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("employee_id")]
        public int EmployeeId { get; set; }
        [Column("employee_user_id")]
        public int EmployeeUserId { get; set; }
        [Column("manager_id")]
        public int ManagerId { get; set; }
        [Column("manager_user_id")]
        public int ManagerUserId { get; set; }
    }
}
