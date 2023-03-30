using BugTracksV3.Areas.Identity.Data;
using System.ComponentModel;

namespace BugTracksV3.Models;

public class Invite
{
    public int Id { get; set; }

    [DisplayName("Date sent")]
    public DateTimeOffset InviteDate { get; set; }

    [DisplayName("Join Date")]
    public DateTimeOffset JoinDate { get; set; }

    [DisplayName("Code")]
    public Guid CompanyToken { get; set; }


    [DisplayName("Invitee Email")]
    public string InviteeEmail { get; set; } = string.Empty;
    [DisplayName("Invitee First Name")]
    public string InviteeFirstName { get; set; } = string.Empty;
    [DisplayName("Invitee Last Name")]
    public string InviteeLastName { get; set; } = string.Empty;

    public bool IsValid { get; set; }

    //-- Foreign Keys Properties --//
    [DisplayName("Company")]
    public int CompanyId { get; set; }
    [DisplayName("Project")]
    public int ProjectId { get; set; }
    [DisplayName("Invitor")]
    public string InvitorId { get; set; } = string.Empty;
    [DisplayName("Invitee")]
    public string InviteeId { get; set; } = string.Empty;

    //-- Navigation Properties --//
    public virtual Company? Company { get; set; }
    public virtual Project? Project { get; set; }
    public virtual ApplicationUser? Invitor { get; set; }
    public virtual ApplicationUser? Invitee { get; set; }
}
