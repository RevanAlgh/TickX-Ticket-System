using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Data.Models.Enums;

namespace Ticket.Service.DTOs
{
    public class CreateUserDTO
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public DateOnly DOB { get; set; }
        public string Address { get; set; }
        public string? UserImage { get; set; }
        public string? FileName { get; set; }
        public string? Password { get; set; }
        public Roles Role { get; set; }
    }
    public class LoginUserDTO
    {
        //name, email, or number and if is active
        public string NENA { get; set; }
        public string Password { get; set; }

    }
    public class PasswordResetRequestDTO
    {
        public string Email { get; set; }
    }
    public class PasswordResetDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
