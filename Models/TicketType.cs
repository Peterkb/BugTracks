using System.ComponentModel;

namespace BugTracksV3.Models;

public class TicketType
{
    public int Id { get; set; }

    [DisplayName("Type Name")]
    public string Name { get; set; } = string.Empty;
}