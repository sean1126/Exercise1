using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Exercise1.Startup))]
namespace Exercise1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
