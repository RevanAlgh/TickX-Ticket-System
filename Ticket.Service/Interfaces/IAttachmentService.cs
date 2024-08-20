using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Service.DTOs;
using Ticket.Data.Models.Enums;

namespace Ticket.Service.Interfaces
{
    public interface IAttachmentService
    {
         string UploadUserProfilePicture(CreateUserAttachmentDTO dto);
         string UploadTicketAttachment(CreateTicketAttachmentDTO dto);
         void ValidateFile(string fileName, string base64File);
    }
}
