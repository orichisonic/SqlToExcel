using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BulksqltoExcel.Startup))]
namespace BulksqltoExcel
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
