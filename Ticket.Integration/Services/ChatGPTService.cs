using Ticket.Integration.DTOs;
using Ticket.Integration.Proxy;

namespace Ticket.Integration.Services;

public class ChatGPTService
{
    private readonly IChatGPTProxy _chatGPTApi;

    public ChatGPTService(IChatGPTProxy chatGPTApi)
    {
        _chatGPTApi = chatGPTApi;
    }

    public async Task<string> GetResponseAsync(string prompt)
    {
        var request = new ChatGPTRequest
        {
            messages = new List<Message>
            {
                new Message { role = "user", content = prompt }
            }
        };

        var response = await _chatGPTApi.GetChatGPTResponseAsync(request);
        return response.choices.FirstOrDefault()?.message.content ?? "No response from ChatGPT.";
    }
}

