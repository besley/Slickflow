using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Slickflow.MvcDemo.Startup))]
namespace Slickflow.MvcDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
