using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticket.Data.Models;
using Ticket.Service.DTOs;
using Ticket.Service.Interfaces;

namespace Ticket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class ViewTicketController : ControllerBase
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ILogger<ViewTicketController> _logger;

    public ViewTicketController(ITicketRepository ticketRepository, ILogger<ViewTicketController> logger)
    {
        _ticketRepository = ticketRepository;
        _logger = logger;
    }

    [HttpGet("ticket-with-replies/{ticketId}")]
    public IActionResult GetTicketWithReplies(int ticketId)
    {
        _logger.LogInformation("Start GetTicketWithReplies API");
        try
        {
            var ticket = _ticketRepository.GetTicketWithReplies(ticketId);
            return Ok(new ApiResponse<TicketWithRepliesDTO> { Data = ticket });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError($"Error in GetTicketWithReplies: {ex.Message}");
            return NotFound(new ApiResponse<string> { Status = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in GetTicketWithReplies: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string> { Status = false, Message = "Internal server error", Errors = new List<string> { ex.Message } });
        }
    }
}
