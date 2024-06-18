
namespace Slickflow.Module.Resource
{
    /// <summary>
    /// 角色对象
    /// </summary>
    [Table("SysRole")]
    public class RoleEntity
    {
        public int ID { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
    }
}
