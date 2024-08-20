using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Data.Models.Enums;

namespace Ticket.Service.DTOs
{
    public class CreateUserAttachmentDTO
    {
        public int UserId { get; set; }
        public string? FileName { get; set; }
        public string? Base64File { get; set; }
    }
    public class CreateTicketAttachmentDTO
    {
        public int TicketId { get; set; }
        public string? FileName { get; set; }
        public string? Base64File { get; set; }
    }
}
