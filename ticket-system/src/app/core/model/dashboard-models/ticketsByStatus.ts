export class TicketsByStatus {
  new: number;
  inProgress: number;
  resolved: number;
  closed: number;
  reOpened: number;

  constructor() {
    this.new = 0;
    this.inProgress = 2;
    this.resolved = 2;
    this.closed = 4;
    this.reOpened = 0;
  }
}
