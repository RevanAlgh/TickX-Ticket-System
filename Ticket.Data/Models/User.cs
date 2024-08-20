using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Ticket.Data.Models.Enums;

namespace Ticket.Data.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public DateOnly DOB { get; set; }
        public string Address { get; set; }
        public string? UserImage { get; set; }
        //add encrypted field
        public string Password { get; set; }
        public bool IsActive { get; set; } = true;
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Roles Role { get; set; }

        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpiration { get; set; }

        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Attachment> Attachments { get; set; } = new List<Attachment>();


    }
}
