import { Component, NgModule, OnInit, PipeTransform } from '@angular/core';
import { NsPipe } from '../../../shared/pipes/ns.pipe';
import { TranslateModule } from '@ngx-translate/core';
import { Ticket } from '../../../core/model/Ticket';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterLink, RouterModule } from '@angular/router';
import { TicketServiceService } from '../../../shared/services/ticket-service.service';
import { AuthService } from '../../../core/services/auth.service';
import { CommonModule } from '@angular/common';
import { Status } from '../../../core/model/enums/Status';
import { Roles } from '../../../core/model/enums/Roles';
import { AlertboxService } from '../../../shared/services/alertbox.service';
import { APIResponse } from '../../../core/model/APIResponse';
import { StatusPipe } from '../../../shared/pipes/status.pipe';
import { PriorityPipe } from '../../../shared/pipes/priority.pipe';
import { Priorities } from '../../../core/model/enums/Priorities';

@Component({
  selector: 'app-assigned-tickets',
  standalone: true,
  imports: [
    NsPipe,
    TranslateModule,
    NgbPaginationModule,
    RouterLink,
    RouterModule,
    CommonModule,
    StatusPipe,
    PriorityPipe,
    ReactiveFormsModule,
    FormsModule,
  ],
  templateUrl: './assigned-tickets.component.html',
  styleUrl: './assigned-tickets.component.css',
})
export class AssignedTicketsComponent implements OnInit {
  page = 1;
  pageSize = 10;
  tickets: Ticket[] = [];
  statusControl = new FormControl(-1);
  priorityControl = new FormControl(-1);
  searchControl = new FormControl('');
  statusOptions = [
    { name: 'New', value: Status.New },
    { name: 'ReOpened', value: Status.ReOpened },
    { name: 'In Progress', value: Status.InProgress },
    { name: 'Resolved', value: Status.Resolved },
    { name: 'Closed', value: Status.Closed },
  ];
  priorityOptions = [
    { name: 'Low', value: Priorities.Low },
    { name: 'Medium', value: Priorities.Medium },
    { name: 'High', value: Priorities.High },
  ];
  constructor(
    private ticketService: TicketServiceService,
    private auth: AuthService,
    private alert: AlertboxService
  ) {}

  ngOnInit(): void {
    this.loadAssignedTickets();
    this.assignFilterChangeHandlers();
  }

  private assignFilterChangeHandlers() {
    this.statusControl.valueChanges.subscribe(() => this.fetchTickets());
    this.priorityControl.valueChanges.subscribe(() => this.fetchTickets());
    this.searchControl.valueChanges.subscribe(() => this.fetchTickets());
  }

  private fetchTickets() {
    const priority =
      this.priorityControl.value !== null
        ? this.priorityControl.value.toString()
        : '0';
    const assignedToUserId = this.auth.getUserID();

    const status =
      this.statusControl.value !== null
        ? this.statusControl.value.toString()
        : '0';
    const name = this.searchControl.value || '';

    const obj = {
      priority: parseInt(priority),
      assignedToUserId: this.auth.getUserID(),
      clientId: -1,
      status: parseInt(status),
      name: name,
    };

    this.ticketService.getFilteredData(obj).subscribe((res) => {
      this.tickets = res.data;
    });
  }

  loadAssignedTickets() {
    this.ticketService
      .getAssignedTicketsByAssigedID(this.auth.getUserID()) //
      .subscribe((res) => {
        if (res.status) this.tickets = res.data;
      });
  }

  changeStatus(id: number) {
    this.alert.ask('Are you sure?', () => {
      this.ticketService
        .setTicketStatus(id, Status.Closed)
        .subscribe((res: APIResponse) => {
          if (res.status) {
            this.alert.success('Ticket closed Successfully');
            this.loadAssignedTickets();
          } else {
            this.alert.error(res.errors[0]);
          }
        });
    });
  }

  //start pagination
  onItemChange(event: any) {
    this.pageSize = event.target.value;
  }
}
