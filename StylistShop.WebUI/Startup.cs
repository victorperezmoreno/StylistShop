using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StylistShop.WebUI.Startup))]
namespace StylistShop.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
