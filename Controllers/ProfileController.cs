using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundations.Data;
using DisasterAlleviationFoundations.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace DisasterAlleviationFoundations.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            // Redirect unauthenticated users immediately
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Redirect("/Account/Login");
            }

            // Safe retrieval of authenticated user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Account/Login");
            }

            // Minimal database usage after authentication
            var model = new ProfileViewModel
            {
                User = user,
                IncidentReports = await _context.IncidentReports
                                    .Where(i => i.UserId == user.Id)
                                    .ToListAsync(),
                Donations = await _context.Donations
                                    .Where(d => d.UserId == user.Id)
                                    .ToListAsync(),
                VolunteerTasks = await _context.VolunteerTasks
                                    .Where(t => t.UserId == user.Id)
                                    .ToListAsync()
            };

            return View(model);
        }
    }
}