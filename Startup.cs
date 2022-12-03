using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(sklepMVCv2.Startup))]
namespace sklepMVCv2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
