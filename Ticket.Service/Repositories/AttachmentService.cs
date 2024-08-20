using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Ticket.Service.DTOs;
using Ticket.Service.Interfaces;
using Ticket.Data.Models;
using Ticket.Data.Models.Data;
using Ticket.Data.Models.Enums;
using Microsoft.Extensions.Logging;

namespace Ticket.Service.Repositories
{
    public class AttachmentService : IAttachmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Attachment> _logger;
        public AttachmentService(ApplicationDbContext context, ILogger<Attachment> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public string UploadUserProfilePicture(CreateUserAttachmentDTO dto)
        {
            _logger.LogInformation($"UploadUserProfilePicture: Request Body {dto}");
            if (!_context.Users.Any(u => u.UserId == dto.UserId))
            {
                _logger.LogError("UserId does not exist.");
                throw new Exception("UserId does not exist.");
            }

            ValidateFile(dto.FileName, dto.Base64File);

            return SaveAttachment(dto.UserId, null, dto.FileName, dto.Base64File);
        }

        public string UploadTicketAttachment(CreateTicketAttachmentDTO dto)
        {
            _logger.LogInformation($"CreateTicketAttachmentDTO: Request Body {dto}");
            if (!_context.Tickets.Any(t => t.TicketId == dto.TicketId))
            {
                _logger.LogError("TicketId does not exist.");
                throw new Exception("TicketId does not exist.");
            }

            ValidateFile(dto.FileName, dto.Base64File);

            return SaveAttachment(null, dto.TicketId, dto.FileName, dto.Base64File);
        }

        public void ValidateFile(string fileName, string base64File)
        {
            _logger.LogInformation($"ValidateFile: Request Body {fileName}, {base64File}");
            List<string> validExtensions = new List<string> { ".jpeg", ".png", ".pdf", ".jpg" };
            string extension = Path.GetExtension(fileName)?.ToLower();
            if (string.IsNullOrEmpty(extension) || !validExtensions.Contains(extension))
            {
                _logger.LogError($"Extension is not valid ({string.Join(',', validExtensions)})");
                throw new Exception($"Extension is not valid ({string.Join(',', validExtensions)})");
            }

            byte[] fileBytes;
            try
            {
                fileBytes = Convert.FromBase64String(base64File);
            }
            catch (FormatException)
            {
                _logger.LogError("Invalid Base64 string.");
                throw new Exception("Invalid Base64 string.");
            }

            long size = fileBytes.Length;
            if (size > (2 * 1024 * 1024))
            {
                _logger.LogError("Maximum size is 2MB.");
                throw new Exception("Maximum size is 2MB.");
            }
        }

        private string SaveAttachment(int? userId, int? ticketId, string fileName, string base64File)
        {
            _logger.LogInformation($"SaveAttachment: Request Body {userId}, {ticketId}, {fileName}, {base64File}");
            byte[] fileBytes = Convert.FromBase64String(base64File);
            string extension = Path.GetExtension(fileName).ToLower();
            string generatedFileName = Guid.NewGuid().ToString() + extension;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Attachments");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fullPath = Path.Combine(path, generatedFileName);

            File.WriteAllBytes(fullPath, fileBytes);

            Types type;
            switch (extension)
            {
                case ".jpeg":
                    type = Types.JPEG;
                    break;
                case ".png":
                    type = Types.PNG;
                    break;
                case ".pdf":
                    type = Types.PDF;
                    break;
                case ".jpg":
                    type = Types.JPG;
                    break;
                default:
                    type = Types.Other;
                    break;
            }

            var attachment = new Attachment
            {
                UserId = userId,
                TicketId = ticketId,
                FileName = generatedFileName,
                FilePath = fullPath,
                CreatedAt = DateTime.Now,
                Content = base64File,
                Type = type
            };

            _context.Attachments.Add(attachment);
            _context.SaveChanges();

            return generatedFileName;
        }
    }
}
