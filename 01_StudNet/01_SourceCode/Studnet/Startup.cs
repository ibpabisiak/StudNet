using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Studnet.Startup))]
namespace Studnet
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

        }
    }
}
