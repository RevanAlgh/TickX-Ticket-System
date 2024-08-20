using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Service.DTOs;
using Ticket.Service.Models;

namespace Ticket.Service.Interfaces;

public interface IViewTeamMemberRepository
{
    IEnumerable<ManagerDTO> GetAllTeamMembers();
    IEnumerable<ManagerDTO> GetActiveTeamMembers();
    ManagerDTO? GetTeamMemberById(int userId);
    void UpdateTeamMember(EditManagerDTO managerDto);
    void UpdateTeamMemberActivation(UpdateActivationDTO isActive);
    void Save();
}
