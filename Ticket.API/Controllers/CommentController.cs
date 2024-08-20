using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticket.Data.Models;
using Ticket.Service.DTOs;
using Ticket.Service.Interfaces;
using Ticket.Service.Models;
using Ticket.Service.Repositories;

namespace Ticket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly ILogger<CommentController> _logger;

    public CommentController(ICommentRepository commentRepository, ILogger<CommentController> logger)
    {
        _commentRepository = commentRepository;
        _logger = logger;
    }

    [HttpGet("get-comments/{TicketId}")]
    public IActionResult GetCommentsByTicketId(int TicketId)
    {
        _logger.LogInformation("Start GetCommentsByTicketId API");
        try
        {
            var comment = _commentRepository.GetCommentByTicketId(TicketId);
            if (comment == null)
            {
                return NotFound(new ApiResponse<string> { Status = false, Message = "there is no commets for this ticket" });
            }
            return Ok(new ApiResponse<IEnumerable<CommentDTO>> { Data = comment });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
            {
                Status = false,
                Message = "An error occurred while geting the Comments.",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPost("add-comment")]
    public IActionResult AddComment([FromBody] AddCommentDTO commentDto)
    {
        _logger.LogInformation("Start AddComment API");
        try
        {
            _commentRepository.AddComment(commentDto);
            return Ok(new ApiResponse<string> { Message = "Comment added successfully." });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in AddComment API: {ex.Message}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
            {
                Status = false,
                Message = "There is no Ticket with this Id or There is no User with this Id.",
                Errors = new List<string> { ex.Message }
            });
        }
    }

}
