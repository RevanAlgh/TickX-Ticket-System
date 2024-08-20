using Microsoft.AspNetCore.Mvc;
using Ticket.Service.Interfaces;
using Ticket.Data.Models;
using Ticket.Service.Interfaces;
using Ticket.Service.DTOs;
using Ticket.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Ticket.Service.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace Ticket.API.Controllers
{
    [Route("api/Ticket")]
    [ApiController]
    [Authorize]

    public class TicketController : ControllerBase
    {
        private readonly ITicketClassRepository _ticketRepository;
        private readonly ILogger _logger;

        public TicketController(ITicketClassRepository ticketRepository, ILogger<TicketController> logger)
        {
            _ticketRepository = ticketRepository;
            _logger = logger;
        }

        [HttpPost("ticket")]
        public async Task<IActionResult> AddTicket([FromBody] CreateTicketDTO ticketDto)
        {
            _logger.LogInformation("Start AddTicket API");
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return BadRequest(new ApiResponse<string> { Status = false, Errors = errors });
            }
            try
            {
                await _ticketRepository.AddTicket(ticketDto);
                return Ok(new ApiResponse<string> { Message = "ticket created successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a ticket.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string> { Status = false, Message = "An error occurred while creating the ticket.", Errors = new List<string> { ex.Message } });
            }
        }

        [HttpGet("tickets")]
        public IActionResult GetAllTickets()
        {
            _logger.LogInformation("Start GetAllTickets API");
            try
            {
                var tickets = _ticketRepository.GetAllTickets();

                var response = new ApiResponse<IEnumerable<TicketDTO>>
                {
                    Data = tickets,
                    Status = true,
                    Message = "Tickets retrieved successfully",
                    Errors = new List<string>()
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllTickets: {ex.Message}");
                var errorResponse = new ApiResponse<IEnumerable<TicketDTO>>
                {
                    Status = false,
                    Message = "No tickets available at the moment.",
                    Data = null,
                    Errors = new List<string> { ex.Message }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpGet("{ticketId}")]
        public IActionResult GetTicketById(int ticketId)
        {
            try
            {
                var ticket = _ticketRepository.GetTicketById(ticketId);
                return Ok(new ApiResponse<TicketDTO> { Data = ticket });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<string> { Status = false, Message = $"Ticket with ID {ticketId} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Status = false,
                    Message = "An error occurred while retrieving the ticket.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }


        [HttpGet("ticket/{userId}")]
        public IActionResult GetTicketsForUser(int userId)
        {
            _logger.LogInformation("Start GetTicketsForUser API");
            try
            {
                var tickets = _ticketRepository.GetTicketsForUser(userId);
                var response = new ApiResponse<TicketDTO>
                {
                    Status = tickets != null,
                    Data = tickets,
                    Message = tickets != null ? "Ticket retrieved successfully" : "No tickets found for the specified user",
                    Errors = tickets == null ? new List<string> { "No tickets found for the specified user." } : null
                };

                if (tickets == null)
                {
                    return NotFound(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetTicketsForUser: {ex.Message}");
                var errorResponse = new ApiResponse<TicketDTO>
                {
                    Status = false,
                    Message = "An error occurred while retrieving tickets.",
                    Data = null,
                    Errors = new List<string> { ex.Message }
                };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpPut("ticket/edit")]
        public async Task<IActionResult> UpdateTicket([FromBody] EditTicketDTO ticketDto)
        {
            _logger.LogInformation("Start UpdateTicket API");
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                var response = new ApiResponse<string>
                {
                    Status = false,
                    Message = "No Ticket with this ID is found.",
                    Data = null,
                    Errors = errors
                };
                return BadRequest(response);
            }

            try
            {
                _ticketRepository.UpdateTicket(ticketDto);
                _ticketRepository.Save();

                var response = new ApiResponse<string>
                {
                    Status = true,
                    Message = "Ticket updated successfully",
                    Data = null,
                    Errors = null
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating a ticket.");

                var statusCode = ex.Message.Contains("Both Attachment and FileName must be non-empty to update attachments.") ? StatusCodes.Status404NotFound : StatusCodes.Status500InternalServerError;

                var errorResponse = new ApiResponse<string>
                {
                    Status = false,
                    Message = ex.Message,
                    Data = null,
                    Errors = new List<string> { ex.Message }
                };

                return StatusCode(statusCode, errorResponse);
            }
        }

        [HttpPut("assign-ticket")]
        public async Task<IActionResult> AssignTicket([FromBody] AssignTicketDTO assignTicketDto)
        {
            _logger.LogInformation("Start AssignTicket API");
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return BadRequest(new ApiResponse<string> { Status = false, Errors = errors });
            }

            try
            {
                string result = await _ticketRepository.AssignTicket(assignTicketDto);
                return Ok(new ApiResponse<string> { Status = true, Data = result });
            }
            catch (Exception ex)
            {
                if (ex.Message == "Ticket not found." || ex.Message == "User not found." || ex.Message == "Cannot assign ticket to client.")
                {
                    return BadRequest(new ApiResponse<string> { Status = false, Message = ex.Message });
                }
                _logger.LogError(ex, "Error occurred while assigning the ticket.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string> { Status = false, Message = "An error occurred while assigning the ticket.", Errors = new List<string> { ex.Message } });
            }
        }

        [HttpGet("tickets-by-client/{clientId}")]
        public async Task<IActionResult> GetTicketsByClient(int clientId)
        {
            _logger.LogInformation("Start GetTicketsByClient API");
            try
            {
                var tickets = await _ticketRepository.GetTicketsByClient(clientId);
                if (!tickets.Any())
                {
                    return Ok(new ApiResponse<string> { Status = true, Message = "No tickets found for this client." });
                }
                return Ok(new ApiResponse<IEnumerable<TicketDTO>> { Status = true, Data = tickets });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching tickets by client.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string> { Status = false, Message = "An error occurred while fetching the tickets.", Errors = new List<string> { ex.Message } });
            }
        }

        [HttpGet("ticket-status/{ticketId}")]
        public async Task<IActionResult> GetTicketStatus(int ticketId)
        {
            _logger.LogInformation("Start GetTicketStatus API");
            try
            {
                var status = await _ticketRepository.GetTicketStatus(ticketId);

                if (status == null)
                {
                    return NotFound(new ApiResponse<string> { Status = false, Message = "Ticket not found." });
                }

                return Ok(new ApiResponse<string> { Status = true, Data = status.ToString() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching the ticket status.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string> { Status = false, Message = "There is no status for this ticket.", Errors = new List<string> { ex.Message } });
            }
        }
        
        [HttpPut("ticket/status")]
        public async Task<IActionResult> ChangeTicketStatus([FromBody] ChangeStatusDTO ticketDto)
        {
            _logger.LogInformation("Start ChangeTicketStatus API");
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                var response = new ApiResponse<string>
                {
                    Status = false,
                    Message = "Validation failed",
                    Data = null,
                    Errors = errors
                };
                return BadRequest(response);
            }

            try
            {
                await _ticketRepository.UpdateTicketStatus(ticketDto);
                _ticketRepository.Save();

                var response = new ApiResponse<string>
                {
                    Status = true,
                    Message = "Ticket status updated successfully",
                    Data = null,
                    Errors = null
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating a ticket status.");

                var errorResponse = new ApiResponse<string>
                {
                    Status = false,
                    Message = "No tickets available for this status at the moment",
                    Data = null,
                    Errors = new List<string> { ex.Message }
                };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpPut("ticket/priority")]
        public async Task<IActionResult> UpdateTicketPriority([FromBody] ChangePriorityDTO priorityDTO)
        {
            _logger.LogInformation("Start UpdateTicketPriority API");
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                var response = new ApiResponse<string>
                {
                    Status = false,
                    Message = "Validation failed",
                    Data = null,
                    Errors = errors
                };
                return BadRequest(response);
            }

            try
            {
                await _ticketRepository.UpdateTicketPriority(priorityDTO);
                _ticketRepository.Save();

                var response = new ApiResponse<string>
                {
                    Status = true,
                    Message = "Ticket priority updated successfully",
                    Data = null,
                    Errors = null
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating a ticket priority.");

                var errorResponse = new ApiResponse<string>
                {
                    Status = false,
                    Message = "No tickets available for this priority at the moment",
                    Data = null,
                    Errors = new List<string> { ex.Message }
                };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpGet("assignedTo/{assignedToUserId}")]
        public async Task<IActionResult> GetTicketsByEmployee(int assignedToUserId)
        {
            _logger.LogInformation("Start GetTicketsByEmployee API");
            try
            {
                var tickets = await _ticketRepository.ListTicketsByEmployee(assignedToUserId);
                if (!tickets.Any())
                {
                    return Ok(new ApiResponse<string> { Status = true, Data = "No tickets found for this employee." });
                }
                return Ok(new ApiResponse<IEnumerable<TicketDTO>> { Status = true, Data = tickets });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "No tickets found for this employee.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string> { Status = false, Message = "No tickets found for this employee.", Errors = new List<string> { ex.Message } });
            }
        }

        [HttpGet("manager")]
        public IActionResult GetTicketCountsForManager()
        {
            _logger.LogInformation("Start GetTicketCountsForManager API");
            try
            {
                var counts = _ticketRepository.GetTicketCountsForManager();

                if (counts == null)
                {
                    return NotFound(new ApiResponse<string>
                    {
                        Status = false,
                        Message = "No tickets found for this employee.",
                        Data = null
                    });
                }
                return Ok(new ApiResponse<TicketStatusCountsDTO>
                {
                    Data = counts,
                    Status = true,
                    Message = "Ticket count retrieved successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetTicketCountsForManager: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<TicketStatusCountsDTO>
                {
                    Status = false,
                    Message = "No tickets found for this employee yet.",
                    Data = null,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetTicketCountsForClientOrEmployee(int userId)
        {
            _logger.LogInformation("Start GetTicketCountsForClientOrEmployee API");
            try
            {
                var counts = _ticketRepository.GetTicketCountsForClientOrEmployee(userId);

                if (counts == null)
                {
                    return NotFound(new ApiResponse<string>
                    {
                        Status = false,
                        Message = "No tickets found for this user.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<TicketStatusCountsDTO>
                {
                    Data = counts,
                    Status = true,
                    Message = "Ticket counts for client/employee retrieved successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetTicketCountsForClientOrEmployee: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<TicketStatusCountsDTO>
                {
                    Status = false,
                    Message = "No tickets found for this client/employee.",
                    Data = null,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

       

        [HttpPost("filter-tickets")]
        public async Task<IActionResult> GetFilteredTickets([FromBody] TicketFilterDTO ticketFilter)
        {
            _logger.LogInformation("Start GetFilteredTickets API");
            try
            {
                var tickets = await _ticketRepository.GetFilteredTickets(ticketFilter);

                if (!tickets.Any())
                {
                    return Ok(new ApiResponse<string> { Status = true, Message = "No tickets found matching the specified criteria." });
                }

                return Ok(new ApiResponse<IEnumerable<TicketDTO>> { Status = true, Data = tickets });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching filtered tickets.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Status = false,
                    Message = "An error occurred while fetching the tickets.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
}
