using Xunit;
using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundations.Controllers;
using DisasterAlleviationFoundations.Data;
using DisasterAlleviationFoundations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

public class DonationsControllerTests
{
    [Fact]
    public async Task CreateDonation_ReturnsRedirectToAction()
    {
        // Arrange: setup in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;

        var context = new ApplicationDbContext(options);
        var controller = new DonationsController(context);

        // ? Mock logged-in user
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
            new Claim(ClaimTypes.Name, "TestUser")
        }, "mock"));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Create a test donation
        var donation = new Donation
        {
            DonorName = "Tester",
            ResourceType = "Food",
            Quantity = 5
        };

        // Act
        var result = await controller.Create(donation);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);

        // Also confirm donation is stored
        Assert.Single(context.Donations);
    }
}
