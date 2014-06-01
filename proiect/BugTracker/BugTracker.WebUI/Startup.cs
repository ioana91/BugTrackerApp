using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BugTracker.WebUI.Startup))]
namespace BugTracker.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
