
namespace Ticket.Integration.DTOs;

public class ChatGPTResponse
{
    public List<Choice> choices { get; set; }
}

public class Choice
{
    public Message message { get; set; }
}
