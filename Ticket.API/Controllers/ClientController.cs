using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticket.Data.Models;
using Ticket.Service.DTOs;
using Ticket.Service.Interfaces;
using Ticket.Service.Models;
using WireMock.Admin.Mappings;

namespace Ticket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class ClientController : ControllerBase
{
    private readonly IClientRepository _clientRepository;
    private readonly ILogger<ClientController> _logger;

    public ClientController(IClientRepository clientRepository, ILogger<ClientController> logger)
    {
        _clientRepository = clientRepository;
        _logger = logger;
    }

    [HttpGet("clients")]
    public IActionResult GetAllClients()
    {
        _logger.LogInformation("Start GetAllClients API");
        try
        {
            var clients = _clientRepository.GetAllClients();
            if (!clients.Any())
            {
                return NotFound(new ApiResponse<string>
                {
                    Status = false,
                    Message = "No Clients found",
                    Data = null
                });
            }
            return Ok(new ApiResponse<IEnumerable<ClientWithTicketCountDTO>> { Data = clients });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in GetAllClients: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string> { Status = false, Message = "Internal server error", Errors = new List<string> { ex.Message } });
        }
    }

    [HttpGet("client/{userId}")]
    public IActionResult GetClientById(int userId)
    {
        _logger.LogInformation("Start GetClientById API");
        try
        {
            var client = _clientRepository.GetClientById(userId);
            if (client == null)
            {
                return NotFound(new ApiResponse<string> { Status = false, Message = "Client not found" });
            }
            return Ok(new ApiResponse<ManagerDTO> { Data = client });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in GetClientById: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string> { Status = false, Message = "Internal server error", Errors = new List<string> { ex.Message } });
        }
    }

    [HttpPut("client")]
    public IActionResult UpdateClient([FromBody] EditManagerDTO managerDto)
    {
        _logger.LogInformation("Start UpdateClient API");
        try
        {
            _clientRepository.UpdateClient(managerDto);
            return Ok(new ApiResponse<string> { Status = true, Message = "Client updated successfully." });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError($"Error in UpdateClient: {ex.Message}");
            return NotFound(new ApiResponse<string> { Status = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in UpdateClient: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string> { Status = false, Message = "Data already existed", Errors = new List<string> { ex.Message } });
        }
    }


    [HttpPut("client/toggle-activation")]
    public IActionResult UpdateClientActivation([FromBody] UpdateActivationDTO updateActivationDto)
    {
        _logger.LogInformation("Start UpdateClientActivation API");
        try
        {
            _clientRepository.UpdateClientActivation(updateActivationDto);
            return Ok(new ApiResponse<string> { Status = true, Message = "Client activation status updated successfully." });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError($"Error in UpdateClientActivation: {ex.Message}");
            return NotFound(new ApiResponse<string> { Status = false, Message = "Error in Updating the Client Activation" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in UpdateClientActivation: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string> { Status = false, Message = "Internal server error", Errors = new List<string> { ex.Message } });
        }
    }

}

