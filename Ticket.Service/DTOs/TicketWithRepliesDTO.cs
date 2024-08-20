using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket.Service.DTOs;

public class TicketWithRepliesDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<AttachmentDTO> Attachments { get; set; }
    public List<ReplyDTO> Replies { get; set; }
}

public class AttachmentDTO
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
}

public class ReplyDTO
{
    public int UserId { get; set; }
    public string Content { get; set; }
    public string UserImage { get; set; } 
}

