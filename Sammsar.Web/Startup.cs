using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sammsar.Web.Startup))]
namespace Sammsar.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
