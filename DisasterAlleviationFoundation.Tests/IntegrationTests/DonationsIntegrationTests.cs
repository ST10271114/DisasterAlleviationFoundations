using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DisasterAlleviationFoundation.Tests.IntegrationTests
{
    public class DonationsIntegrationTests : IClassFixture<CustomWebApplicationFactory<DisasterAlleviationFoundations.Program>>
    {
        private readonly HttpClient _client;

        public DonationsIntegrationTests(CustomWebApplicationFactory<DisasterAlleviationFoundations.Program> factory)
        {
            // Disable auto-redirect to capture 302 redirect responses
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Get_Donations_Index_RedirectsToLogin_WhenNotAuthenticated()
        {
            var response = await _client.GetAsync("/Donations");

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("/Account/Login", response.Headers.Location!.ToString());
        }

        [Fact]
        public async Task Get_Donations_Create_RedirectsToLogin_WhenNotAuthenticated()
        {
            var response = await _client.GetAsync("/Donations/Create");

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("/Account/Login", response.Headers.Location!.ToString());
        }
    }
}
