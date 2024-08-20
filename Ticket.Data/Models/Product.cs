using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket.Data.Models
{
    public class Product
    {   
        public int ProductId { get; set; }
        public string Name { get; set; }
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
