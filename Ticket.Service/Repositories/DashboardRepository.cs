using System;
using System.Collections.Generic;
using System.Linq;
using Ticket.Data.Models.Data;
using Ticket.Data.Models.Enums;
using Ticket.Service.DTOs;
using Ticket.Service.Interfaces;
using Ticket.Service.Models;
using Ticket.Data.Models;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Ticket.Service.Repositories;


public class DashboardRepository : IDashboardRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DashboardRepository> _logger;

    public DashboardRepository(ApplicationDbContext context, ILogger<DashboardRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public TotalTicketsDTO GetDashboardStatistics(int userId)
    {
        _logger.LogInformation($"GetDashboardStatistics: Request Body {userId}");
        if (userId <= 0)
        {
            _logger.LogError("Invalid User ID.");
            throw new ArgumentException("Invalid User ID.");
        }

        IQueryable<Data.Models.Ticket> query;

        bool isEmployee = _context.Tickets.Any(t => t.AssignedTo == userId);
        bool isClient = _context.Tickets.Any(t => t.CreatedBy == userId);

        if (isEmployee)
        {
            // Employee-specific query
            query = _context.Tickets.Where(t => t.AssignedTo == userId);
        }
        else if (isClient)
        {
            // Client-specific query
            query = _context.Tickets.Where(t => t.CreatedBy == userId);
        }
        else
        {
            _logger.LogError("User ID does not match any ticket records.");
            throw new InvalidOperationException("User ID does not match any ticket records.");
        }

        int totalTickets = query.Count();

        var ticketCount = new TotalTicketsDTO
        {
            TotalTickets = totalTickets
        };

        return ticketCount;
    }


    public IEnumerable<MonthlyCountDTO> GetTicketsByMonth(int userId, int? month, int? year)
    {
        _logger.LogInformation($"GetTicketsByMonth: Request Body {userId}, {month}, {year}");
        if (userId <= 0)
        {
            _logger.LogError("Invalid User ID.");
            throw new ArgumentException("Invalid User ID.");
        }

        if (!month.HasValue && !year.HasValue)
        {
            _logger.LogError("Either month or year must be specified.");
            throw new ArgumentException("Either month or year must be specified.");
        }

        // Determine if the user is a client (CreatedBy) or an employee (AssignedTo)
        bool isClient = _context.Tickets.Any(t => t.CreatedBy == userId);
        bool isEmployee = _context.Tickets.Any(t => t.AssignedTo == userId);

        if (!isClient && !isEmployee)
        {
            _logger.LogError("User ID does not match any ticket records.");
            throw new InvalidOperationException("User ID does not match any ticket records.");
        }

        IQueryable<Data.Models.Ticket> query;

        if (isEmployee)
        {
            // Employee-specific query
            query = _context.Tickets.Where(t => t.AssignedTo == userId);
        }
        else
        {
            // Client-specific query
            query = _context.Tickets.Where(t => t.CreatedBy == userId);
        }

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



    public IEnumerable<YearlyCountDTO> GetTicketsByYear(int userId, int? year)
    {
        _logger.LogInformation($"GetTicketsByYear: Request Body {userId}, {year}");
        if (userId <= 0)
        {
            _logger.LogError("Invalid User ID.");
            throw new ArgumentException("Invalid User ID.");
        }

        if (!year.HasValue)
        {
            _logger.LogError("Year must be specified.");
            throw new ArgumentException("Year must be specified.");
        }

        // Determine if the user is a client (CreatedBy) or an employee (AssignedTo)
        bool isClient = _context.Tickets.Any(t => t.CreatedBy == userId);
        bool isEmployee = _context.Tickets.Any(t => t.AssignedTo == userId);

        if (!isClient && !isEmployee)
        {
            _logger.LogError("User ID does not match any ticket records.");
            throw new InvalidOperationException("User ID does not match any ticket records.");
        }

        IQueryable<Data.Models.Ticket> query;

        if (isEmployee)
        {
            // Employee-specific query
            query = _context.Tickets.Where(t => t.AssignedTo == userId);
        }
        else
        {
            // Client-specific query
            query = _context.Tickets.Where(t => t.CreatedBy == userId);
        }

        query = query.Where(t => t.CreatedAt.HasValue && t.CreatedAt.Value.Year == year.Value);

        if (!query.Any())
        {
            _logger.LogError("No tickets found for the specified year.");
            throw new InvalidOperationException("No tickets found for the specified year.");
        }

        return query
            .GroupBy(t => t.CreatedAt.Value.Year)
            .Select(g => new YearlyCountDTO
            {
                Year = g.Key,
                TicketsCount = g.Count()
            }).ToList();
    }


    public TotalTicketsDTO GetTotalTickets(int userId, Status? status = null, Priorities? priority = null)
    {
        _logger.LogInformation($"GetTotalTickets: Request Body {userId}");
        if (userId <= 0)
        {
            _logger.LogError("Invalid User ID.");
            throw new ArgumentException("Invalid User ID.");
        }

        bool isClient = _context.Tickets.Any(t => t.CreatedBy == userId);
        bool isEmployee = _context.Tickets.Any(t => t.AssignedTo == userId);

        if (!isClient && !isEmployee)
        {
            _logger.LogError("User ID does not match any ticket records.");
            throw new InvalidOperationException("User ID does not match any ticket records.");
        }

        IQueryable<Data.Models.Ticket> query;

        if (isEmployee)
        {
            // Employee-specific query
            query = _context.Tickets.Where(t => t.AssignedTo == userId);
        }
        else
        {
            // Client-specific query
            query = _context.Tickets.Where(t => t.CreatedBy == userId);
        }

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
            _logger.LogError("No tickets found for the specified criteria.");
            throw new InvalidOperationException("No tickets found for the specified criteria.");
        }

        return new TotalTicketsDTO
        {
            TotalTickets = ticketCount
        };
    }

    public TotalTicketsDTO GetTotalTicketsByStatus(int userId, Status? status = null)
    {
        _logger.LogInformation($"GetTotalTicketsByStatus: Request Body {userId}");
        if (userId <= 0)
        {
            _logger.LogError("Invalid User ID.");
            throw new ArgumentException("Invalid User ID.");
        }

        // Determine if the user is a client (CreatedBy) or an employee (AssignedTo)
        bool isClient = _context.Tickets.Any(t => t.CreatedBy == userId);
        bool isEmployee = _context.Tickets.Any(t => t.AssignedTo == userId);

        if (!isClient && !isEmployee)
        {
            _logger.LogError("User ID does not match any ticket records.");
            throw new InvalidOperationException("User ID does not match any ticket records.");
        }

        IQueryable<Data.Models.Ticket> query;

        if (isEmployee)
        {
            // Employee-specific query
            query = _context.Tickets.Where(t => t.AssignedTo == userId);
        }
        else
        {
            // Client-specific query
            query = _context.Tickets.Where(t => t.CreatedBy == userId);
        }

        if (status.HasValue)
        {
            query = query.Where(t => t.status == status.Value);
        }

        var ticketCount = query.Count();

        if (ticketCount == 0)
        {
            _logger.LogError("No tickets found for the specified criteria.");
            throw new InvalidOperationException("No tickets found for the specified criteria.");
        }

        return new TotalTicketsDTO
        {
            TotalTickets = ticketCount
        };
    }

    public TotalTicketsDTO GetTotalTicketsByPriorities(int userId,Priorities? priority = null)
    {
        _logger.LogInformation($"GetTotalTicketsByPriorities: Request Body {userId}");
        if (userId <= 0)
        {
            _logger.LogError("Invalid User ID.");
            throw new ArgumentException("Invalid User ID.");
        }

        // Determine if the user is a client (CreatedBy) or an employee (AssignedTo)
        bool isClient = _context.Tickets.Any(t => t.CreatedBy == userId);
        bool isEmployee = _context.Tickets.Any(t => t.AssignedTo == userId);

        if (!isClient && !isEmployee)
        {
            _logger.LogError("User ID does not match any ticket records.");
            throw new InvalidOperationException("User ID does not match any ticket records.");
        }

        IQueryable<Data.Models.Ticket> query;

        if (isEmployee)
        {
            // Employee-specific query
            query = _context.Tickets.Where(t => t.AssignedTo == userId);
        }
        else
        {
            // Client-specific query
            query = _context.Tickets.Where(t => t.CreatedBy == userId);
        }

        if (priority.HasValue)
        {
            query = query.Where(t => t.Priority == priority.Value);
        }

        var ticketCount = query.Count();

        if (ticketCount == 0)
        {
            _logger.LogError("No tickets found for the specified criteria.");
            throw new InvalidOperationException("No tickets found for the specified criteria.");
        }

        return new TotalTicketsDTO
        {
            TotalTickets = ticketCount
        };
    }

    public StatusCountDTO GetTicketCountsByStatus(int userId)
    {
        _logger.LogInformation($"GetTicketCountsByStatus: Request Body {userId}");
        if (userId <= 0)
        {
            _logger.LogError("Invalid User ID.");
            throw new ArgumentException("Invalid User ID.");
        }

        IQueryable<Data.Models.Ticket> query;

        if (_context.Tickets.Any(t => t.AssignedTo == userId))
        {
            // Employee-specific query
            query = _context.Tickets.Where(t => t.AssignedTo == userId);
        }
        else if (_context.Tickets.Any(t => t.CreatedBy == userId))
        {
            // Client-specific query
            query = _context.Tickets.Where(t => t.CreatedBy == userId);
        }
        else
        {
            _logger.LogError("User ID does not match any ticket records.");
            throw new InvalidOperationException("User ID does not match any ticket records.");
        }

        var statusCounts = new StatusCountDTO
        {
            New = query.Count(t => t.status == Status.New),
            InProgress = query.Count(t => t.status == Status.InProgress),
            Resolved = query.Count(t => t.status == Status.Resolved),
            Closed = query.Count(t => t.status == Status.Closed),
            ReOpened = query.Count(t => t.status == Status.ReOpened)
        };

        if (statusCounts.New == 0 && statusCounts.InProgress == 0 && statusCounts.Resolved == 0 &&
            statusCounts.Closed == 0 && statusCounts.ReOpened == 0)
        {
            _logger.LogError("No tickets found for any status.");
            throw new InvalidOperationException("No tickets found for any status.");
        }

        return statusCounts;
    }

    public PriorityCountDTO GetTicketCountsByPriority(int userId)
    {
        _logger.LogInformation($"GetTicketCountsByPriority: Request Body {userId}");
        if (userId <= 0)
        {
            _logger.LogError("Invalid User ID.");
            throw new ArgumentException("Invalid User ID.");
        }

        IQueryable<Data.Models.Ticket> query;

        if (_context.Tickets.Any(t => t.AssignedTo == userId))
        {
            // Employee-specific query
            query = _context.Tickets.Where(t => t.AssignedTo == userId);
        }
        else if (_context.Tickets.Any(t => t.CreatedBy == userId))
        {
            // Client-specific query
            query = _context.Tickets.Where(t => t.CreatedBy == userId);
        }
        else
        {
            _logger.LogError("User ID does not match any ticket records.");
            throw new InvalidOperationException("User ID does not match any ticket records.");
        }

        var priorityCounts = new PriorityCountDTO
        {
            Low = query.Count(t => t.Priority == Priorities.Low),
            Medium = query.Count(t => t.Priority == Priorities.Medium),
            High = query.Count(t => t.Priority == Priorities.High)
        };

        if (priorityCounts.Low == 0 && priorityCounts.Medium == 0 && priorityCounts.High == 0)
        {
            _logger.LogError("No tickets found for any priority.");
            throw new InvalidOperationException("No tickets found for any priority.");
        }

        return priorityCounts;
    }

    public IEnumerable<RecentTicketDTO> GetRecentTicketsForClient(RecentTicketsRequestDTO request)
    {
        _logger.LogInformation($"GetRecentTicketsForClient: Request Body {request.UserId}, {request.TicketCount}");

        if (request.UserId <= 0)
        {
            _logger.LogError("Invalid User ID.");
            throw new ArgumentException("Invalid User ID.");
        }

        var recentTickets = _context.Tickets
            .Where(t => t.CreatedBy == request.UserId)
            .OrderByDescending(t => t.CreatedAt)
            .Take(request.TicketCount)
            .Select(t => new RecentTicketDTO
            {
                TicketId = t.TicketId,
                ProductId = t.ProductId,
                Title = t.Title,
                TicketDescription = t.TicketDescription,
                CreatedAt = t.CreatedAt ?? DateTime.MinValue,
                ClosedAt = t.ClosedAt,
                ModifiedAt = t.ModifiedAt,
                ClosedBy = t.ClosedBy,
                AssignedTo = t.AssignedTo,
                Priority = (int)t.Priority, 
                Status = (int)t.status,
                ProductName = t.Product.Name,
                CreatedBy = t.CreatedBy ?? 0

            })
            .ToList();

        if (!recentTickets.Any())
        {
            _logger.LogError("No recent tickets found for the client.");
            throw new InvalidOperationException("No recent tickets found for the client.");
        }

        return recentTickets;
    }


}

