using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket.Data.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public int TicketId { get; set; }
        public string Replie { get; set; }
        public DateTime CreateAt { get; set; }
        public Ticket Ticket { get; set; } 
        public User User { get; set; }

    }
}
