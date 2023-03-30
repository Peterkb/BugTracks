﻿using System.ComponentModel;

namespace BugTracksV3.Models;

public class TicketStatus
{
    public int Id { get; set; }

    [DisplayName("Status Name")]
    public string Name { get; set; } = string.Empty;
}
