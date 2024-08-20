using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Data.Models;
using Ticket.Data.Models.Data;
using Ticket.Data.Models.Enums;
using Ticket.Service.DTOs;
using Ticket.Service.Interfaces;
using Ticket.Service.Models;

namespace Ticket.Service.Repositories;

public class TeamMemberRepository : IViewTeamMemberRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TeamMemberRepository> _logger;
    private readonly IAttachmentService _attachmentService;

    public TeamMemberRepository(ApplicationDbContext context, ILogger<TeamMemberRepository> logger, IAttachmentService attachmentService)
    {
        _context = context;
        _logger = logger;
        _attachmentService = attachmentService;
    }

    public IEnumerable<ManagerDTO> GetAllTeamMembers()
    {
        _logger.LogInformation("GetAllTeamMembers");
        try
        {
            var result =  _context.Users
                .Where(u => u.Role == Roles.TeamMember)
                .Select(u => new ManagerDTO
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    UserName = u.UserName,
                    MobileNumber = u.MobileNumber,
                    Email = u.Email,
                    DOB = u.DOB,
                    Address = u.Address,
                    FileName = u.Attachments.Select(a => a.FileName).FirstOrDefault(),
                    UserImage = u.Attachments.Select(a => a.Content).FirstOrDefault(),
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt,
                    Role = u.Role
                }).ToList();


            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in GetAllTeamMembers: {ex.Message}");
            throw;
        }
    }

    public IEnumerable<ManagerDTO> GetActiveTeamMembers()
    {
        _logger.LogInformation("GetActiveTeamMembers");
        try
        {
            return _context.Users
                .Where(u => u.Role == Roles.TeamMember && u.IsActive)
                .Select(u => new ManagerDTO
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    UserName = u.UserName,
                    MobileNumber = u.MobileNumber,
                    Email = u.Email,
                    DOB = u.DOB,
                    Address = u.Address,
                    FileName = u.Attachments.Select(a => a.FileName).FirstOrDefault(),
                    UserImage = u.Attachments.Select(a => a.Content).FirstOrDefault(),
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt,
                    Role = u.Role
                }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in GetActiveTeamMembers: {ex.Message}");
            throw;
        }
    }

    public ManagerDTO? GetTeamMemberById(int userId)
    {
        _logger.LogInformation($"GetTeamMemberById: Request Body {userId}");
        try
        {
            return _context.Users
                .Where(u => u.UserId == userId && u.Role == Roles.TeamMember)
                .Select(u => new ManagerDTO
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    UserName = u.UserName,
                    MobileNumber = u.MobileNumber,
                    Email = u.Email,
                    DOB = u.DOB,
                    Address = u.Address,
                    FileName = u.Attachments.Select(a => a.FileName).FirstOrDefault(),
                    UserImage = u.Attachments.Select(a => a.Content).FirstOrDefault(),
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt,
                    Role = u.Role
                }).FirstOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in GetTeamMemberById: {ex.Message}");
            throw;
        }
    }

    public void UpdateTeamMember(EditManagerDTO managerDto)
    {
        _logger.LogInformation($"UpdateTeamMember: Request Body {managerDto}");
        try
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == managerDto.UserId && u.Role == Roles.TeamMember);
            if (user != null)
            {
                user.FullName = managerDto.FullName;
                user.UserName = managerDto.UserName;
                user.MobileNumber = managerDto.MobileNumber;
                user.Email = managerDto.Email;
                user.DOB = managerDto.DOB;
                user.Address = managerDto.Address;
                user.UserImage = managerDto.UserImage;
                user.IsActive = managerDto.IsActive;
                user.Role = managerDto.Role;

                if (!string.IsNullOrEmpty(managerDto.UserImage) && !string.IsNullOrEmpty(managerDto.FileName))
                {
                    var existingAttachment = _context.Attachments.FirstOrDefault(a => a.UserId == managerDto.UserId);
                    if (existingAttachment != null)
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "Attachments", existingAttachment.FileName);
                        if (File.Exists(oldImagePath))
                        {
                            File.Delete(oldImagePath);
                        }

                        _context.Attachments.Remove(existingAttachment);
                    }

                    var newImageFileName = _attachmentService.UploadUserProfilePicture(new CreateUserAttachmentDTO
                    {
                        UserId = managerDto.UserId,
                        FileName = managerDto.FileName,
                        Base64File = managerDto.UserImage
                    });

                }

                _context.Users.Update(user);
                Save();
            }
            else
            {
                _logger.LogError($"Team member with ID {managerDto.UserId} not found.");
                throw new KeyNotFoundException($"Team member with ID {managerDto.UserId} not found.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in UpdateTeamMember: {ex.Message}");
            throw;
        }
    }


    public void UpdateTeamMemberActivation(UpdateActivationDTO activationDto)
    {
        _logger.LogInformation($"UpdateTeamMember: Request Body {activationDto}");
        try
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == activationDto.UserId && u.Role == Roles.TeamMember);
            if (user != null)
            {
                user.IsActive = activationDto.IsActive;

                _context.Users.Update(user);
                Save();
            }
            else
            {
                _logger.LogError($"Team member with ID {activationDto.UserId} not found.");
                throw new KeyNotFoundException($"Team member with ID {activationDto.UserId} not found.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in UpdateTeamMemberActivation: {ex.Message}");
            throw;
        }
    }

    public void Save()
    {
        _logger.LogInformation("Save");
        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in Save: {ex.Message}");
            throw;
        }
    }
}
