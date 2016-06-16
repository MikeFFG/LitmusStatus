using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LitmusStatus.Startup))]
namespace LitmusStatus
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
