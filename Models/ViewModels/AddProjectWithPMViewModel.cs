using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugTracksV3.Models.ViewModels
{
    public class AddProjectWithPMViewModel
    {
        public Project? Project { get; set; }

        public SelectList? PmList { get; set; }

        public string PmId { get; set; } = string.Empty;

        public SelectList? PriorityList { get; set; }
    }
}