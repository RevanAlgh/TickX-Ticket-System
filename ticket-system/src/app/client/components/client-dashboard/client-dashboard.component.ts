import { Component } from '@angular/core';
import { RouterLink, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { ViewTicketComponent } from '../view-ticket/view-ticket.component';
import { AddTicketComponent } from '../add-ticket/add-ticket.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TicketsByStatus } from '../../../core/model/dashboard-models/ticketsByStatus';
import { AuthService } from '../../../core/services/auth.service';
import { DashboardService } from '../../../shared/services/dashboard.service';
import { APIResponse } from '../../../core/model/APIResponse';
import { Product } from '../../../core/model/Product';
import { Ticket } from '../../../core/model/Ticket';
import { TicketServiceService } from '../../../shared/services/ticket-service.service';
import { StatusPipe } from '../../../shared/pipes/status.pipe';
import { PriorityPipe } from '../../../shared/pipes/priority.pipe';

@Component({
  selector: 'app-client-dashboard',
  standalone: true,
  imports: [
    TranslateModule,
    RouterLink,
    ViewTicketComponent,
    AddTicketComponent,
    CommonModule,
    FormsModule,
    StatusPipe,
    PriorityPipe,
  ],
  templateUrl: './client-dashboard.component.html',
  styleUrl: '../../../admin/components/dashboard/dashboard.component.css',
})
export class ClientDashboardComponent {
  ticketPerMonth!: { ticketsCount: 0; month: 0 };

  ticketsOfYear!: number | 0;
  ticketsByPriority: any | 0;
  ticketsByStatus: TicketsByStatus = {
    closed: 0,
    reOpened: 0,
    new: 0,
    inProgress: 0,
    resolved: 0,
  };

  tickets: Ticket[] = [];
  products: Product[] = [];

  constructor(
    private auth: AuthService,
    private dashService: DashboardService,
    private ticketService: TicketServiceService
  ) {}
  ngOnInit(): void {
    this.loadTicketsOfMonth();
    this.getTicketsGroupByStat();
    this.loadTicketsPerYear();
    this.loadRecentTickets();
  }

  loadRecentTickets() {
    this.ticketService
      .loadRecentTickets(this.auth.getUserID())
      .subscribe((res) => {
        this.tickets = res.data;
        console.log(this.tickets);
      });
  }

  getTicketStatus(status: number): string {
    return this.ticketService.getTicketStatus(status);
  }
  getStatusIconClass(status: number): string {
    return this.ticketService.getStatusIconClass(status);
  }

  getProduct(productId: number): Product | undefined {
    return this.products.find((product) => product.productId === productId);
  }

  loadTicketsOfMonth() {
    const month = new Date().getMonth() + 1;
    const year = new Date().getFullYear();

    this.dashService
      .getTicketsPerMonthUser(this.auth.getUserID(), month, year)
      .subscribe((res: APIResponse) => {
        this.ticketPerMonth = res.data[0];
      });
  }
  loadTicketsPerYear() {
    const date = new Date().getFullYear();

    this.dashService
      .getTicketsPerYearUser(this.auth.getUserID(), date)
      .subscribe((res: APIResponse) => {
        this.ticketsOfYear = res.data[0].ticketsCount;
      });
  }

  getTicketsGroupByStat() {
    this.dashService
      .userGetTicketsGroupByStatus(this.auth.getUserID())
      .subscribe((res: APIResponse) => {
        this.ticketsByStatus = Object(res.data);
      });
  }
}
