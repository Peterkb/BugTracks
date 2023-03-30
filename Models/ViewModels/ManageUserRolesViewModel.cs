using BugTracksV3.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugTracksV3.Models.ViewModels;

public class ManageUserRolesViewModel
{
    public ApplicationUser? AppUser { get; set; }

    public MultiSelectList? Roles { get; set; }

    public List<string>? SelectedRoles { get; set; }
}