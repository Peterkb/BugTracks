using BugTracksV3.Areas.Identity.Data;
using System.ComponentModel;

namespace BugTracksV3.Models
{
    public class TicketComment
    {
        public int Id { get; set; }

        [DisplayName("User Comment")]
        public string Comment { get; set; } = string.Empty;

        [DisplayName("Date Created")]
        public DateTimeOffset DateCreated { get; set; }
        [DisplayName("Date Modified")]
        public DateTimeOffset DateModified { get; set; }

        //-- Foreign Keys --//
        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        [DisplayName("Team Member")]
        public string UserId { get; set; } = string.Empty;


        //-- Navigation Properties --//
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
