
namespace Ticket.Integration.DTOs;

public class ChatGPTRequest
{
    public string model { get; set; } = "gpt-3.5-turbo";
    public List<Message> messages { get; set; }
    public int max_tokens { get; set; } = 1000;
}

public class Message
{
    public string role { get; set; } = "user";
    public string content { get; set; }
}
