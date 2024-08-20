using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Data.Models.Enums;
using Ticket.Service.DTOs;

namespace Ticket.Service.Interfaces;

public interface IDashboardRepository
{
    TotalTicketsDTO GetDashboardStatistics(int userId);
    IEnumerable<MonthlyCountDTO> GetTicketsByMonth(int userId, int? month, int? year);
    IEnumerable<YearlyCountDTO> GetTicketsByYear(int userId, int? year);
    TotalTicketsDTO GetTotalTickets(int userId, Status? status = null, Priorities? priority = null);
    TotalTicketsDTO GetTotalTicketsByStatus(int userId, Status? status = null);
    TotalTicketsDTO GetTotalTicketsByPriorities(int userId, Priorities? priority = null);
    public StatusCountDTO GetTicketCountsByStatus(int userId);
    public PriorityCountDTO GetTicketCountsByPriority(int userId);
    IEnumerable<RecentTicketDTO> GetRecentTicketsForClient(RecentTicketsRequestDTO request);

}