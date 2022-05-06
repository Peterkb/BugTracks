using BugTracksV3.Areas.Identity.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracksV3.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Company Name")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        [DisplayName("Company Description")]
        public string Description { get; set; } = string.Empty;

        //-- Avatar --//
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile? LogoFile { get; set; }

        [DisplayName("Company Logo")]
        public string?LogoFileName { get; set; }
        public byte[]? AvatarFileData { get; set; }

        [DisplayName("File Extension")]
        public string? LogoContentType { get; set; }

        //-- Navigation Properties --//
        public virtual ICollection<ApplicationUser> Members { get; set; } = new HashSet<ApplicationUser>();
        public virtual ICollection<Project> Projects { get; set; } = new HashSet<Project>();
        public virtual ICollection<Invite> Invites { get; set; } = new HashSet<Invite>();
    }
}
