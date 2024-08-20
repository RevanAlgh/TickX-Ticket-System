using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ticket.Service.DTOs;
using Ticket.Service.Interfaces;
using Ticket.Data.Models;
using Ticket.Data.Models.Data;
using Ticket.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Ticket.Service.Utility;
using Microsoft.Extensions.Logging;
using System.Web;

namespace Ticket.Service.Repositories
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAttachmentService _attachmentService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ILogger<UserService> _logger;

        public UserService(ApplicationDbContext context, IConfiguration configuration, IAttachmentService attachmentService, IEmailService emailService, ILogger<UserService> logger)
        {
            _context = context;
            _configuration = configuration;
            _attachmentService = attachmentService;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            _logger.LogInformation($"EmailExistsAsync: Request Body {email}");
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> UserNameExistsAsync(string userName)
        {
            _logger.LogInformation($"UserNameExistsAsync: Request Body {userName}");
            return await _context.Users.AnyAsync(u => u.UserName == userName);
        }

        public async Task<bool> MobileNumberExistsAsync(string mobileNumber)
        {
            _logger.LogInformation($"MobileNumberExistsAsync: Request Body {mobileNumber}");
            return await _context.Users.AnyAsync(u => u.MobileNumber == mobileNumber);
        }

        public async Task RegisterUserAsync(CreateUserDTO dto)
        {
            _logger.LogInformation($"RegisterUserAsync: Request Body {dto}");
            if (string.IsNullOrEmpty(dto.UserName) || dto.UserName.Contains(" "))
            {
                _logger.LogError("Username cannot be null or contain spaces.");
                throw new Exception("Username cannot be null or contain spaces.");
            }

            if (dto.Role == Roles.Client && string.IsNullOrEmpty(dto.Password))
            {
                _logger.LogError("Password is required for clients.");
                throw new Exception("Password is required for clients.");
            }

            if (string.IsNullOrEmpty(dto.Password) && dto.Role == Roles.TeamMember)
            {
                var defaultEmployeePassword = _configuration.GetSection("DefaultPassword:TeamMember").Value;
                if (string.IsNullOrEmpty(defaultEmployeePassword))
                {
                    _logger.LogError("Default password for employee is not set in configuration.");
                    throw new Exception("Default password for employee is not set in configuration.");
                }
                dto.Password = defaultEmployeePassword;
            }

            var hashedPassword = PasswordHasher.HashPassword(dto.Password);


            if (string.IsNullOrEmpty(dto.UserImage) != string.IsNullOrEmpty(dto.FileName))
            {
                _logger.LogError("Both Image and File Name must be provided together");
                throw new Exception("Both Image and File Name must be provided together");
            }

            if (string.IsNullOrEmpty(dto.UserImage) || string.IsNullOrEmpty(dto.FileName))
            {
                string defaultFileName = "DefaultUserImage.jpg";
                string defaultFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Attachments", defaultFileName);

                if (!File.Exists(defaultFilePath))
                {
                    _logger.LogError("Default image not found.");
                    throw new Exception("Default image not found.");
                }

                dto.UserImage = Convert.ToBase64String(File.ReadAllBytes(defaultFilePath));
                dto.FileName = defaultFileName;
            }
            else
            {
                try
                {
                    _attachmentService.ValidateFile(dto.FileName, dto.UserImage);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Image validation failed: " + ex.Message);
                    throw new Exception("Image validation failed: " + ex.Message);
                }
            }


            var user = new User
            {
                FullName = dto.FullName,
                UserName = dto.UserName,
                MobileNumber = dto.MobileNumber,
                Email = dto.Email,
                DOB = dto.DOB,
                Address = dto.Address,
                Password = hashedPassword,
                //required  
                Token = string.Empty,
                Role = dto.Role,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(dto.UserImage) && !string.IsNullOrEmpty(dto.FileName))
            {
                var createUserAttachmentDTO = new CreateUserAttachmentDTO
                {
                    UserId = user.UserId,
                    FileName = dto.FileName,
                    Base64File = dto.UserImage
                };

                try
                {
                    _attachmentService.UploadUserProfilePicture(createUserAttachmentDTO);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    _logger.LogError("Failed to upload image: " + ex.Message);
                    throw new Exception("Failed to upload image: " + ex.Message);
                }
            }
            if (dto.Role == Roles.Client)
            {
                var emailDTO = new EmailDTO
                {
                    Email = user.Email,
                    Subject = "Welcome",
                    Message = "Thank you for registering in our ticket system!"
                };
                await _emailService.SendEmail(emailDTO);
            }
            if (dto.Role == Roles.TeamMember)
            {
                var emailDTO = new EmailDTO
                {
                    Email = user.Email,
                    Subject = "Welcome",
                    Message = $"You have been registered successfully!\nUser Name: {user.UserName}\nPassword: {dto.Password}"
                };
                await _emailService.SendEmail(emailDTO);
            }
        }

        public async Task<User> ValidateUserAsync(string nena, string password)
        {
            _logger.LogInformation($"ValidateUserAsync: Request Body {nena}, {password}");
            var user = await _context.Users
                .Include(u => u.Attachments)
                .SingleOrDefaultAsync(u =>
                (u.Email == nena || u.UserName == nena || u.MobileNumber == nena) && u.IsActive);

            if (user == null || !PasswordHasher.VerifyPassword(password, user.Password))
            {
                return null;
            }

            return user;
        }

        public string GenerateJwtToken(User user)
        {
            _logger.LogInformation($"GenerateJwtToken: Request Body {user}");
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiresInMinutes"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public object GetUserResponse(User user)
        {
            _logger.LogInformation($"GetUserResponse: Request Body {user}");
            var userAttachments = user.Attachments
                .Where(a => a.UserId == user.UserId)
                .Select(a => new
                {
                    a.FileName,
                    a.Content,
                })
                .FirstOrDefault();

            return new
            {
                UserId = user.UserId,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                Address = user.Address,
                DOB = user.DOB,
                Role = user.Role,
                fileName = userAttachments?.FileName,
                userImage = userAttachments?.Content
            };
        }
        public async Task<User> GetUserByIdAsync(int userId)
        {
            _logger.LogInformation($"ResetPasswordRepo: Request Body {userId}");
            return await _context.Users
                .SingleOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<bool> RequestPasswordResetAsync(PasswordResetRequestDTO request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                return false;

            var token = Guid.NewGuid().ToString();

            user.ResetPasswordToken = token;
            user.ResetPasswordTokenExpiration = DateTime.Now.AddHours(1);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            var baseUrl = _configuration["ResetPassword:BaseUrl"];
            var encodedEmail = HttpUtility.UrlEncode(request.Email);
            var encodedToken = HttpUtility.UrlEncode(token);
            var resetLink = $"{baseUrl}?email={encodedEmail}&token={encodedToken}";

            var emailDTO = new EmailDTO
            {
                Email = user.Email,
                Subject = "Reset Password",
                Message = $"Please click the following link to reset your password: {resetLink}"
            };
            await _emailService.SendEmail(emailDTO);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(PasswordResetDTO resetDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == resetDTO.Email);
            if (user == null || user.ResetPasswordToken != resetDTO.Token || user.ResetPasswordTokenExpiration <= DateTime.Now)
            {
                return false;
            }

            user.Password = PasswordHasher.HashPassword(resetDTO.NewPassword);
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiration = null;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
