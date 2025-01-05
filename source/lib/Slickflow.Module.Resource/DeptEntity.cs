
namespace Slickflow.Module.Resource
{
    /// <summary>
    /// Department Entity
    /// 部门实体对象
    /// </summary>
    [Table("SysDepartment")]
    public class DeptEntity
    {
        public int ID { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public int ParentID { get; set; }
        public string Description { get; set; }
    }
}
