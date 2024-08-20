using System.Xml.Linq;
using Ticket.Data.Models;
using Ticket.Data.Models.Enums;
using Ticket.Service.DTOs;

namespace Ticket.Service.Interfaces
{
    public interface ITicketClassRepository
    {
        Task AddTicket(CreateTicketDTO ticketDto);
        IEnumerable<TicketDTO> GetAllTickets();
        TicketDTO GetTicketById(int ticketId);
        TicketDTO GetTicketsForUser(int userId);
        void UpdateTicket(EditTicketDTO ticketDto);
        Task<string> AssignTicket(AssignTicketDTO assignTicketDto);
        Task<IEnumerable<TicketDTO>> GetTicketsByClient(int clientId);
        Task<Status?> GetTicketStatus(int ticketId);
        Task UpdateTicketStatus(ChangeStatusDTO changeStatusDTO);
        Task<IEnumerable<TicketDTO>> ListTicketsByEmployee(int assignedToUserId);
        TicketStatusCountsDTO GetTicketCountsForManager();
        TicketStatusCountsDTO GetTicketCountsForClientOrEmployee(int userId);
        Task<IEnumerable<TicketDTO>> GetFilteredTickets(TicketFilterDTO filter);
        Task<User?> GetUserByTicketId(int ticketId);
        Task UpdateTicketPriority(ChangePriorityDTO changePriorityDTO);
        void Save();

        // For the JobScheduler:

        Task<IEnumerable<TicketReminderDTO>> FindTicketsInProgressForReminderAsync(DateTime thresholdTime);
        Task UpdateTicketReminderLevelAsync(int ticketId, int reminderLevel);
        Task UpdateTicketStatusAsync(int ticketId, Status newStatus);
    }
}