using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using Ticket.Service.Interfaces;
using Ticket.Service.Interfaces;
using Ticket.Service.DTOs;
using Ticket.Service.Repositories;
using Ticket.Data.Models.Enums;
using System.Data;
using Ticket.Data.Models.Data;
using Microsoft.EntityFrameworkCore;
using Ticket.Data.Models;
using Microsoft.IdentityModel.Tokens;

namespace Ticket.Service.Repositories
{
    public class TicketClassRepository : ITicketClassRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TicketClassRepository> _logger;
        private readonly IAttachmentService _attachmentService;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        public TicketClassRepository(ApplicationDbContext context, ILogger<TicketClassRepository> logger, IAttachmentService attachmentService, IEmailService emailService, IUserService userService)
        {
            _context = context;
            _logger = logger;
            _attachmentService = attachmentService;
            _emailService = emailService;
            _userService = userService;
        }

        public async Task AddTicket(CreateTicketDTO ticketDto)
        {
            _logger.LogInformation($"AddTicket: Request Body {ticketDto}");
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var product = await _context.Products.FindAsync(ticketDto.ProductId);
                if (product == null)
                {
                    _logger.LogError("Product not found.");
                    throw new Exception("Product not found.");
                }

                if (ticketDto.CreatedBy.HasValue)
                {
                    var user = await _context.Users.FindAsync(ticketDto.CreatedBy.Value);
                    if (user == null)
                    {
                        _logger.LogError("CreatedBy user not found.");
                        throw new Exception("CreatedBy user not found.");
                    }
                }
                else
                {
                    _logger.LogError("CreatedBy is required.");
                    throw new Exception("CreatedBy is required.");
                }

                if (ticketDto.Attachment != null && ticketDto.FileName != null && ticketDto.Attachment.Count == ticketDto.FileName.Count)
                {
                    for (int i = 0; i < ticketDto.Attachment.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(ticketDto.Attachment[i]) && !string.IsNullOrEmpty(ticketDto.FileName[i]))
                        {
                            try
                            {
                                _attachmentService.ValidateFile(ticketDto.FileName[i], ticketDto.Attachment[i]);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError("Image validation failed: " + ex.Message);
                                throw new Exception("Image validation failed: " + ex.Message);
                            }
                        }
                    }
                }

                var ticket = new Data.Models.Ticket
                {
                    ProductId = ticketDto.ProductId,
                    TicketDescription = ticketDto.TicketDescription,
                    Title = ticketDto.Title,
                    status = ticketDto.status,
                    CreatedBy = ticketDto.CreatedBy,
                    Priority = ticketDto.Priority,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    ClosedAt = null
                };

                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();

                if (ticketDto.Attachment != null && ticketDto.FileName != null && ticketDto.Attachment.Count == ticketDto.FileName.Count)
                {
                    for (int i = 0; i < ticketDto.Attachment.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(ticketDto.Attachment[i]) && !string.IsNullOrEmpty(ticketDto.FileName[i]))
                        {
                            var createTicketAttachmentDTO = new CreateTicketAttachmentDTO
                            {
                                TicketId = ticket.TicketId,
                                FileName = ticketDto.FileName[i],
                                Base64File = ticketDto.Attachment[i]
                            };

                            try
                            {
                                _attachmentService.UploadTicketAttachment(createTicketAttachmentDTO);
                                await _context.SaveChangesAsync();
                            }
                            catch (Exception ex)
                            {
                                await transaction.RollbackAsync();
                                await _context.SaveChangesAsync();
                                _logger.LogError("Failed to upload image: " + ex.Message);
                                throw new Exception("Failed to upload image: " + ex.Message);
                            }
                        }
                    }
                }

                await transaction.CommitAsync();
                var userData = await _userService.GetUserByIdAsync(ticketDto.CreatedBy.GetValueOrDefault());
                var emailDTO = new EmailDTO
                {
                    Email = userData.Email,
                    Subject = "New Ticket",
                    Message = "A new ticket has been created for you"
                };
                await _emailService.SendEmail(emailDTO);
                _logger.LogInformation("End of AddTicket");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while adding a ticket: {ex.Message}");
                throw new Exception($"Error occurred while adding a ticket: {ex.Message}");
            }
        }


        public IEnumerable<TicketDTO> GetAllTickets()
        {
            _logger.LogInformation("GetAllTickets");
            try
            {
                return _context.Tickets
                    .Select(t => new TicketDTO
                    {
                        TicketId = t.TicketId,
                        ProductId = t.ProductId,
                        TicketDescription = t.TicketDescription,
                        Title = t.Title,
                        FileName = t.Attachments.Select(a => a.FileName).ToList(),
                        Attachment = t.Attachments.Select(a => a.Content).ToList(),
                        status = t.status,
                        ClosedAt = t.ClosedAt,
                        CreatedAt = t.CreatedAt,
                        ModifiedAt = t.ModifiedAt,
                        ClosedBy = t.ClosedBy,
                        AssignedTo = t.AssignedTo,
                        Priority = t.Priority,
                        CreatedBy = t.CreatedBy,
                        UserId = t.User.UserId,
                        UserName = t.User.UserName

                    }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }
        }

        public TicketDTO GetTicketById(int ticketId)
        {
            _logger.LogInformation($"GetTicketById: {ticketId}");
            try
            {
                var ticket = _context.Tickets
                    .Where(t => t.TicketId == ticketId)
                    .Select(t => new TicketDTO
                    {
                        TicketId = t.TicketId,
                        ProductId = t.ProductId,
                        TicketDescription = t.TicketDescription,
                        Title = t.Title,
                        FileName = t.Attachments.Select(a => a.FileName).ToList(),
                        Attachment = t.Attachments.Select(a => a.Content).ToList(),
                        status = t.status,
                        ClosedAt = t.ClosedAt,
                        CreatedAt = t.CreatedAt,
                        ModifiedAt = t.ModifiedAt,
                        ClosedBy = t.ClosedBy,
                        AssignedTo = t.AssignedTo,
                        Priority = t.Priority,
                        CreatedBy = t.CreatedBy,
                        UserId = t.User.UserId,
                        UserName = t.User.UserName
                    })
                    .FirstOrDefault();

                if (ticket == null)
                {
                    throw new KeyNotFoundException($"Ticket with ID {ticketId} not found.");
                }

                return ticket;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetTicketById: {ex.Message}");
                throw;
            }
        }


        public void UpdateTicket(EditTicketDTO ticketDto)
        {
            _logger.LogInformation($"UpdateTicket: Request Body {ticketDto}");
            var ticket = _context.Tickets.FirstOrDefault(t => t.TicketId == ticketDto.TicketId);
            if (ticket == null)
            {
                _logger.LogError($"Ticket with ID {ticketDto.TicketId} not found.");
                throw new Exception($"Ticket with ID {ticketDto.TicketId} not found.");
            }

            ticket.status = ticketDto.status;
            ticket.ModifiedAt = DateTime.Now;

            if (ticketDto.Attachment != null && ticketDto.FileName != null &&
                ticketDto.Attachment.Count == ticketDto.FileName.Count &&
                ticketDto.Attachment.All(a => !string.IsNullOrEmpty(a)) &&
                ticketDto.FileName.All(f => !string.IsNullOrEmpty(f)))
            {
                var existingAttachments = _context.Attachments.Where(a => a.TicketId == ticket.TicketId).ToList();
                _context.Attachments.RemoveRange(existingAttachments);

                for (int i = 0; i < ticketDto.Attachment.Count; i++)
                {
                    _attachmentService.ValidateFile(ticketDto.FileName[i], ticketDto.Attachment[i]);

                    var createTicketAttachmentDTO = new CreateTicketAttachmentDTO
                    {
                        TicketId = ticket.TicketId,
                        FileName = ticketDto.FileName[i],
                        Base64File = ticketDto.Attachment[i]
                    };
                    _attachmentService.UploadTicketAttachment(createTicketAttachmentDTO);
                }
            }
            else if (ticketDto.Attachment != null || ticketDto.FileName != null)
            {
                if (ticketDto.Attachment.Any(a => !string.IsNullOrEmpty(a)) || ticketDto.FileName.Any(f => !string.IsNullOrEmpty(f)))
                {
                    _logger.LogError("Both Attachment and FileName must be non-empty to update attachments.");
                    throw new Exception("Both Attachment and FileName must be non-empty to update attachments.");
                }
            }

            _context.Tickets.Update(ticket);
            _context.SaveChanges();
        }
        public TicketDTO GetTicketsForUser(int userId)
        {
            try
            {
                return _context.Tickets
                    .Where(t => t.User.UserId == userId)
                    .Select(t => new TicketDTO
                    {
                        TicketId = t.TicketId,
                        ProductId = t.ProductId,
                        TicketDescription = t.TicketDescription,
                        Title = t.Title,
                        FileName = t.Attachments.Select(a => a.FileName).ToList(),
                        Attachment = t.Attachments.Select(a => a.Content).ToList(),
                        status = t.status,
                        ClosedAt = t.ClosedAt,
                        CreatedAt = t.CreatedAt,
                        ModifiedAt = t.ModifiedAt,
                        ClosedBy = t.ClosedBy,
                        AssignedTo = t.AssignedTo,
                        Priority = t.Priority,
                        CreatedBy = t.CreatedBy,
                        UserId = t.User.UserId,
                        UserName = t.User.UserName
                    }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }
        }
        public async Task<string> AssignTicket(AssignTicketDTO assignTicketDto)
        {
            _logger.LogInformation($"AssignTicket: Request Body {assignTicketDto}");
            try
            {
                var ticket = await _context.Tickets.FindAsync(assignTicketDto.TicketId);
                if (ticket == null)
                {
                    _logger.LogError("Ticket not found.");
                    throw new Exception("Ticket not found.");
                }

                var user = await _context.Users.FindAsync(assignTicketDto.AssignedTo);
                if (user == null)
                {
                    _logger.LogError("User not found.");
                    throw new Exception("User not found.");
                }

                //check if manager can get a ticket
                if (user.Role == Roles.SupportManager)
                {
                    _logger.LogError("Cannot assign ticket to Manager.");
                    throw new Exception("Cannot assign ticket to Manager.");
                }

                if (user.Role != Roles.TeamMember)
                {
                    _logger.LogError("Cannot assign ticket to client.");
                    throw new Exception("Cannot assign ticket to client.");
                }

                bool isReassigned = ticket.AssignedTo != assignTicketDto.AssignedTo && ticket.AssignedTo != 0;
                ticket.AssignedTo = assignTicketDto.AssignedTo;
                ticket.ModifiedAt = DateTime.Now;

                ticket.status = Status.InProgress;

                await _context.SaveChangesAsync();
                //var emailDTO = new EmailDTO
                //{
                //    Email = user.Email,
                //    Subject = "Assign To",
                //    Message = "A ticket has been assigned to you"
                //};
                //await _emailService.SendEmail(emailDTO);
                return isReassigned ? "Ticket reassigned successfully" : "Ticket assigned successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TicketDTO>> GetTicketsByClient(int clientId)
        {
            _logger.LogInformation($"GetTicketsByClient: Request Body {clientId}");
            try
            {
                _logger.LogError($"Checking existence of user with ID: {clientId}");
                var user = await _context.Users.FindAsync(clientId);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {clientId} not found.");
                    throw new Exception("User not found.");
                }

                if (user.Role != Roles.Client)
                {
                    _logger.LogWarning($"User with ID {clientId} is not a client. Role: {user.Role}");
                    throw new Exception("User is not a client.");
                }

                var tickets = await _context.Tickets
                    .Where(t => t.CreatedBy == clientId)
                    .Select(t => new TicketDTO
                    {
                        TicketId = t.TicketId,
                        ProductId = t.ProductId,
                        TicketDescription = t.TicketDescription,
                        Title = t.Title,
                        FileName = t.Attachments.Select(a => a.FileName).ToList(),
                        Attachment = t.Attachments.Select(a => a.Content).ToList(),
                        status = t.status,
                        ClosedAt = t.ClosedAt,
                        CreatedAt = t.CreatedAt,
                        ModifiedAt = t.ModifiedAt,
                        ClosedBy = t.ClosedBy,
                        AssignedTo = t.AssignedTo,
                        Priority = t.Priority,
                        CreatedBy = t.CreatedBy,
                        UserId = t.User.UserId,
                        UserName = t.User.UserName
                    })
                    .ToListAsync();
                return tickets;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }
        }
        public async Task<Status?> GetTicketStatus(int ticketId)
        {
            _logger.LogInformation($"GetTicketStatus: Request Body {ticketId}");
            try
            {
                var ticketExists = await _context.Tickets.AnyAsync(t => t.TicketId == ticketId);

                if (!ticketExists)
                {
                    return null;
                }

                var ticket = await _context.Tickets
                .Where(t => t.TicketId == ticketId)
                .Select(t => t.status)
                .FirstOrDefaultAsync();

                return ticket;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateTicketStatus(ChangeStatusDTO changeStatusDTO)
        {
            _logger.LogInformation($"UpdateTicketStatus: Request Body {changeStatusDTO}");
            try
            {
                var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.TicketId == changeStatusDTO.TicketId);
                if (ticket == null)
                {
                    _logger.LogError($"Ticket with ID {changeStatusDTO.TicketId} not found.");
                    throw new KeyNotFoundException($"Ticket with ID {changeStatusDTO.TicketId} not found.");
                }

                if (!Enum.IsDefined(typeof(Status), changeStatusDTO.status))
                {
                    _logger.LogError($"Invalid status: {changeStatusDTO.status}. The status must be one of the following values: {string.Join(", ", Enum.GetNames(typeof(Status)))}.");
                    throw new ArgumentException($"Invalid status: {changeStatusDTO.status}. The status must be one of the following values: {string.Join(", ", Enum.GetNames(typeof(Status)))}.");
                }

                ticket.status = (Status)changeStatusDTO.status;
                await _context.SaveChangesAsync();
                //if (changeStatusDTO.status == Status.Closed)
                //{
                //    var user = GetUserByTicketId(changeStatusDTO.TicketId);
                //    var emailDTO = new EmailDTO
                //    {
                //        Email = user.Result.Email,
                //        Subject = "Ticket Closed",
                //        Message = $"This ticket ({changeStatusDTO.TicketId}) has been closed! thank you"
                //    };
                //    await _emailService.SendEmail(emailDTO);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating ticket status for ticket with ID {changeStatusDTO.TicketId}.");
                throw;
            }
        }

        public async Task UpdateTicketPriority(ChangePriorityDTO changePriorityDTO)
        {
            _logger.LogInformation($"UpdateTicketPriority: Request Body {changePriorityDTO}");
            try
            {
                var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.TicketId == changePriorityDTO.TicketId);
                if (ticket == null)
                {
                    _logger.LogError($"Ticket with ID {changePriorityDTO.TicketId} not found.");
                    throw new Exception($"Ticket with ID {changePriorityDTO.TicketId} not found.");
                }

                if (!Enum.IsDefined(typeof(Priorities), changePriorityDTO.priority))
                {
                    _logger.LogError($"Invalid priority: {changePriorityDTO.priority}. The priority must be one of the following values: {string.Join(", ", Enum.GetNames(typeof(Priorities)))}.");
                    throw new Exception($"Invalid priority: {changePriorityDTO.priority}. The priority must be one of the following values: {string.Join(", ", Enum.GetNames(typeof(Priorities)))}.");
                }

                ticket.Priority = (Priorities)changePriorityDTO.priority;
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating ticket priority for ticket with ID {changePriorityDTO.TicketId}.");
                throw;
            }
        }
        public async Task<IEnumerable<TicketDTO>> ListTicketsByEmployee(int assignedToUserId)
        {
            _logger.LogInformation($"ListTicketsByEmployee: Request Body {assignedToUserId}");
            try
            {
                _logger.LogInformation($"Checking existence of user with ID: {assignedToUserId}");
                var user = await _context.Users.FindAsync(assignedToUserId);
                if (user == null)
                {
                    _logger.LogError($"User with ID {assignedToUserId} not found.");
                    throw new Exception("User not found.");
                }

                if (user.Role != Roles.TeamMember)
                {
                    _logger.LogError($"User with ID {assignedToUserId} is not a employee. Role: {user.Role}");
                    throw new Exception("User is not a employee.");
                }
                try
                {
                    return _context.Tickets
                        .Where(t => t.AssignedTo == assignedToUserId)
                        .Select(t => new TicketDTO
                        {
                            TicketId = t.TicketId,
                            ProductId = t.ProductId,
                            TicketDescription = t.TicketDescription,
                            Title = t.Title,
                            FileName = t.Attachments.Select(a => a.FileName).ToList(),
                            Attachment = t.Attachments.Select(a => a.Content).ToList(),
                            status = t.status,
                            ClosedAt = t.ClosedAt,
                            CreatedAt = t.CreatedAt,
                            ModifiedAt = t.ModifiedAt,
                            ClosedBy = t.ClosedBy,
                            AssignedTo = t.AssignedTo,
                            Priority = t.Priority,
                            CreatedBy = t.CreatedBy,
                            UserId = t.User.UserId,
                            UserName = t.User.UserName
                        }).ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }
        }

        public TicketStatusCountsDTO GetTicketCountsForClientOrEmployee(int userId)
        {
            _logger.LogInformation($"GetTicketCountsForClientOrEmployee: Request Body {userId}");
            try
            {
                var user = _context.Users.Find(userId);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }


                bool isEmployee = user.Role == Roles.TeamMember;

                var ticketCounts = _context.Tickets
                    .Where(t => isEmployee ? t.AssignedTo == userId : t.CreatedBy == userId)
                    .GroupBy(t => t.status)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToList();

                if (!ticketCounts.Any())
                {
                    return null;
                }

                var result = new TicketStatusCountsDTO
                {
                    Total = ticketCounts.Sum(t => t.Count),
                    New = ticketCounts.FirstOrDefault(t => t.Status == Status.New)?.Count ?? 0,
                    InProgress = ticketCounts.FirstOrDefault(t => t.Status == Status.InProgress)?.Count ?? 0,
                    Closed = ticketCounts.FirstOrDefault(t => t.Status == Status.Closed)?.Count ?? 0
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while getting ticket counts for client/employee: {ex.Message}");
                throw;
            }
        }

        public TicketStatusCountsDTO GetTicketCountsForManager()
        {
            _logger.LogInformation("GetTicketCountsForManager");
            try
            {
                var ticketCounts = _context.Tickets
                    .GroupBy(t => t.status)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToList();

                if (!ticketCounts.Any())
                {
                    return null;
                }

                var result = new TicketStatusCountsDTO
                {
                    Total = ticketCounts.Sum(t => t.Count),
                    New = ticketCounts.FirstOrDefault(t => t.Status == Status.New)?.Count ?? 0,
                    InProgress = ticketCounts.FirstOrDefault(t => t.Status == Status.InProgress)?.Count ?? 0,
                    Closed = ticketCounts.FirstOrDefault(t => t.Status == Status.Closed)?.Count ?? 0
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while getting ticket count: {ex.Message}");
                throw;
            }
        }


        public async Task<IEnumerable<TicketDTO>> GetFilteredTickets(TicketFilterDTO filter)
        {
            _logger.LogInformation($"GetFilteredTickets: Request Body {filter}");
            try
            {
                var query = _context.Tickets.AsQueryable();

                if (filter.Priority.HasValue && (int)filter.Priority.Value != -1)
                {
                    query = query.Where(t => t.Priority == filter.Priority.Value);
                }

                if (filter.AssignedToUserId.HasValue && filter.AssignedToUserId != -1)
                {
                    query = query.Where(t => t.AssignedTo == filter.AssignedToUserId.Value);
                }

                if (filter.ClientId.HasValue && filter.ClientId != -1)
                {
                    query = query.Where(t => t.CreatedBy == filter.ClientId.Value);
                }

                if (filter.Status.HasValue && (int)filter.Status.Value != -1)
                {
                    query = query.Where(t => t.status == filter.Status.Value);
                }

                if (!string.IsNullOrEmpty(filter.Name))
                {
                    query = query.Where(t => t.Title.Contains(filter.Name));
                }

                return await query.Select(t => new TicketDTO
                {
                    TicketId = t.TicketId,
                    ProductId = t.ProductId,
                    TicketDescription = t.TicketDescription,
                    Title = t.Title,
                    FileName = t.Attachments.Select(a => a.FileName).ToList(),
                    Attachment = t.Attachments.Select(a => a.Content).ToList(),
                    status = t.status,
                    ClosedAt = t.ClosedAt,
                    CreatedAt = t.CreatedAt,
                    ModifiedAt = t.ModifiedAt,
                    ClosedBy = t.ClosedBy,
                    AssignedTo = t.AssignedTo,
                    Priority = t.Priority,
                    CreatedBy = t.CreatedBy,
                    UserId = t.User.UserId,
                    UserName = t.User.UserName
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }
        }
        public async Task<User?> GetUserByTicketId(int ticketId)
        {
            _logger.LogInformation($"GetUserByTicketId: Request Body {ticketId}");
            try
            {
                var ticket = await _context.Tickets
                    .Include(t => t.User)
                    .FirstOrDefaultAsync(t => t.TicketId == ticketId);

                if (ticket == null)
                {
                    return null;
                }

                return ticket.User;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while retrieving user for ticket with ID {ticketId}: {ex.Message}");
                throw;
            }
        }
        public void Save()
        {
            _logger.LogInformation("Save");
            _context.SaveChanges();
        }


        // For the JobScheduler:

        public async Task<IEnumerable<TicketReminderDTO>> FindTicketsInProgressForReminderAsync(DateTime now)
        {
            return await _context.Tickets
                .Where(t => t.status == Status.InProgress && t.ModifiedAt <= now)
                .Join(_context.Users,
                      ticket => ticket.CreatedBy,
                      user => user.UserId,
                      (ticket, user) => new TicketReminderDTO
                      {
                          TicketId = ticket.TicketId,
                          CreatedBy = ticket.CreatedBy,
                          Email = user.Email,
                          ModifiedAt = ticket.ModifiedAt,
                          Status = ticket.status,
                          ReminderLevel = ticket.ReminderLevel
                      })
                .ToListAsync();
        }
        public async Task UpdateTicketReminderLevelAsync(int ticketId, int reminderLevel)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.TicketId == ticketId);
            if (ticket != null)
            {
                ticket.ReminderLevel = reminderLevel;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateTicketStatusAsync(int ticketId, Status newStatus)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.TicketId == ticketId);
            if (ticket != null)
            {
                ticket.status = newStatus;
                await _context.SaveChangesAsync();
            }
        }

    }
}
