using DisasterAlleviationFoundations.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DisasterAlleviationFoundations.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<IncidentReport> IncidentReports { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<VolunteerTask> VolunteerTasks { get; set; }
    }
}
