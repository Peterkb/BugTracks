using BugTracksV3.Areas.Identity.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracksV3.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Project Name")]
        public string Name { get; set; } = string.Empty;

        [DisplayName("Description")]
        public string Description { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        public DateTimeOffset StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        public DateTimeOffset EndDate { get; set; }

        //-- Image Keys --//
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile? ImageFormFile { get; set; }
        [DisplayName("File Name")]
        public string? ImageFileName { get; set; }
        public byte[]? ImageFileData { get; set; }
        [DisplayName("File Extension")]
        public string? ImageContentType { get; set; }

        [DisplayName("Archived")]
        public bool IsArchived { get; set; }

        //-- Foreign Keys --//

        [DisplayName("Company")]
        public int? CompanyId { get; set; } //1

        [DisplayName("Priority")]
        public int? ProjectPriorityId { get; set; }

        //-- Navigation Properties --//
        public virtual Company? Company { get; set; }
        public virtual ProjectPriority? ProjectPriority { get; set; }

        public virtual ICollection<ApplicationUser> Members { get; set; } = new HashSet<ApplicationUser>();
        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
