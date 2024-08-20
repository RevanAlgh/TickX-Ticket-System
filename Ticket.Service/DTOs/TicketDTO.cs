using Ticket.Data.Models.Enums;

namespace Ticket.Service.DTOs
{
    public class TicketDTO
    {
        public int TicketId { get; set; }
        public int ProductId { get; set; }
        public string TicketDescription { get; set; }
        public string Title { get; set; }
        public List<string>? FileName { get; set; }  
        public List<string>? Attachment { get; set; } 
        public Status status { get; set; }
        public DateTime? ClosedAt { get; set; } = null;
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public int? ClosedBy { get; set; } = 0;
        public int? AssignedTo { get; set; } = 0;
        public Priorities? Priority { get; set; } = null;
        public int? CreatedBy { get; set; } 
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class TicketReminderDTO
    {
        public int TicketId { get; set; }
        public string Email { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public Status Status { get; set; }
        public int ReminderLevel { get; set; }

    }


    public class CreateTicketDTO
    {
        public int ProductId { get; set; }
        public string TicketDescription { get; set; }
        public string Title { get; set; }
        public List<string>? Attachment { get; set; } 
        public List<string>? FileName { get; set; } 
        public Status status { get; set; }
        public Priorities Priority { get; set; }
        public int? CreatedBy { get; set; }
    }
    public class EditTicketDTO
    {
        public int TicketId { get; set; }
        public List<string>? Attachment { get; set; } 
        public List<string>? FileName { get; set; }
        public Status status { get; set; }
    }

    public class AssignTicketDTO
    {
        public int TicketId { get; set; }
        public int AssignedTo { get; set; }
    }

    public class TicketStatusCountsDTO
    {
        public int Total { get; set; }
        public int New { get; set; }
        public int Closed { get; set; }
        public int InProgress { get; set; }
    }
    public class ChangeStatusDTO
    {
        public int TicketId { get; set; }
        public Status status { get; set; }
    }

    public class ChangePriorityDTO
    {
        public int TicketId { get; set; }
        public Priorities priority { get; set; }
    }

    public class TicketFilterDTO
    {
        public Priorities? Priority { get; set; }
        public int? AssignedToUserId { get; set; }
        public int? ClientId { get; set; }
        public Status? Status { get; set; }
        public string Name { get; set; } = string.Empty;
    }


}
