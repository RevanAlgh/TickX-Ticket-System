using Microsoft.Extensions.Logging;
using System.Globalization;
using Ticket.Data.Models.Data;
using Ticket.Data.Models.Enums;
using Ticket.Service.DTOs;
using Ticket.Service.Interfaces;

namespace Ticket.Service.Repositories;

public class ManagerDashboardRepository : IManagerDashboardRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ManagerDashboardRepository> _logger;

    public ManagerDashboardRepository(ApplicationDbContext context, ILogger<ManagerDashboardRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public DashboardStatisticsDTO GetDashboardStatistics()
    {
        _logger.LogInformation("GetDashboardStatistics");
        try
        {
            var employeeCount = _context.Users.Count(u => u.Role == Roles.TeamMember || u.Role == Roles.SupportManager);
            var clientCount = _context.Users.Count(u => u.Role == Roles.Client);
            var ticketCount = _context.Tickets.Count();

            return new DashboardStatisticsDTO
            {
                EmployeeCount = employeeCount,
                ClientCount = clientCount,
                TicketCount = ticketCount
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in GetDashboardStatistics: {ex.Message}");
            throw;
        }
    }

    public IEnumerable<MonthlyCountDTO> GetTicketsByMonth(int? month, int? year)
    {
        _logger.LogInformation($"GetTicketsByMonth: Request Body {month}, {year}");
        var query = _context.Tickets.AsQueryable();

        if (month.HasValue)
        {
            query = query.Where(t => t.CreatedAt.HasValue && t.CreatedAt.Value.Month == month.Value);
        }

        if (year.HasValue)
        {
            query = query.Where(t => t.CreatedAt.HasValue && t.CreatedAt.Value.Year == year.Value);
        }

        if (!query.Any())
        {
            _logger.LogError("No tickets found for the specified month and year.");
            throw new InvalidOperationException("No tickets found for the specified month and year.");
        }

        return query
            .Where(t => t.CreatedAt.HasValue)
            .GroupBy(t => t.CreatedAt.Value.Month)
            .Select(g => new MonthlyCountDTO
            {
                Month = g.Key,
                TicketsCount = g.Count()
            }).ToList();
    }

    public IEnumerable<YearlyCountDTO> GetTicketsByYear(int? year)
    {
        _logger.LogInformation($"GetTicketsByYear: Request Body {year}");
        var query = _context.Tickets.AsQueryable();

        if (year.HasValue)
        {
            query = query.Where(t => t.CreatedAt.HasValue && t.CreatedAt.Value.Year == year.Value);
        }

        if (!query.Any())
        {
            _logger.LogError("No tickets found for the specified year.");
            throw new InvalidOperationException("No tickets found for the specified year.");
        }

        return query
            .Where(t => t.CreatedAt.HasValue)
            .GroupBy(t => t.CreatedAt.Value.Year)
            .Select(g => new YearlyCountDTO
            {
                Year = g.Key,
                TicketsCount = g.Count()
            }).ToList();
    }

    public int GetTotalEmployees()
    {
        _logger.LogInformation("GetTotalEmployees");
        var employeeCount = _context.Users.Count(u => u.Role == Roles.TeamMember);

        if (employeeCount == 0)
        {
            _logger.LogError("No employees found.");
            throw new InvalidOperationException("No employees found.");
        }

        return employeeCount;
    }

    public int GetTotalClients()
    {
        _logger.LogInformation("GetTotalClients");
        var clientCount = _context.Users.Count(u => u.Role == Roles.Client);

        if (clientCount == 0)
        {
            _logger.LogError("No clients found.");
            throw new InvalidOperationException("No clients found.");
        }

        return clientCount;
    }

    public TicketSummaryDTO GetTotalTickets(Status? status, Priorities? priority = null)
    {
        _logger.LogInformation($"GetTotalTickets: Request Body {status}");
        var query = _context.Tickets.AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(t => t.status == status.Value);
        }

        if (priority.HasValue)
        {
            query = query.Where(t => t.Priority == priority.Value);
        }

        var ticketCount = query.Count();

        if (ticketCount == 0)
        {
            _logger.LogError("No tickets found.");
            throw new InvalidOperationException("No tickets found.");
        }

        var assignedEmployeesCount = query
            .Where(t => t.AssignedTo.HasValue)
            .Select(t => t.AssignedTo.Value)
            .Distinct()
            .Count();

        var clientsCount = query
            .Where(t => t.CreatedBy.HasValue)
            .Select(t => t.CreatedBy.Value)
            .Distinct()
            .Count();

        return new TicketSummaryDTO
        {
            TotalTickets = ticketCount,
            TotalAssignedEmployees = assignedEmployeesCount,
            TotalClients = clientsCount
        };
    }

    public TicketSummaryDTO GetTotalTicketsByStatus(Status? status)
    {
        _logger.LogInformation($"GetTotalTicketsByStatus: Request Body {status}");
        var query = _context.Tickets.AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(t => t.status == status.Value);
        }

        var ticketCount = query.Count();

        if (ticketCount == 0)
        {
            _logger.LogError("No tickets found.");
            throw new InvalidOperationException("No tickets found.");
        }

        var assignedEmployeesCount = query
            .Where(t => t.AssignedTo.HasValue)
            .Select(t => t.AssignedTo.Value)
            .Distinct()
            .Count();

        var clientsCount = query
            .Where(t => t.CreatedBy.HasValue)
            .Select(t => t.CreatedBy.Value)
            .Distinct()
            .Count();

        return new TicketSummaryDTO
        {
            TotalTickets = ticketCount,
            TotalAssignedEmployees = assignedEmployeesCount,
            TotalClients = clientsCount
        };
    }

    public TicketSummaryDTO GetTotalTicketsByPriorities(Priorities? priority)
    {
        _logger.LogInformation($"GetTotalTicketsByPriorities: Request Body {priority}");
        var query = _context.Tickets.AsQueryable();

        if (priority.HasValue)
        {
            query = query.Where(t => t.Priority == priority.Value);
        }

        var ticketCount = query.Count();

        if (ticketCount == 0)
        {
            _logger.LogError("No tickets found.");
            throw new InvalidOperationException("No tickets found.");
        }

        var assignedEmployeesCount = query
            .Where(t => t.AssignedTo.HasValue)
            .Select(t => t.AssignedTo.Value)
            .Distinct()
            .Count();

        var clientsCount = query
            .Where(t => t.CreatedBy.HasValue)
            .Select(t => t.CreatedBy.Value)
            .Distinct()
            .Count();

        return new TicketSummaryDTO
        {
            TotalTickets = ticketCount,
            TotalAssignedEmployees = assignedEmployeesCount,
            TotalClients = clientsCount
        };
    }

    public IEnumerable<TopEmployeeDTO> GetTopProductiveEmployees()
    {
        _logger.LogInformation("GetTopProductiveEmployees");
        var status = Status.Closed;
        var topCount = 5;

        var topEmployees = _context.Tickets
            .Where(t => t.status == status && t.AssignedTo.HasValue)
            .GroupBy(t => t.AssignedTo)
            .Select(g => new TopEmployeeDTO
            {
                UserId = g.Key.Value,
                FullName = _context.Users.FirstOrDefault(u => u.UserId == g.Key.Value).FullName,
                UserImage = _context.Users.FirstOrDefault(u => u.UserId == g.Key.Value).UserImage,
                ClosedTicketsCount = g.Count()
            })
            .OrderByDescending(e => e.ClosedTicketsCount)
            .Take(topCount)
            .ToList();

        if (!topEmployees.Any())
        {
            _logger.LogError("No employees with closed tickets found.");
            throw new InvalidOperationException("No employees with closed tickets found.");
        }

        return topEmployees;
    }


    public StatusCountDTO GetTicketCountsByStatus()
    {
        _logger.LogInformation("GetTicketCountsByStatus");
        var statusCounts = new StatusCountDTO
        {
            New = _context.Tickets.Count(t => t.status == Status.New),
            InProgress = _context.Tickets.Count(t => t.status == Status.InProgress),
            Resolved = _context.Tickets.Count(t => t.status == Status.Resolved),
            Closed = _context.Tickets.Count(t => t.status == Status.Closed),
            ReOpened = _context.Tickets.Count(t => t.status == Status.ReOpened)
        };

        if (statusCounts.New == 0 && statusCounts.InProgress == 0 && statusCounts.Resolved == 0 &&
            statusCounts.Closed == 0 && statusCounts.ReOpened == 0)
        {
            _logger.LogError("No tickets found for any status.");
            throw new InvalidOperationException("No tickets found for any status.");
        }

        return statusCounts;
    }

    public PriorityCountDTO GetTicketCountsByPriority()
    {
        _logger.LogInformation("GetTicketCountsByPriority");
        var priorityCounts = new PriorityCountDTO
        {
            Low = _context.Tickets.Count(t => t.Priority == Priorities.Low),
            Medium = _context.Tickets.Count(t => t.Priority == Priorities.Medium),
            High = _context.Tickets.Count(t => t.Priority == Priorities.High)
        };

        if (priorityCounts.Low == 0 && priorityCounts.Medium == 0 && priorityCounts.High == 0)
        {
            _logger.LogError("No tickets found for any priority.");
            throw new InvalidOperationException("No tickets found for any priority.");
        }

        return priorityCounts;
    }

    public IEnumerable<MonthlyTicketDTO> GetTicketsForCurrentYear()
    {
        _logger.LogInformation("GetTicketsForCurrentYear");
        if (_context == null)
        {
            _logger.LogError("Database context is not available.");
            throw new InvalidOperationException("Database context is not available.");
        }

        var currentYear = DateTime.UtcNow.Year;
        var result = new List<MonthlyTicketDTO>();

        for (int i = 1; i <= 12; i++)
        {
            var targetMonth = new DateTime(currentYear, i, 1);

            if (targetMonth.Year > currentYear)
            {
                throw new ArgumentException("Target month cannot be in the future.");
            }

            var ticketCount = _context.Tickets
                .Where(t => t.CreatedAt.HasValue &&
                            t.CreatedAt.Value.Year == currentYear &&
                            t.CreatedAt.Value.Month == i)
                .Count();

            result.Add(new MonthlyTicketDTO
            {
                Month = targetMonth.ToString("MMMM yyyy", CultureInfo.InvariantCulture),
                TicketCount = ticketCount
            });
        }

        if (!result.Any())
        {
            _logger.LogError("No tickets found for the current year.");
            throw new InvalidOperationException("No tickets found for the current year.");
        }

        if (result.All(r => r.TicketCount == 0))
        {
            _logger.LogError("All months have zero tickets.");
            throw new InvalidOperationException("All months have zero tickets.");
        }

        return result.OrderByDescending(r => DateTime.ParseExact(r.Month, "MMMM yyyy", CultureInfo.InvariantCulture));
    }


}
