using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Data.Models.Enums;

namespace Ticket.Data.Models
{
    public class Attachment
    {
        [Key]
        public int AttachmentId { get; set; }
        public int? TicketId { get; set; }
        public int? UserId { get; set; }
        public string? FileName { get; set; }
        public Types Type { get; set; }

        [Required]
        public string FilePath { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? Content { get; set; }
        public Ticket Ticket { get; set; }
        public User User { get; set; }
    }
}
