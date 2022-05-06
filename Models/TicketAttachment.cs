using BugTracksV3.Areas.Identity.Data;
using BugTracksV3.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracksV3.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }

        

        [DisplayName("File Date")]
        public DateTimeOffset Created { get; set; }

        //Image file
        [DisplayName("File Description")]
        public string Description { get; set; } = string.Empty;
        [NotMapped]

        //Attachment
        [DisplayName("Select a file")]
        [DataType(DataType.Upload)]
        [MaxFileSize(1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", "gif", ".doc", ".docx", ".xls", ".xlsx", ".pdf", ".ppt", ".pptx" })]
        public IFormFile? FormFile { get; set; }

        [DisplayName("File Name")]
        public string? FileName { get; set; }

        public byte[]? FileData { get; set; }

        [DisplayName("File Extensions")]
        public string FileContentType { get; set; } = string.Empty;

        //-- Foreign Keys --//
        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        [DisplayName("Team Member")]
        public string UserId { get; set; } = string.Empty;

        //-- Navigation Properties --//
        public virtual Ticket? Ticket { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
