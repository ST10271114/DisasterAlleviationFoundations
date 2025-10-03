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
    [Authorize] // 🔒 require login for ALL incident report actions
    public class IncidentReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IncidentReportsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: IncidentReports
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(await _context.IncidentReports
                .Where(i => i.UserId == user.Id) // ✅ only logged-in user’s reports
                .ToListAsync());
        }

        // GET: IncidentReports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var incidentReport = await _context.IncidentReports
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == user.Id);

            if (incidentReport == null) return NotFound();

            return View(incidentReport);
        }

        // GET: IncidentReports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IncidentReports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Location,DateReported")] IncidentReport incidentReport)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return RedirectToAction("Login", "Account");

                incidentReport.UserId = user.Id; // ✅ link to logged-in user
                _context.Add(incidentReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(incidentReport);
        }

        // GET: IncidentReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var incidentReport = await _context.IncidentReports
                .FirstOrDefaultAsync(i => i.Id == id && i.UserId == user.Id);

            if (incidentReport == null) return NotFound();

            return View(incidentReport);
        }

        // POST: IncidentReports/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Location,DateReported")] IncidentReport incidentReport)
        {
            if (id != incidentReport.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);

                    // ensure it belongs to the same user
                    var existingReport = await _context.IncidentReports.AsNoTracking()
                        .FirstOrDefaultAsync(i => i.Id == id && i.UserId == user.Id);

                    if (existingReport == null) return Unauthorized();

                    incidentReport.UserId = existingReport.UserId; // preserve ownership

                    _context.Update(incidentReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncidentReportExists(incidentReport.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(incidentReport);
        }

        // GET: IncidentReports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var incidentReport = await _context.IncidentReports
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == user.Id);

            if (incidentReport == null) return NotFound();

            return View(incidentReport);
        }

        // POST: IncidentReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var incidentReport = await _context.IncidentReports
                .FirstOrDefaultAsync(i => i.Id == id && i.UserId == user.Id);

            if (incidentReport != null)
            {
                _context.IncidentReports.Remove(incidentReport);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool IncidentReportExists(int id)
        {
            return _context.IncidentReports.Any(e => e.Id == id);
        }
    }
}
