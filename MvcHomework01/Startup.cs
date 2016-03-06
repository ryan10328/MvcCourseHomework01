using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcHomework01.Startup))]
namespace MvcHomework01
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
