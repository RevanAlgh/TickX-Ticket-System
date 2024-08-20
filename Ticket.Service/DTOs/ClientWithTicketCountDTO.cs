using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Data.Models.Enums;

namespace Ticket.Service.DTOs;

public class ClientWithTicketCountDTO
{
    public int UserId { get; set; }
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string MobileNumber { get; set; }
    public string Email { get; set; }
    public DateOnly DOB { get; set; }
    public string Address { get; set; }
    public string UserImage { get; set; }
    public string FileName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Roles Role { get; set; }
    public int TicketCount { get; set; }
}

public class UpdateActivationDTO
{
    public int UserId { get; set; }
    public bool IsActive { get; set; }
}
