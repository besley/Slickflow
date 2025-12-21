
namespace Slickflow.Module.Resource
{
    /// <summary>
    /// Role Entity
    /// 角色对象
    /// </summary>
    [Table("sys_role")]
    public class RoleEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("role_code")]
        public string RoleCode { get; set; }
        [Column("role_name")]
        public string RoleName { get; set; }
    }
}
