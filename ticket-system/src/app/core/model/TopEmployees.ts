export class TopEmployee {
  closedTicketsCount: number;
  fullName: string;
  userId: number;
  userImage: string | null;

  constructor() {
    this.closedTicketsCount = 0;
    this.fullName = '';
    this.userId = 0;
    this.userImage = '';
  }
}
