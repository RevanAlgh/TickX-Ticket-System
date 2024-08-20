export class Ticket {
  ticketId: number;
  productId: number;
  ticketDescription: string;
  title: string;
  attachment: string[];
  fileName: string[];
  status: number;
  closedAt: string;
  createdAt: string;
  modifiedAt: string;
  closedBy: number;
  assignedTo: number;
  priority: number;
  createdBy: number;
  userId: number;
  userName: string;

  constructor() {
    this.ticketId = 0;
    this.productId = 0;
    this.ticketDescription = '';
    this.title = '';
    this.attachment = [];
    this.status = 0;
    this.closedAt = '0001-01-01T00:00:00';
    this.createdAt = '';
    this.modifiedAt = '';
    this.closedBy = 0;
    this.assignedTo = 0;
    this.priority = 0;
    this.createdBy = 0;
    this.userId = 0;
    this.userName = '';
    this.fileName = [];
  }
}
