    using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TeachMeBackendService.Startup))]

namespace TeachMeBackendService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}