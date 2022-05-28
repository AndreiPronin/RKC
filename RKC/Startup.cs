using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RKC.Startup))]
namespace RKC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
