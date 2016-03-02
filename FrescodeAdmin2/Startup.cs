using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FrescodeAdmin2.Startup))]
namespace FrescodeAdmin2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
