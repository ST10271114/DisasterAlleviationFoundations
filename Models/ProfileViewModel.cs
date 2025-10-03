using DisasterAlleviationFoundations.Data;
using DisasterAlleviationFoundations.Models;
using System.Collections.Generic;

public class ProfileViewModel
{
    public ApplicationUser User { get; set; }
    public List<IncidentReport> IncidentReports { get; set; }
    public List<Donation> Donations { get; set; }
    public List<VolunteerTask> VolunteerTasks { get; set; }
}
