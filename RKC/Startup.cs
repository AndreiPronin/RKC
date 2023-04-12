using Microsoft.Owin;
using Owin;
using System.IdentityModel.Claims;
using System.Net;
using System.Web.Helpers;

[assembly: OwinStartupAttribute(typeof(RKC.Startup))]
namespace RKC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            ConfigureAuth(app);
            //AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            //ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }
    }
}
