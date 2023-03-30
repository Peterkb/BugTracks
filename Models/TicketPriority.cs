using System.ComponentModel;

namespace BugTracksV3.Models;

public class TicketPriority
{
    public int Id { get; set; }

    [DisplayName("Priority Name")]
    public string Name { get; set; } = string.Empty;
}