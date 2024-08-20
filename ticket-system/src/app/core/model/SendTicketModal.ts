export class SendTicketModel {
  productId: number;
  ticketDescription: string;
  title: string;
  attachment: string[];
  fileName: string[];
  status: number;
  priority: number;
  createdBy: number;

  constructor() {
    this.productId = 0;
    this.ticketDescription = '';
    this.title = '';
    this.attachment = [];
    this.fileName = [];
    this.status = 0;
    this.priority = 0;
    this.createdBy = 0;
  }
}
