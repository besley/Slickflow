using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
namespace Slickflow.MvcDemo.Models
{
    // 可以通过向 ApplicationUser 类添加更多属性来为用户添加配置文件数据。若要了解详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=317594。
    public class ApplicationUser : IdentityUser<int, IntUserLogin, IntUserRole, IntUserClaim>
    {
        public int Sex { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 昵称,如井先生，井小姐，只包含姓，不包含名
        /// </summary>
        public string PetName { get; set; }
        public string HomeTown { get; set; }
        public ApplicationUser() { }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }
        public ApplicationUser(string name) : this() { UserName = name; }
    }


    public class IntRole : IdentityRole<int, IntUserRole>
    {
        public IntRole()
        {

        }

        /// <summary>
        /// extension rolecode
        /// </summary>
        public string RoleDescript { get; set; }
        /// <summary>
        /// extension contructed function 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="roleCode"></param>
        public IntRole(string name, string roleCode)
        {
            this.Name = name;
            this.RoleDescript = roleCode;
        }

        public IntRole(string name) : this() { Name = name; }
    }
    public class IntUserRole : IdentityUserRole<int> { }
    public class IntUserClaim : IdentityUserClaim<int> { }
    public class IntUserLogin : IdentityUserLogin<int> { }

    public class IntUserContext : IdentityDbContext<ApplicationUser, IntRole, int, IntUserLogin, IntUserRole, IntUserClaim>
    {
        public IntUserContext()
            : base("WfDBConnectionString")
        {

        }
    }

    public class IntUserStore : UserStore<ApplicationUser, IntRole, int, IntUserLogin, IntUserRole, IntUserClaim>
    {
        public IntUserStore(DbContext context)
            : base(context)
        {

        }
    }
    public class IntRoleStore : RoleStore<IntRole, int, IntUserRole>
    {
        public IntRoleStore(DbContext context)
            : base(context)
        {

        }
    }
    //public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IntRole, int, IntUserLogin, IntUserRole, IntUserClaim>
    //{
    //    public ApplicationDbContext()
    //        : base("DefaultConnection", throwIfV1Schema: false)
    //    {

    //    }

    //    public static ApplicationDbContext Create()
    //    {
    //        return new ApplicationDbContext();
    //    }
    //}
    public class ApplicationMaiLaFeng : IdentityDbContext<ApplicationUser, IntRole, int, IntUserLogin, IntUserRole, IntUserClaim>
    {
        public ApplicationMaiLaFeng() : base("WfDBConnectionString") { }
        public static ApplicationMaiLaFeng Create()
        {
            return new ApplicationMaiLaFeng();
        }

        
    }
}