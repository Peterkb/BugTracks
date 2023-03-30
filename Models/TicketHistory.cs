using BugTracksV3.Areas.Identity.Data;
using System.ComponentModel;

namespace BugTracksV3.Models;

public class TicketHistory
{
    public int Id { get; set; }


    [DisplayName("Updated Item")]
    public string Property { get; set; } = string.Empty;

    [DisplayName("Previous")]
    public string OldValue { get; set; } = string.Empty;

    [DisplayName("Current")]
    public string NewValue { get; set; } = string.Empty;

    [DisplayName("Date Modified")]
    public DateTimeOffset DateUpdated { get; set; }

    [DisplayName("Description of Change")]
    public string Description { get; set; } = string.Empty;

    //-- Foreign Keys --//
    [DisplayName("Ticket")]
    public int TicketId { get; set; }

    [DisplayName("Team member")]
    public string UserId { get; set; } = string.Empty;

    //-- Navigation Properties --//
    public virtual Ticket? Ticket { get; set; }
    public virtual ApplicationUser? User { get; set; }
}
