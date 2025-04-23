using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GFadda_Local_Theatre_Company.Startup))]
namespace GFadda_Local_Theatre_Company
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
