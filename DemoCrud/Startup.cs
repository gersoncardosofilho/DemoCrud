using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DemoCrud.Startup))]
namespace DemoCrud
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
