using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticket.Data.Models;
using Ticket.Service.DTOs;
using Ticket.Service.Interfaces;
using Ticket.Service.Models;

namespace Ticket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class TeamMemberController : ControllerBase
{
    private readonly IViewTeamMemberRepository _teamMemberRepository;
    private readonly ILogger<TeamMemberController> _logger;

    public TeamMemberController(IViewTeamMemberRepository teamMemberRepository, ILogger<TeamMemberController> logger)
    {
        _teamMemberRepository = teamMemberRepository;
        _logger = logger;
    }

    [HttpGet("team-members")]
    public IActionResult GetAllTeamMembers()
    {
        _logger.LogInformation("Start GetAllTeamMembers API");
        try
        {
            var teamMembers = _teamMemberRepository.GetAllTeamMembers();

            if (!teamMembers.Any()) 
            {
                return NotFound(new ApiResponse<string>
                {
                    Status = false,
                    Message = "No team members found",
                    Data = null
                });
            }
            return Ok(new ApiResponse<IEnumerable<ManagerDTO>> { Data = teamMembers });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in GetAllTeamMembers: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string> { Status = false, Message = "Internal server error", Errors = new List<string> { ex.Message } });
        }
    }

    [HttpGet("team-member/{userId}")]
    public IActionResult GetTeamMemberById(int userId)
    {
        _logger.LogInformation("Start GetTeamMemberById API");
        try
        {
            var teamMember = _teamMemberRepository.GetTeamMemberById(userId);
            if (teamMember == null)
            {
                return NotFound(new ApiResponse<string> { Status = false, Message = "Team member not found" });
            }
            return Ok(new ApiResponse<ManagerDTO> { Data = teamMember });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in GetTeamMemberById: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string> { Status = false, Message = "Internal server error", Errors = new List<string> { ex.Message } });
        }
    }

    [HttpPut("team-member")]
    public IActionResult UpdateTeamMember([FromBody] EditManagerDTO userDto)
    {
        _logger.LogInformation("Start UpdateTeamMember API");
        try
        {
            _teamMemberRepository.UpdateTeamMember(userDto);
            return Ok(new ApiResponse<string> { Status = true, Message = "Employee updated successfully." });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError($"Error in UpdateTeamMember: {ex.Message}");
            return NotFound(new ApiResponse<string> { Status = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in UpdateTeamMember: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string> { Status = false, Message = "Data already existed", Errors = new List<string> { ex.Message } });
        }
    }


    [HttpPut("team-member/toggle-activation")]
    public IActionResult UpdateTeamMemberActivation([FromBody] UpdateActivationDTO updateActivationDto)
    {
        _logger.LogInformation("Start UpdateTeamMemberActivation API");
        try
        {
            _teamMemberRepository.UpdateTeamMemberActivation(updateActivationDto);
            return Ok(new ApiResponse<string> { Status = true, Message = "Employee activation status updated successfully." });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError($"Error in UpdateTeamMemberActivation: {ex.Message}");
            return NotFound(new ApiResponse<string> { Status = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in UpdateTeamMemberActivation: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string> { Status = false, Message = "Internal server error", Errors = new List<string> { ex.Message } });
        }
    }


    [HttpGet("active-team-members")]
    public IActionResult GetActiveTeamMembers()
    {
        _logger.LogInformation("Start GetActiveTeamMembers API");
        try
        {
            var activeTeamMembers = _teamMemberRepository.GetActiveTeamMembers();
            if (!activeTeamMembers.Any())
            {
                return NotFound(new ApiResponse<string>
                {
                    Status = false,
                    Message = "No team members found",
                    Data = null
                });
            }
            return Ok(new ApiResponse<IEnumerable<ManagerDTO>> { Data = activeTeamMembers });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in GetActiveTeamMembers: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string> { Status = false, Message = "Internal server error", Errors = new List<string> { ex.Message } });
        }
    }
}
