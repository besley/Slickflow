
namespace Slickflow.Module.Resource
{
    /// <summary>
    /// Department Entity
    /// 部门实体对象
    /// </summary>
    [Table("sys_department")]
    public class DeptEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("dept_code")]
        public string DeptCode { get; set; }
        [Column("dept_name")]
        public string DeptName { get; set; }
        [Column("parent_dept_id")]
        public int ParentDeptId { get; set; }
        [Column("description")]
        public string Description { get; set; }
    }
}
