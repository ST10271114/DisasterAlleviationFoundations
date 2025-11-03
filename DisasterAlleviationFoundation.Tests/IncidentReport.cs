using DisasterAlleviationFoundations.Controllers;
using DisasterAlleviationFoundations.Data;
using DisasterAlleviationFoundations.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using Xunit;

public class IncidentReportsControllerTests
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly IncidentReportsController _controller;

    public IncidentReportsControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("IncidentReportsTestDb")
            .Options;

        _context = new ApplicationDbContext(options);

        _mockUserManager = MockUserManager();
        _controller = new IncidentReportsController(_context, _mockUserManager.Object);
    }

    private Mock<UserManager<ApplicationUser>> MockUserManager()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        return new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
    }


    [Fact]
    public async Task Create_ValidIncident_RedirectsToIndex()
    {
        // Arrange
        var user = new ApplicationUser { Id = "test-user" };
        _mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
            .ReturnsAsync(user);

        var incident = new IncidentReport
        {
            Title = "Flood",
            Description = "Severe flooding in Cape Town",
            Location = "Cape Town"
        };

        // Act
        var result = await _controller.Create(incident);

        // Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
    }
}
