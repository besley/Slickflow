namespace Slickflow.Module.Resource
{
    /// <summary>
    /// Role User Entity
    /// 角色用户关联表
    /// </summary>
    [Table("sys_role_user")]
    public class RoleUserEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("role_id")]
        public int RoleId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
    }
}
