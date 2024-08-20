using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Ticket.Data.Models.Enums;

namespace Ticket.Data.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public int ProductId { get; set; }
        public string TicketDescription { get; set; }      
        public string Title { get; set; }
        //public string Attachment { get; set; }
        public Status status { get; set; }
        public DateTime? ClosedAt { get; set; } = null;
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; } = null;
        public int? ClosedBy { get; set; } = 0;
        public int? AssignedTo { get; set; } = 0;
        public Priorities? Priority { get; set; } = null;

        public int ReminderLevel { get; set; } = 0;
        public int? CreatedBy { get; set; }
        public Product Product { get; set; }
        public User? User { get; set; } 
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Attachment> Attachments { get; set; } = new List<Attachment>();

    }
}
