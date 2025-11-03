using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq;
using System.Security.Claims;
using DisasterAlleviationFoundations.Data;
using DisasterAlleviationFoundations.Models;
using Microsoft.AspNetCore.Identity;

namespace DisasterAlleviationFoundation.Tests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Replace DbContext with in-memory DB
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Remove existing UserManager
                var userManagerDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(UserManager<ApplicationUser>));
                if (userManagerDescriptor != null)
                    services.Remove(userManagerDescriptor);

                // Mock UserManager
                var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
                var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                    mockUserStore.Object, null, null, null, null, null, null, null, null);

                // Return null for unauthenticated users
                mockUserManager
                    .Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                    .ReturnsAsync((ApplicationUser?)null);

                services.AddSingleton(mockUserManager.Object);
            });
        }
    }
}
