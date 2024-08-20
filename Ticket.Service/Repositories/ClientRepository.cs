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

public class ClientRepository : IClientRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ClientRepository> _logger;
    private readonly IAttachmentService _attachmentService;

    public ClientRepository(ApplicationDbContext context, ILogger<ClientRepository> logger, IAttachmentService attachmentService)
    {
        _context = context;
        _logger = logger;
        _attachmentService = attachmentService; 
    }

    public IEnumerable<ClientWithTicketCountDTO> GetAllClients()
    {
        _logger.LogInformation("GetAllClients");
        try
        {
            var clients = _context.Users
                .Where(u => u.Role == Roles.Client)
                .GroupJoin(
                    _context.Tickets,
                    user => user.UserId,
                    ticket => ticket.CreatedBy,
                    (user, tickets) => new ClientWithTicketCountDTO
                    {
                        UserId = user.UserId,
                        FullName = user.FullName,
                        UserName = user.UserName,
                        MobileNumber = user.MobileNumber,
                        Email = user.Email,
                        DOB = user.DOB,
                        Address = user.Address,
                        FileName = user.Attachments.Select(a => a.FileName).FirstOrDefault(),
                        UserImage = user.Attachments.Select(a => a.Content).FirstOrDefault(),
                        IsActive = user.IsActive,
                        CreatedAt = user.CreatedAt,
                        UpdatedAt = user.UpdatedAt,
                        Role = user.Role,
                        TicketCount = tickets.Count()
                    })
                .ToList();

            return clients;

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in GetAllClients: {ex.Message}");
            throw;
        }
    }

    public ManagerDTO? GetClientById(int userId)
    {
        _logger.LogInformation($"GetClientById: Request Body {userId}");
        try
        {
            return _context.Users
                .Where(u => u.UserId == userId && u.Role == Roles.Client)
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
            _logger.LogError($"Error in GetClientById: {ex.Message}");
            throw;
        }
    }

    public void UpdateClient(EditManagerDTO managerDto)
    {
        _logger.LogInformation($"UpdateClient: Request Body {managerDto}");
        try
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == managerDto.UserId && u.Role == Roles.Client);
            if (user != null)
            {

                user.FullName = managerDto.FullName;
                user.UserName = managerDto.UserName;
                user.MobileNumber = managerDto.MobileNumber;
                user.Email = managerDto.Email;
                user.DOB = managerDto.DOB;
                user.Address = managerDto.Address;
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
                _logger.LogError($"Client with ID {managerDto.UserId} not found.");
                throw new KeyNotFoundException($"Client with ID {managerDto.UserId} not found.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in UpdateClient: {ex.Message}");
            throw;
        }
    }


    public void UpdateClientActivation(UpdateActivationDTO activationDto)
    {
        _logger.LogInformation($"UpdateClientActivation: Request Body {activationDto}");
        try
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == activationDto.UserId && u.Role == Roles.Client);
            if (user != null)
            {
                user.IsActive = activationDto.IsActive;

                _context.Users.Update(user);
                Save(); 
            }
            else
            {
                _logger.LogError($"Client with ID {activationDto.UserId} not found.");
                throw new KeyNotFoundException($"Client with ID {activationDto.UserId} not found.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in UpdateClientActivation: {ex.Message}");
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

