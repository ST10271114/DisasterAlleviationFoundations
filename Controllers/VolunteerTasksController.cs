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
    [Authorize] // ensure only logged-in users can access volunteer tasks
    public class VolunteerTasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VolunteerTasksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: VolunteerTasks
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            return View(await _context.VolunteerTasks
                .Where(t => t.UserId == user.Id) // only show current user's tasks
                .Include(t => t.User)
                .ToListAsync());
        }

        // GET: VolunteerTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.VolunteerTasks
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (task == null) return NotFound();

            return View(task);
        }

        // GET: VolunteerTasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VolunteerTasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TaskName,Description,DueDate,AssignedVolunteer")] VolunteerTask volunteerTask)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return RedirectToAction("Login", "Account");

                volunteerTask.UserId = user.Id; // link to logged-in user
                _context.Add(volunteerTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(volunteerTask);
        }

        // GET: VolunteerTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.VolunteerTasks.FindAsync(id);
            if (task == null) return NotFound();

            return View(task);
        }

        // POST: VolunteerTasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TaskName,Description,DueDate,AssignedVolunteer")] VolunteerTask volunteerTask)
        {
            if (id != volunteerTask.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // keep the original user who created the task
                    var existingTask = await _context.VolunteerTasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
                    volunteerTask.UserId = existingTask.UserId;

                    _context.Update(volunteerTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerTaskExists(volunteerTask.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(volunteerTask);
        }

        // GET: VolunteerTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.VolunteerTasks
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (task == null) return NotFound();

            return View(task);
        }

        // POST: VolunteerTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.VolunteerTasks.FindAsync(id);
            if (task != null)
            {
                _context.VolunteerTasks.Remove(task);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool VolunteerTaskExists(int id)
        {
            return _context.VolunteerTasks.Any(e => e.Id == id);
        }
    }
}
