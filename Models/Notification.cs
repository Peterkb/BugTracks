using BugTracksV3.Areas.Identity.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BugTracksV3.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Ticket")]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [DisplayName("Message")]
        public string Message { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date")]
        public DateTimeOffset DateCreated { get; set; }

        

        [DisplayName("Read")]
        public bool IsRead { get; set; }

        //-- Foreign Keys --//

        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        //String Ids
        [Required]
        [DisplayName("Recipient")]
        public string RecipientId { get; set; } //GUID

        [Required]
        [DisplayName("Sender")]
        public string SenderId { get; set; }    //GUID


        //-- Navigation Properties --//
        public virtual Ticket Ticket { get; set; }     //1
        public virtual ApplicationUser? Recipient { get; set; }  //2
        public virtual ApplicationUser? Sender { get; set; }     //3
    }
}
