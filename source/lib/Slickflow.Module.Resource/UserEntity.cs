
namespace Slickflow.Module.Resource
{
    /// <summary>
    /// User Entity
    /// 用户对象
    /// </summary>
    [Table("SysUser")]
    public class UserEntity
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string EMail { get; set; }
    }
}
