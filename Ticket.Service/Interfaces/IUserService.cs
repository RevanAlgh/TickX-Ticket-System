using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Service.DTOs;
using Ticket.Data.Models;

namespace Ticket.Service.Interfaces
{
    public interface IUserService
    {
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UserNameExistsAsync(string fullName);
        Task<bool> MobileNumberExistsAsync(string mobileNumber);
        Task RegisterUserAsync(CreateUserDTO dto);
        Task<User> ValidateUserAsync(string nena, string password);
        public string GenerateJwtToken(User user);
        object GetUserResponse(User user);
        Task<User> GetUserByIdAsync(int userId);
        Task<bool> RequestPasswordResetAsync(PasswordResetRequestDTO request);
        Task<bool> ResetPasswordAsync(PasswordResetDTO resetDTO);
    }
}
