
using Microsoft.AspNetCore.Mvc;
using Ticket.Service.DTOs;
using Ticket.Service.Interfaces;
using Ticket.Data.Models.Enums;
using System;
using System.Collections.Generic;
using Ticket.Data.Models;
using System.Net.Sockets;
using Microsoft.AspNetCore.Authorization;


namespace Ticket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class DashboardController : ControllerBase
{
    private readonly IDashboardRepository _dashboardRepository;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(IDashboardRepository dashboardRepository, ILogger<DashboardController> logger)
    {
        _dashboardRepository = dashboardRepository;
        _logger = logger;
    }

    [HttpGet("dashboard-statistics/{userId}")]
    public IActionResult GetDashboardStatistics(int userId)
    {
        _logger.LogInformation("Start GetDashboardStatistics API");
        try
        {
            var data = _dashboardRepository.GetDashboardStatistics(userId);
            return Ok(new ApiResponse<TotalTicketsDTO>
            {
                Data = data,
                Message = "Dashboard statistics retrieved successfully."
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Status = false,
                Message = "User ID does not match any ticket records"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
            {
                Status = false,
                Message = "An error occurred while retrieving dashboard statistics.",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPost("tickets-by-month")]
    public IActionResult GetTicketsByMonth([FromBody] MonthRequest request)
    {
        _logger.LogInformation("Start GetTicketsByMonth API");
        try
        {
            var data = _dashboardRepository.GetTicketsByMonth(request.UserId, request.Month, request.Year);
            return Ok(new ApiResponse<IEnumerable<MonthlyCountDTO>>
            {
                Data = data,
                Message = "Tickets by month retrieved successfully."
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Status = false,
                Message = "No tickets found for the specified month and year."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
            {
                Status = false,
                Message = "An error occurred while retrieving tickets by month.",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPost("tickets-by-year")]
    public IActionResult GetTicketsByYear([FromBody] YearRequest request)
    {
        _logger.LogInformation("Start GetTicketsByYear API");
        try
        {
            var data = _dashboardRepository.GetTicketsByYear(request.UserId, request.Year);
            return Ok(new ApiResponse<IEnumerable<YearlyCountDTO>>
            {
                Data = data,
                Message = "Tickets by year retrieved successfully."
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Status = false,
                Message = "No tickets found for the specified year."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
            {
                Status = false,
                Message = "An error occurred while retrieving tickets by year.",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPost("total-tickets")]
    public IActionResult GetTotalTickets([FromBody] DashboardRequest request)
    {
        _logger.LogInformation("Start GetTotalTickets API");
        try
        {
            var data = _dashboardRepository.GetTotalTickets(request.UserId, request.Status, request.Priority);
            return Ok(new ApiResponse<TotalTicketsDTO>
            {
                Data = data,
                Message = "Total tickets retrieved successfully."
            });
        }

        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Status = false,
                Message = "No tickets found"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
            {
                Status = false,
                Message = "An error occurred while retrieving total tickets.",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPost("total-tickets-by-status")]
    public IActionResult GetTotalTicketsByStatus([FromBody] DashboardRequestByStatus request)
    {
        _logger.LogInformation("Start GetTotalTicketsByStatus API");
        try
        {
            var data = _dashboardRepository.GetTotalTicketsByStatus(request.UserId, request.Status);
            return Ok(new ApiResponse<TotalTicketsDTO>
            {
                Data = data,
                Message = "Total tickets retrieved successfully."
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Status = false,
                Message = "No tickets found for this Status"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
            {
                Status = false,
                Message = "An error occurred while retrieving total tickets.",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPost("total-tickets-by-priority")]
    public IActionResult GetTotalTicketsByPriority([FromBody] DashboardRequestByPriorities request)
    {
        _logger.LogInformation("Start GetTotalTicketsByPriority API");
        try
        {
            var data = _dashboardRepository.GetTotalTicketsByPriorities(request.UserId, request.Priority);
            return Ok(new ApiResponse<TotalTicketsDTO>
            {
                Data = data,
                Message = "Total tickets retrieved successfully."
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Status = false,
                Message = "No tickets found for this Priority"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
            {
                Status = false,
                Message = "An error occurred while retrieving total tickets.",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpGet("status-count/{userId}")]
    public IActionResult GetTicketCountsByStatus(int userId)
    {
        _logger.LogInformation("Start GetTicketCountsByStatus API");
        try
        {
            var data = _dashboardRepository.GetTicketCountsByStatus(userId);
            return Ok(new ApiResponse<StatusCountDTO>
            {
                Data = data,
                Message = "Ticket status counts retrieved successfully."
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Status = false,
                Message = "No tickets found for any status"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
            {
                Status = false,
                Message = "An error occurred while retrieving ticket status counts.",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpGet("priority-count/{userId}")]
    public IActionResult GetTicketCountsByPriority(int userId)
    {
        _logger.LogInformation("Start GetTicketCountsByPriority API");
        try
        {
            var data = _dashboardRepository.GetTicketCountsByPriority(userId);
            return Ok(new ApiResponse<PriorityCountDTO>
            {
                Data = data,
                Message = "Ticket priority counts retrieved successfully."
            });
        }

        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Status = false,
                Message = "No tickets found for any priority."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
            {
                Status = false,
                Message = "An error occurred while retrieving ticket priority counts.",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPut("recent-tickets")]
    public IActionResult GetRecentTicketsForClient([FromBody] RecentTicketsRequestDTO request)
    {
        _logger.LogInformation("Start GetRecentTicketsForClient API");
        try
        {
            var data = _dashboardRepository.GetRecentTicketsForClient(request);
            return Ok(new ApiResponse<IEnumerable<RecentTicketDTO>>
            {
                Data = data,
                Message = "Recent tickets retrieved successfully."
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<string>
            {
                Status = false,
                Message = "Invalid Data."
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Status = false,
                Message = "No recent tickets found for the client."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
            {
                Status = false,
                Message = "An error occurred while retrieving recent tickets.",
                Errors = new List<string> { ex.Message }
            });
        }
    }

}





