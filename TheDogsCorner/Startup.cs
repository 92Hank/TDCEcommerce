using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TheDogsCorner.Startup))]
namespace TheDogsCorner
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
