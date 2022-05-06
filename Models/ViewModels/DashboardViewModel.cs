using BugTracksV3.Areas.Identity.Data;

namespace BugTracksV3.Models.ViewModels
{
    public class DashboardViewModel
    {
        public Company? Company { get; set; }

        public List<Project>? Projects { get; set; }

        public List<Ticket>? Tickets { get; set; }

        public List<ApplicationUser>? Members { get; set; }
    }
}