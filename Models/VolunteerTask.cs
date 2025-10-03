using System;
using DisasterAlleviationFoundations.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace DisasterAlleviationFoundations.Models
{
    public class VolunteerTask
    {
        public int Id { get; set; }

        public string TaskName { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public string AssignedVolunteer { get; set; }

        // Not required on the form, set by controller
        [ForeignKey("User")]
        public string? UserId { get; set; }  // <-- make nullable

        public ApplicationUser? User { get; set; } // <-- make nullable
    }
}
