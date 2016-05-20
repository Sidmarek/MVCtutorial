using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCtutorial.Startup))]
namespace MVCtutorial
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
