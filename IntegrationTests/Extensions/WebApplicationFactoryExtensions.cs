using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using WebXmlImporter.Configuration.Test;

namespace IntegrationTests.Extensions
{
    public static class WebApplicationFactoryExtensions
    {
        public static HttpClient SetupClient(this WebApplicationFactory<StartupTest> fixture)
        {
            var options = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            };

            return fixture.WithWebHostBuilder(
                builder => builder
                    .UseStartup<StartupTest>()
                    .ConfigureTestServices(services => { })
            ).CreateClient(options);
        }
    }
}
