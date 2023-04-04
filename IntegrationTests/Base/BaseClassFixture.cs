using IntegrationTests.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using WebXmlImporter.Configuration.Test;
using Xunit;

namespace IntegrationTests.Base
{
    public class BaseClassFixture : IClassFixture<WebApplicationFactory<StartupTest>>
    {
        protected readonly HttpClient Client;
        public BaseClassFixture(WebApplicationFactory<StartupTest> factory)
        {
            Client = factory.SetupClient();
        }
    }
}
