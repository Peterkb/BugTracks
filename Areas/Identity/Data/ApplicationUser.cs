using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BugTracksV3.Models;
using Microsoft.AspNetCore.Identity;

namespace BugTracksV3.Areas.Identity.Data;

public class ApplicationUser : IdentityUser
{
    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [NotMapped]
    [Display(Name = "Full Name")]
    public string FullName { get { return $"{FirstName} {LastName}"; } }

    //-- Avatar --//
    [NotMapped]
    [DataType(DataType.Upload)]
    public IFormFile? AvatarFormFile { get; set; }

    [DisplayName("Avatar")]
    public string? AvatarFileName { get; set; }
    public byte[]? AvatarFileData { get; set; }

    [DisplayName("File Extension")]
    public string? AvatarContentType { get; set; }

    //-- Foreign Keys --//
    public int CompanyId { get; set; }

    //-- Navigation Properties --//
    public virtual Company? Company { get; set; }
    //public virtual ICollection<Project> Projects { get; set; }
}

