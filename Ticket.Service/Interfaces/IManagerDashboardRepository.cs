using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Data.Models.Enums;
using Ticket.Service.DTOs;
using Ticket.Service.Models;

namespace Ticket.Service.Interfaces;

public interface IManagerDashboardRepository
{
    DashboardStatisticsDTO GetDashboardStatistics();
    IEnumerable<MonthlyCountDTO> GetTicketsByMonth(int? month, int? year);
    IEnumerable<YearlyCountDTO> GetTicketsByYear(int? year);
    int GetTotalEmployees();
    int GetTotalClients();
    TicketSummaryDTO GetTotalTicketsByStatus(Status? status);
    TicketSummaryDTO GetTotalTicketsByPriorities(Priorities? priority);

    IEnumerable<TopEmployeeDTO> GetTopProductiveEmployees();
    public TicketSummaryDTO GetTotalTickets(Status? status, Priorities? priority = null);

    public StatusCountDTO GetTicketCountsByStatus();
    public PriorityCountDTO GetTicketCountsByPriority();
    IEnumerable<MonthlyTicketDTO> GetTicketsForCurrentYear();
}

