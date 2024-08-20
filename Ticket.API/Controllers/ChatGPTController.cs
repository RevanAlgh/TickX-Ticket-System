using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ticket.Integration.Services;


namespace Ticket.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]

    public class ChatGPTController : ControllerBase
    {
        private readonly ChatGPTService _chatGPTService;
        private readonly  ILogger<ChatGPTController> _logger;

    public ChatGPTController(ChatGPTService chatGPTService, ILogger<ChatGPTController> logger)
    {
        _chatGPTService = chatGPTService;
        _logger = logger;
    }

    [HttpPost("ask")]
    public async Task<IActionResult> AskChatGPT([FromBody] ChatGPTRequestDto request)
    {
        _logger.LogInformation("Start AskChatGPT API");
        if (string.IsNullOrWhiteSpace(request.Prompt))
        {
            return BadRequest(new { message = "The prompt field is required." });
        }

        var response = await _chatGPTService.GetResponseAsync(request.Prompt);
        return Ok(response);
    }

    public class ChatGPTRequestDto
    {
        public string Prompt { get; set; }
    }

}



