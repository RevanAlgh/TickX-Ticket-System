import { StatusPipe } from './../../../shared/pipes/status.pipe';
import { Ticket } from './../../../core/model/Ticket';
import { DateService } from './../../../shared/services/date.service';
import { Component } from '@angular/core';

import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { ClientDashboardComponent } from '../client-dashboard/client-dashboard.component';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { TicketServiceService } from '../../../shared/services/ticket-service.service';

import { AuthService } from '../../../core/services/auth.service';
import { APIResponse } from '../../../core/model/APIResponse';
import { Status } from '../../../core/model/enums/Status';
import { FormsModule } from '@angular/forms';
import { Product } from '../../../core/model/Product';
import { ProductService } from '../../../shared/services/product.service';
import { PriorityPipe } from '../../../shared/pipes/priority.pipe';

@Component({
  selector: 'app-view-ticket',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    RouterLink,
    FormsModule,
    RouterModule,
    RouterLink,
    StatusPipe,
    PriorityPipe,
  ],
  templateUrl: './view-ticket.component.html',
  styleUrl: './view-ticket.component.css',
})
export class ViewTicketComponent {
  tickets: Ticket[] = [];
  products: Product[] = [];
  constructor(
    private ticketService: TicketServiceService,
    private auth: AuthService,
    private dateService: DateService,
    private pService: ProductService,
    private router: Router
  ) {}
  ngOnInit(): void {
    this.ticketService
      .getTickets(this.auth.getUserID())
      .subscribe((res: APIResponse) => {
        if (res.status) {
          this.tickets = res.data;
        }
      });
    this.pService.getPorudcts().subscribe((res: APIResponse) => {
      if (res.status) this.products = res.data;
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
}
