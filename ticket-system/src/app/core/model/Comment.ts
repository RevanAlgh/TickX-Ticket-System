export class Comment {
  commentId?: number;
  ticketId: number;
  userId: number;
  replie: string;
  createAt?: string;
  fullName?: string;

  constructor() {
    this.ticketId = 0;
    this.userId = 0;
    this.replie = '';
  }
}
