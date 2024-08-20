using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Data.Models.Enums;

namespace Ticket.Service.DTOs;

public class DashboardStatisticsDTO
{
    public int EmployeeCount { get; set; }
    public int ClientCount { get; set; }
    public int TicketCount { get; set; }
}


public class MonthlyCountDTO
{
    public int Month { get; set; }
    public int TicketsCount { get; set; }
}

public class YearlyCountDTO
{
    public int Year { get; set; }
    public int TicketsCount { get; set; }
}

public class TopEmployeeDTO
{
    public int UserId { get; set; }
    public string FullName { get; set; }
    public string? UserImage { get; set; }
    public int ClosedTicketsCount { get; set; }
}

public class TicketSummaryDTO
{
    public int TotalTickets { get; set; }
    public int TotalAssignedEmployees { get; set; }
    public int TotalClients { get; set; }
}

public class TotalTicketsDTO
{
    public int TotalTickets { get; set; }
}

public class DashboardRequest
{
    public int UserId { get; set; }
    public Status? Status { get; set; }
    public Priorities? Priority { get; set; }
}

public class DashboardRequestByStatus
{
    public int UserId { get; set; }
    public Status? Status { get; set; }
}

public class DashboardRequestByPriorities
{
    public int UserId { get; set; }
    public Priorities? Priority { get; set; }
}

public class MonthRequest
{
    public int UserId { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }
}

public class MMonthRequest
{
    public int? Month { get; set; }
    public int? Year { get; set; }
}

public class MYearRequest
{
    public int? Year { get; set; }
}

public class YearRequest
{
    public int UserId { get; set; }
    public int? Year { get; set; }
}

public class TicketFilterRequestByStatus
{
    public Status? Status { get; set; }
}

public class TicketFilterRequestByPriorities
{
    public Priorities? Priority { get; set; }
}

public class TicketFilterRequest
{
    public Status? status { get; set; }

    public Priorities? priority { get; set; }
}

public class StatusCountDTO
{
    public int New { get; set; }
    public int InProgress { get; set; }
    public int Resolved { get; set; }
    public int Closed { get; set; }
    public int ReOpened { get; set; }
}


public class PriorityCountDTO
{
    public int Low { get; set; }
    public int Medium { get; set; }
    public int High { get; set; }
}


public class MonthlyTicketDTO
{
    public string Month { get; set; }
    public int TicketCount { get; set; }
}

public class RecentTicketDTO
{
    public int TicketId { get; set; }
    public int ProductId { get; set; }
    public string Title { get; set; }
    public string TicketDescription { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ClosedBy { get; set; }
    public int? AssignedTo { get; set; }
    public int Priority { get; set; }
    public int Status { get; set; }
    public string ProductName { get; set; }
    public int CreatedBy { get; set; }
}

public class RecentTicketsRequestDTO
{
    public int UserId { get; set; }
    public int TicketCount { get; set; }
}
