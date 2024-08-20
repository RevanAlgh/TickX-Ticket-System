import { Status } from './enums/Status';
import { Types } from './enums/Types';
import { Product } from './Product';
import { User } from './User';

export class Attachment {
  attachmentId: number;
  ticketId: number;
  fileName: string;
  type: Types;
  filePath: string;
  createdAt: Date;
  content?: string;
  ticket: Ticket;

  constructor(
    attachmentId: number,
    ticketId: number,
    fileName: string,
    type: Types,
    filePath: string,
    createdAt: Date,
    content: string | undefined,
    ticket: Ticket
  ) {
    this.attachmentId = attachmentId;
    this.ticketId = ticketId;
    this.fileName = fileName;
    this.type = type;
    this.filePath = filePath;
    this.createdAt = createdAt;
    this.content = content;
    this.ticket = ticket;
  }
}

// Assuming the Ticket class is already defined
class Ticket {
  ticketId: number;
  productId: number;
  ticketDescription: string;
  title: string;
  attachment: string;
  status: Status;
  closedAt: Date;
  createdAt: Date;
  modifiedAt: Date;
  closedBy: number;
  assignedTo: number;
  priority: number;
  createdBy?: number;
  product: Product;
  user?: User;
  comments: Comment[] = [];
  attachments: Attachment[] = [];

  constructor(
    ticketId: number,
    productId: number,
    ticketDescription: string,
    title: string,
    attachment: string,
    status: Status,
    closedAt: Date,
    createdAt: Date,
    modifiedAt: Date,
    closedBy: number,
    assignedTo: number,
    priority: number,
    createdBy: number | undefined,
    product: Product,
    user: User | undefined,
    comments: Comment[],
    attachments: Attachment[]
  ) {
    this.ticketId = ticketId;
    this.productId = productId;
    this.ticketDescription = ticketDescription;
    this.title = title;
    this.attachment = attachment;
    this.status = status;
    this.closedAt = closedAt;
    this.createdAt = createdAt;
    this.modifiedAt = modifiedAt;
    this.closedBy = closedBy;
    this.assignedTo = assignedTo;
    this.priority = priority;
    this.createdBy = createdBy;
    this.product = product;
    this.user = user;
    this.comments = comments;
    this.attachments = attachments;
  }
}
