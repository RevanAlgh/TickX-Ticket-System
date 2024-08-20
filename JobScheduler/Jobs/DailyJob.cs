using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;
using Ticket.Data.Models.Enums;
using Ticket.Service.DTOs;
using Ticket.Service.Interfaces;
using Ticket.Service.Repositories;

namespace Ticket.JobScheduler.Jobs;

[DisallowConcurrentExecution]
public class DailyJob : IJob
{
    private readonly ITicketClassRepository _ticketClassRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<DailyJob> _logger;

    public DailyJob(ITicketClassRepository ticketRepository, IEmailService emailService, ILogger<DailyJob> logger)
    {
        _ticketClassRepository = ticketRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Starting DailyJob at {Time}", DateTime.UtcNow);

        try
        {
            DateTime now = DateTime.UtcNow;

            var ticketsInProgress = await _ticketClassRepository.FindTicketsInProgressForReminderAsync(now);

            foreach (var ticket in ticketsInProgress)
            {
                if (ticket.ModifiedAt.HasValue)
                {
                    TimeSpan timeSinceModified = now - ticket.ModifiedAt.Value;
                    _logger.LogInformation($"Ticket {ticket.TicketId}: Time since modified: {timeSinceModified.TotalDays} days");

                    if (ticket.ReminderLevel == 0 && timeSinceModified.TotalDays >= 2)
                    {
                        await ProcessTicketsForReminder(ticket, "Ticket Reminder",
                            "Ticket has been inactive for 2 days. Please respond to keep it open.");
                        await _ticketClassRepository.UpdateTicketReminderLevelAsync(ticket.TicketId, 1); 
                    }
                    else if (ticket.ReminderLevel == 1 && timeSinceModified.TotalDays >= 3)
                    {
                        await ProcessTicketsForReminder(ticket, "Final Ticket Reminder",
                            "Ticket has been inactive for 3 days. It will be closed in 2 days if you don't respond.");
                        await _ticketClassRepository.UpdateTicketReminderLevelAsync(ticket.TicketId, 2);  
                    }
                    else if (ticket.ReminderLevel == 2 && timeSinceModified.TotalDays >= 5)
                    {
                        await ProcessTicketsForClosure(ticket);
                    }
                }
                else
                {
                    _logger.LogWarning($"Ticket {ticket.TicketId} has no ModifiedAt value, skipping.");
                }
            }

            _logger.LogInformation("DailyJob completed successfully at {Time}", DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DailyJob");
            throw;
        }
    }

    private async Task ProcessTicketsForReminder(TicketReminderDTO ticket, string subject, string message)
    {
        _logger.LogInformation($"Sending reminder to {ticket.Email} for ticket {ticket.TicketId}.");

        try
        {
            await _emailService.SendEmail(new EmailDTO
            {
                Email = ticket.Email,
                Subject = subject,
                Message = $"{message} (Ticket {ticket.TicketId})"
            });
            _logger.LogInformation($"Reminder email sent to {ticket.Email} for ticket {ticket.TicketId}.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send reminder to {ticket.Email} for ticket {ticket.TicketId}.");
        }
    }

    private async Task ProcessTicketsForClosure(TicketReminderDTO ticket)
    {
        _logger.LogInformation($"Closing ticket {ticket.TicketId} and sending closure email to {ticket.Email}.");

        try
        {
            await _ticketClassRepository.UpdateTicketStatusAsync(ticket.TicketId, Status.Closed);
            await _emailService.SendEmail(new EmailDTO
            {
                Email = ticket.Email,
                Subject = "Ticket Closed",
                Message = $"Ticket {ticket.TicketId} has been automatically closed due to inactivity."
            });
            _logger.LogInformation($"Ticket {ticket.TicketId} closed and closure email sent to {ticket.Email}.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to close ticket {ticket.TicketId} or send closure email to {ticket.Email}.");
        }
    }
}

