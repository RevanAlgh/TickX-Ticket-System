using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Ticket.Service.Repositories;
using Ticket.Service.Interfaces;
using Ticket.Data.Models;
using Ticket.Data.Models.Data;
using Ticket.Data.Models.Enums;
using Ticket.Service.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Ticket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ApplicationDbContext context, ILogger<UserController> logger)
        {
            _userService = userService;
            _context = context;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDTO model)
        {
            _logger.LogInformation("Start Register API");
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return BadRequest(new ApiResponse<string> { Status = false, Errors = errors });
            }

            if (await _userService.EmailExistsAsync(model.Email))
            {
                return BadRequest(new ApiResponse<string> { Status = false, Message = "Email already exists. Please choose a different email." });
            }

            if (await _userService.UserNameExistsAsync(model.UserName))
            {
                return BadRequest(new ApiResponse<string> { Status = false, Message = "UserName already exists. Please choose a different Username." });
            }

            if (await _userService.MobileNumberExistsAsync(model.MobileNumber))
            {
                return BadRequest(new ApiResponse<string> { Status = false, Message = "Mobile number already exists. Please choose a different mobile number." });
            }

            try
            {
                await _userService.RegisterUserAsync(model);
                return Ok(new ApiResponse<string> { Data = "User registered successfully" });
            }
            catch (Exception ex)
            {
                // log exception details
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string> { Status = false, Message = "An error occurred while creating the user.", Errors = new List<string> { ex.Message } });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO model)
        {
            _logger.LogInformation("Start Login API");
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                    return BadRequest(new ApiResponse<string> { Status = false, Errors = errors });
                }

                // Check if user is active
                var user = await _userService.ValidateUserAsync(model.NENA, model.Password);
                if (user == null)
                {
                    return Unauthorized(new ApiResponse<string> { Status = false, Message = "Invalid email, number, username, password, or user is inactive." });
                }

                var token = _userService.GenerateJwtToken(user);

                var userResponse = _userService.GetUserResponse(user);
                return Ok(new ApiResponse<object> { Data = new { Success = "Login successful", User = userResponse, Token = token } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string> { Status = false, Message = "An error occurred while processing your request. Please try again later." });
            }

        }

        [HttpPost("request-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequestDTO request)
        {
            _logger.LogInformation("Start RequestPasswordReset API");

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new ApiResponse<string>
                {
                    Status = false,
                    Message = "Email is required."
                });
            }

            try
            {
                var result = await _userService.RequestPasswordResetAsync(request);

                if (!result)
                {
                    return NotFound(new ApiResponse<string>
                    {
                        Status = false,
                        Message = "User not found or unable to send reset email."
                    });
                }

                return Ok(new ApiResponse<string>
                {
                    Status = true,
                    Message = "Password reset link sent successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the password reset request.");
                return StatusCode(500, new ApiResponse<string> { Status = false, Message = "An error occurred while processing your request. Please try again later." });

            }
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDTO resetDTO)
        {
            _logger.LogInformation("Start ResetPassword API");

            if (string.IsNullOrWhiteSpace(resetDTO.Email) ||
                string.IsNullOrWhiteSpace(resetDTO.Token) ||
                string.IsNullOrWhiteSpace(resetDTO.NewPassword))
            {
                return BadRequest(new ApiResponse<string>
                {
                    Status = false,
                    Message = "Email, token, and new password are required."
                });
            }

            try
            {
                var result = await _userService.ResetPasswordAsync(resetDTO);

                if (!result)
                {
                    return BadRequest(new ApiResponse<string>
                    {
                        Status = false,
                        Message = "Invalid token or token expired."
                    });
                }

                return Ok(new ApiResponse<string>
                {
                    Status = true,
                    Message = "Password reset successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the password reset.");
                return StatusCode(500, new ApiResponse<string> { Status = false, Message = "An error occurred while processing your request. Please try again later." });

            }
        }
    }
}

