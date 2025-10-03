using DisasterAlleviationFoundations.Data;

namespace DisasterAlleviationFoundations.Models
{
    public class IncidentReport
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime DateReported { get; set; } = DateTime.Now;

        // Link to user
        // Optional link to user
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}

