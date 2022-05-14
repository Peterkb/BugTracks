using BugTracksV3.Areas.Identity.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BugTracksV3.Models
{
    public class Ticket
    {
        //Primary Key
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Title")]
        public string Title { get; set; } = string.Empty;

        [DisplayName("Description")]
        [StringLength(50)]
        public string Description { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayName("Date Created")]
        public DateTimeOffset DateCreated { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date Updated")]
        public DateTimeOffset? DateUpdated { get; set; }

        //Archive
        [DisplayName("Archived")]
        public bool IsArchived { get; set; }

        [DisplayName("Archived By Project")]
        public bool ArchivedByProject { get; set; }

        //-- Foreign Keys --//
        [DisplayName("Project")]
        public int ProjectId { get; set; }          //1

        [DisplayName("Ticket Type")]
        public int TicketTypeId { get; set; }       //2

        [DisplayName("Ticket Priority")]
        public int TicketPriorityId { get; set; }   //3

        [DisplayName("Ticket Status")]
        public int TicketStatusId { get; set; }     //4

        //String Ids - AppUsers
        [DisplayName("Ticket Owner")]
        public string? OwnerUserId { get; set; }     //5
        [DisplayName("Ticket Developer")]
        public string? DeveloperUserId { get; set; } //6

        //-- Navigation Properties --//
        public virtual Project? Project { get; set; }               //1
        public virtual TicketType? TicketType { get; set; }         //2
        public virtual TicketPriority? TicketPriority { get; set; } //3
        public virtual TicketStatus? TicketStatus { get; set; }     //4
        public virtual ApplicationUser? OwnerUser { get; set; }     //5
        public virtual ApplicationUser? DeveloperUser { get; set; } //6
        //Hash Sets
        public virtual ICollection<TicketComment> Comments { get; set; } = new HashSet<TicketComment>();
        public virtual ICollection<TicketAttachment> Attachments { get; set; } = new HashSet<TicketAttachment>();
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public virtual ICollection<TicketHistory> History { get; set; } = new HashSet<TicketHistory>();
    }
}
