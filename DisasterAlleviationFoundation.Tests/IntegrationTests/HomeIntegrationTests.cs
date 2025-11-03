using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DisasterAlleviationFoundation.Tests.IntegrationTests
{
    public class HomeIntegrationTests : IClassFixture<CustomWebApplicationFactory<DisasterAlleviationFoundations.Program>>
    {
        private readonly HttpClient _client;

        public HomeIntegrationTests(CustomWebApplicationFactory<DisasterAlleviationFoundations.Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Home_Index_ReturnsSuccess()
        {
            var response = await _client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Get_Home_Privacy_ReturnsSuccess()
        {
            var response = await _client.GetAsync("/Home/Privacy");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
