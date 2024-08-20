using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticket.Data.Models;
using Ticket.Data.Models.Enums;
using Ticket.Integration.DTOs;
using Ticket.Service.DTOs;
using Ticket.Service.Interfaces;

namespace Ticket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ManagerDashboardController : ControllerBase
{
    private readonly IManagerDashboardRepository _dashboardRepository;
    private readonly ILogger<ManagerDashboardController> _logger;

    public ManagerDashboardController(IManagerDashboardRepository dashboardRepository, ILogger<ManagerDashboardController> logger)
    {
        _dashboardRepository = dashboardRepository;
        _logger = logger;
    }

    [HttpGet("statistics")]
    public IActionResult GetDashboardStatistics()
    {
        _logger.LogInformation("Start GetDashboardStatistics API");
        try
        {
            var statistics = _dashboardRepository.GetDashboardStatistics();
            return Ok(new ApiResponse<DashboardStatisticsDTO> { Data = statistics });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in GetDashboardStatistics: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string> { Status = false, Message = "An error occurred while retrieving dashboard statistics.", Errors = new List<string> { ex.Message } });
        }
    }

    [HttpPost("tickets-per-month")]
    public IActionResult GetTicketsByMonth([FromBody] MMonthRequest request)
    {
        _logger.LogInformation("Start GetTicketsByMonth API");
        try
        {
            var data = _dashboardRepository.GetTicketsByMonth(request.Month, request.Year);
            if (data == null || !data.Any())
            {
                return Ok(new ApiResponse<IEnumerable<MonthlyCountDTO>>
                {
                    Data = data,
                    Message = "No tickets found for the specified month and year."
                });
            }

            return Ok(new ApiResponse<IEnumerable<MonthlyCountDTO>>
            {
                Data = data,
                Message = "Tickets per month retrieved successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
            {
                Status = false,
                Message = "An error occurred while retrieving tickets per month.",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPost("tickets-per-year")]
    public IActionResult GetTicketsByYear([FromBody] MYearRequest request)
    {
        _logger.LogInformation("Start GetTicketsByYear API");
        try
        {
            var data = _dashboardRepository.GetTicketsByYear(request.Year);
            if (data == null || !data.Any())
            {
                return Ok(new ApiResponse<IEnumerable<YearlyCountDTO>>
                {
                    Data = data,
                    Message = "No tickets found for the specified year."
                });
            }

            return Ok(new ApiResponse<IEnumerable<YearlyCountDTO>>
            {
                Data = data,
                Message = "Tickets per year retrieved successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
            {
                Status = false,
                Message = "An error occurred while retrieving tickets per year.",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPost("total-tickets-By-status&priority")]
    public IActionResult GetTotalTicketsSummary([FromBody] TicketFilterRequest request)
    {
        _logger.LogInformation("Start GetTotalTicketsSummary API");
        try
        {
            var data = _dashboardRepository.GetTotalTickets(request.status, request.priority);
            if (data == null)
            {
                return Ok(new ApiResponse<TicketSummaryDTO>
                {
                    Data = data,
                    Message = "No tickets found."
                });
            }

            return Ok(new ApiResponse<TicketSummaryDTO>
            {
                Data = data,
                Message = "Total tickets summary retrieved successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
            {
                Status = false,
                Message = "An error occurred while retrieving total tickets summary.",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpGet("top-employees")]
    public IActionResult GetTopProductiveEmployees()
    {
        _logger.LogInformation("Start GetTopProductiveEmployees API");
        try
        {
            var data = _dashboardRepository.GetTopProductiveEmployees();
            if (data == null || !data.Any())
            {
                return Ok(new ApiResponse<IEnumerable<TopEmployeeDTO>>
                {
                    Data = data,
                    Message = "No employees with closed tickets found."
                });
            }

            return Ok(new ApiResponse<IEnumerable<TopEmployeeDTO>>
            {
                Data = data,
                Message = "Top productive employees retrieved successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
            {
                Status = false,
                Message = "An error occurred while retrieving top productive employees.",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpGet("tickets-by-status")]
    public IActionResult GetTicketCountsByStatus()
    {
        _logger.LogInformation("Start GetTicketCountsByStatus API");
        try
        {
            var data = _dashboardRepository.GetTicketCountsByStatus();
            if (data == null)
            {
                return Ok(new ApiResponse<StatusCountDTO>
                {
                    Data = data,
                    Message = "No tickets found."
                });
            }

            return Ok(new ApiResponse<StatusCountDTO>
            {
                Data = data,
                Message = "Ticket counts by status retrieved successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
            {
                Status = false,
                Message = "An error occurred while retrieving ticket counts by status.",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpGet("tickets-by-priority")]
    public IActionResult GetTicketCountsByPriority()
    {
        _logger.LogInformation("Start GetTicketCountsByPriority API");
        try
        {
            var data = _dashboardRepository.GetTicketCountsByPriority();
            if (data == null)
            {
                return Ok(new ApiResponse<PriorityCountDTO>
                {
                    Data = data,
                    Message = "No tickets found."
                });
            }

            return Ok(new ApiResponse<PriorityCountDTO>
            {
                Data = data,
                Message = "Ticket counts by priority retrieved successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
            {
                Status = false,
                Message = "An error occurred while retrieving ticket counts by priority.",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPost("total-tickets-by-status")]
    public IActionResult GetTotalTicketsByStatus([FromBody] TicketFilterRequestByStatus request)
    {
        _logger.LogInformation("Start GetTotalTicketsByStatus API");
        try
        {
            var data = _dashboardRepository.GetTotalTicketsByStatus(request.Status);
            if (data == null)
            {
                return Ok(new ApiResponse<TicketSummaryDTO>
                {
                    Data = data,
                    Message = "No tickets found for any status."
                });
            }

            return Ok(new ApiResponse<TicketSummaryDTO>
            {
                Data = data,
                Message = "Total tickets by status retrieved successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
            {
                Status = false,
                Message = "An error occurred while retrieving total tickets by status.",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPost("total-tickets-by-priority")]
    public IActionResult GetTotalTicketsByPriority([FromBody] TicketFilterRequestByPriorities request)
    {
        _logger.LogInformation("Start GetTotalTicketsByPriority API");
        try
        {
            var data = _dashboardRepository.GetTotalTicketsByPriorities(request.Priority);
            if (data == null)
            {
                return Ok(new ApiResponse<TicketSummaryDTO>
                {
                    Data = data,
                    Message = "No tickets found for any priority."
                });
            }

            return Ok(new ApiResponse<TicketSummaryDTO>
            {
                Data = data,
                Message = "Total tickets by priority retrieved successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
            {
                Status = false,
                Message = "An error occurred while retrieving total tickets by priority.",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpGet("tickets-current-year")]
    public IActionResult GetTicketsForCurrentYear()
    {
        _logger.LogInformation("Start GetTicketsForCurrentYear API");
        try
        {
            var data = _dashboardRepository.GetTicketsForCurrentYear();
            if (data == null)
            {
                return Ok(new ApiResponse<IEnumerable<MonthlyTicketDTO>>
                {
                    Data = data,
                    Message = "No tickets found for the current year."
                });
            }

            return Ok(new ApiResponse<IEnumerable<MonthlyTicketDTO>>
            {
                Data = data,
                Message = "Tickets for the current year retrieved successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
            {
                Status = false,
                Message = "An error occurred while retrieving tickets for the current year.",
                Errors = new List<string> { ex.Message }
            });
        }
    }
}
