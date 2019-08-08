using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Owlvey.Falcon.Authority.Presentation.Areas.Identity.IdentityHostingStartup))]
namespace Owlvey.Falcon.Authority.Presentation.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}