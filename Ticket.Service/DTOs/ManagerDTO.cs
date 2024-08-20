using System;
using Ticket.Data.Models.Enums;

namespace Ticket.Service.Models;

//Get
public class ManagerDTO
{
    public int UserId { get; set; }
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string MobileNumber { get; set; }
    public string Email { get; set; }
    public DateOnly DOB { get; set; }
    public string Address { get; set; }
    public string? UserImage { get; set; } = null;
    public string FileName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Roles Role { get; set; }
}

//Put
public class EditManagerDTO
{
    public int UserId { get; set; }
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string MobileNumber { get; set; }
    public string Email { get; set; }
    public DateOnly DOB { get; set; }
    public string Address { get; set; }
    public string? UserImage { get; set; } = null;
    public string? FileName { get; set; } = null;
    public bool IsActive { get; set; }
    public Roles Role { get; set; }
}

