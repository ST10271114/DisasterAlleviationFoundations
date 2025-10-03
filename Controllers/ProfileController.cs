using DisasterAlleviationFoundations.Data;
using DisasterAlleviationFoundations.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public ProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var model = new ProfileViewModel
        {
            User = user,
            IncidentReports = await _context.IncidentReports
                                .Where(i => i.UserId == user.Id)
                                .ToListAsync() ?? new List<IncidentReport>(),
            Donations = await _context.Donations
                                .Where(d => d.UserId == user.Id)
                                .ToListAsync() ?? new List<Donation>(),
            VolunteerTasks = await _context.VolunteerTasks
                                .Where(t => t.UserId == user.Id)
                                .ToListAsync() ?? new List<VolunteerTask>()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
    {
        if (!ModelState.IsValid) return View("Index", model);

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        user.FullName = model.User.FullName;
        user.Email = model.User.Email;
        user.UserName = model.User.Email;

        var result = await _userManager.UpdateAsync(user);

        TempData["Message"] = result.Succeeded
            ? "Profile updated successfully!"
            : "Failed to update profile.";

        return RedirectToAction("Index");
    }
}
