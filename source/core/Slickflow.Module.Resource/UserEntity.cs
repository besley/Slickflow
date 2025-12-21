
namespace Slickflow.Module.Resource
{
    /// <summary>
    /// User Entity
    /// 用户对象
    /// </summary>
    [Table("sys_user")]
    public class UserEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("user_name")]
        public string UserName { get; set; }
        [Column("email")]
        public string EMail { get; set; }
    }
}
