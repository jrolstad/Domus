using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(domus.mvc.Startup))]
namespace domus.mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
