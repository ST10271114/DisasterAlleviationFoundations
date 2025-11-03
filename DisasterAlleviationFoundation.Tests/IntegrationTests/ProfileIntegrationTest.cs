using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DisasterAlleviationFoundation.Tests.IntegrationTests
{
    public class ProfileIntegrationTests : IClassFixture<CustomWebApplicationFactory<DisasterAlleviationFoundations.Program>>
    {
        private readonly HttpClient _client;

        public ProfileIntegrationTests(CustomWebApplicationFactory<DisasterAlleviationFoundations.Program> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Get_Profile_Index_RedirectsToLogin_WhenNotAuthenticated()
        {
            var response = await _client.GetAsync("/Profile");

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("/Account/Login", response.Headers.Location!.ToString());
        }
    }
}
