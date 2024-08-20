using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket.Data.Models.Enums
{
    public enum Status
    {
        New = 1,
        InProgress = 2,
        Resolved = 3,
        Closed = 4,
        ReOpened = 5
    }
}
