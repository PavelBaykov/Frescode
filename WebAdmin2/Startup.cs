using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAdmin2.Startup))]
namespace WebAdmin2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
