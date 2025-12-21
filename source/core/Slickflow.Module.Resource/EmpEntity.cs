
namespace Slickflow.Module.Resource
{
    /// <summary>
    /// Employee Entity
    /// 员工实体对象
    /// </summary>
    [Table("sys_employee")]
    public class EmpEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("dept_id")]
        public int DeptId { get; set; }
        [Column("emp_code")]
        public string EmpCode { get; set; }
        [Column("emp_name")]
        public string EmpName { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("mobile")]
        public string Mobile { get; set; }
        [Column("email")]
        public string EMail { get; set; }
        [Column("manager_id")]
        public int ManagerId { get; set; }
        [Column("remark")]
        public string Remark { get; set; }
    }
}
