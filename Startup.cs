using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BigJobbs.Startup))]
namespace BigJobbs
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
