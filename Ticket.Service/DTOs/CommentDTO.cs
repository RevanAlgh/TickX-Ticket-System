using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Data.Models;
using Ticket.Service.Repositories;

namespace Ticket.Service.DTOs;

public class CommentDTO
{
    public int CommentId { get; set; }
    public int UserId { get; set; }
    public string FullName { get; set; }
    public int TicketId { get; set; }
    public string Replie { get; set; }
    public DateTime CreateAt { get; set; }
}

public class AddCommentDTO
{
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public string Replie { get; set; }

}
