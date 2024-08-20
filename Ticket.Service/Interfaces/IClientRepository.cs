using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Service.DTOs;
using Ticket.Service.Models;

namespace Ticket.Service.Interfaces;

public interface IClientRepository
{
    IEnumerable<ClientWithTicketCountDTO> GetAllClients();
    ManagerDTO? GetClientById(int userId);
    void UpdateClient(EditManagerDTO managerDto);
    void UpdateClientActivation(UpdateActivationDTO isActive);
    void Save();
}
