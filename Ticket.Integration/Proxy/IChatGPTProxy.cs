using Refit;
using Ticket.Integration.DTOs;


namespace Ticket.Integration.Proxy;

public interface IChatGPTProxy
{
    [Post("/v1/chat/completions")]
    Task<ChatGPTResponse> GetChatGPTResponseAsync([Body] ChatGPTRequest request);
}





