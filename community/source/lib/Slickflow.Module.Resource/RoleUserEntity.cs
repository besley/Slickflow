namespace Slickflow.Module.Resource
{
    /// <summary>
    /// 角色用户关联表
    /// </summary>
    [Table("SysRoleUser")]
    public class RoleUserEntity
    {
        public int ID { get; set; }
        public int RoleID { get; set; }
        public int UserID { get; set; }
    }
}
