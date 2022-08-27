using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShackUp.UI.Startup))]
namespace ShackUp.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
