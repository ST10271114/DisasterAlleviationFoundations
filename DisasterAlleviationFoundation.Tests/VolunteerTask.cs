using System;
using System.Threading.Tasks;
using DisasterAlleviationFoundations.Controllers;
using DisasterAlleviationFoundations.Data;
using DisasterAlleviationFoundations.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DisasterAlleviationFoundation.Tests
{
    public class VolunteerTasksControllerTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Create_ValidTask_RedirectsToIndex()
        {
            // Arrange
            var dbContext = GetDbContext();

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
            );

            var user = new ApplicationUser { Id = "user123", UserName = "testuser@example.com" };
            userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                           .ReturnsAsync(user);

            var controller = new VolunteerTasksController(dbContext, userManagerMock.Object);

            var task = new VolunteerTask
            {
                TaskName = "Clean-up Drive",
                Description = "Help clean the community area",
                DueDate = DateTime.Now.AddDays(3),
                AssignedVolunteer = "Test Volunteer" // ✅ required property
            };

            // Act
            var result = await controller.Create(task);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }
    }
}
