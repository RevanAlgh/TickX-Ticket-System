using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Data.Models.Data;
using Ticket.Service.DTOs;
using Ticket.Service.Interfaces;

namespace Ticket.Service.Repositories;

public class ViewTicketRepository : ITicketRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ViewTicketRepository> _logger;

    public ViewTicketRepository(ApplicationDbContext context, ILogger<ViewTicketRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public TicketWithRepliesDTO GetTicketWithReplies(int ticketId)
    {
        _logger.LogInformation($"GetTicketWithReplies: Request Body {ticketId}");
        try
        {
            var ticket = _context.Tickets
                .Include(t => t.Attachments)
                .Include(t => t.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefault(t => t.TicketId == ticketId);

            if (ticket == null)
            {
                _logger.LogError($"Ticket with ID {ticketId} not found.");
                throw new KeyNotFoundException($"Ticket with ID {ticketId} not found.");
            }

            return new TicketWithRepliesDTO
            {
                Title = ticket.Title,
                Description = ticket.TicketDescription,
                Attachments = ticket.Attachments.Select(a => new AttachmentDTO
                {
                    FileName = a.FileName,
                    FilePath = a.FilePath
                }).ToList(),
                Replies = ticket.Comments.Select(c => new ReplyDTO
                {
                    UserId = c.UserId,
                    Content = c.Replie,
                    UserImage = c.User.UserImage 
                }).ToList()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in GetTicketWithReplies: {ex.Message}");
            throw;
        }
    }
}
