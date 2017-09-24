using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MEDECAWebApp.Startup))]
namespace MEDECAWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
