using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Data.Models.Data;
using Ticket.Data.Models;
using Ticket.Service.DTOs;
using Ticket.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ticket.Service.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CommentRepository> _logger;

    public CommentRepository(ApplicationDbContext context, ILogger<CommentRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IEnumerable<CommentDTO> GetCommentByTicketId(int ticketId)
    {
        _logger.LogInformation($"GetCommentByTicketId: Request Body {ticketId}");
        try
        {
            if (!_context.Comments.Any())
            {
                _logger.LogError("There is no Comments for this ticket yet.");
                throw new InvalidOperationException("There is no Comments for this ticket yet.");
            }

            return _context.Comments
                .Where(c => c.TicketId == ticketId)
                .Select(c => new CommentDTO
                {
                    CommentId = c.CommentId,
                    TicketId = c.TicketId,
                    UserId = c.User.UserId,
                    FullName = c.User.FullName,
                    Replie = c.Replie,
                    CreateAt = c.CreateAt,
                })
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in GetCommentByTicketId: {ex.Message}");
            throw;
        }
    }

    public void AddComment(AddCommentDTO commentDto)
    {
        _logger.LogInformation($"AddComment: Request Body {commentDto}");
        try
        {
            var ticket = _context.Tickets.FirstOrDefault(t => t.TicketId == commentDto.TicketId);
            if (ticket == null)
            {
                _logger.LogError("There is no Ticket with this Id.");
                throw new InvalidOperationException("There is no Ticket with this Id.");
            }

            if (!_context.Users.Any(u => u.UserId == commentDto.UserId))
            {
                _logger.LogError("There is no User with this Id.");
                throw new InvalidOperationException("There is no User with this Id.");
            }

            // Add the comment
            var comment = new Comment
            {
                TicketId = commentDto.TicketId,
                UserId = commentDto.UserId,
                Replie = commentDto.Replie,
                CreateAt = DateTime.Now,
            };

            _context.Comments.Add(comment);
            _context.Tickets.Update(ticket);

            /*            if (ticket.AssignedTo == commentDto.UserId)
                        {
                            ticket.ModifiedAt = DateTime.Now;
                            _context.Tickets.Update(ticket);
                        }*/

            if (ticket.CreatedBy == commentDto.UserId)
            {
                ticket.ModifiedAt = DateTime.Now;
                ticket.ReminderLevel = 0;
                _context.Tickets.Update(ticket);
            }

            Save();
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError($"DbUpdateException in AddComment: {dbEx.InnerException?.Message ?? dbEx.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in AddComment: {ex.Message}");
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
