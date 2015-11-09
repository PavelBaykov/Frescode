using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Frescode.Startup))]
namespace Frescode
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
