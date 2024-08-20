using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Service.DTOs;

namespace Ticket.Service.Interfaces;

public interface ITicketRepository
{
    TicketWithRepliesDTO GetTicketWithReplies(int ticketId);
}
