import { Status } from './../../../core/model/enums/Status';
import { Component } from '@angular/core';
import { Ticket } from '../../../core/model/Ticket';
import { Product } from '../../../core/model/Product';
import { TicketServiceService } from '../../../shared/services/ticket-service.service';
import { AuthService } from '../../../core/services/auth.service';
import { DateService } from '../../../shared/services/date.service';
import { ProductService } from '../../../shared/services/product.service';
import { Router, RouterLink } from '@angular/router';
import { APIResponse } from '../../../core/model/APIResponse';
import { CommonModule, DatePipe } from '@angular/common';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { PopupComponent } from '../ticket-popup/ticket-popup.component';
import { MatDialog } from '@angular/material/dialog';
import { EmployeesService } from '../../services/employees.service';
import { EmployeeModal } from '../../../core/model/Employee';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { Observable } from 'rxjs';
import { elementAt, map, startWith } from 'rxjs/operators';
import { MatInputModule } from '@angular/material/input';
import { AlertboxService } from '../../../shared/services/alertbox.service';

import { Priorities } from '../../../core/model/enums/Priorities';
import { StatusPipe } from '../../../shared/pipes/status.pipe';
import { PriorityPipe } from '../../../shared/pipes/priority.pipe';
import { HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-tickets',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    NgbPaginationModule,
    MatFormFieldModule,
    MatAutocompleteModule,
    ReactiveFormsModule,
    MatInputModule,
    RouterLink,
    StatusPipe,
    PriorityPipe,
  ],
  templateUrl: './tickets.component.html',
  styleUrl: './tickets.component.css',
})
export class TicketsComponent {
  tickets: Ticket[] = [];
  products: Product[] = [];
  page: number = 1;
  pageSize: number = 10;
  employees: EmployeeModal[] = [];

  showSearchInputIndex: number | null = null;

  assignedEmployeeControl = new FormControl(-1);

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
    private dialog: MatDialog,
    private employeeService: EmployeesService,
    private alert: AlertboxService
  ) {}

  ngOnInit(): void {
    this.loadTickets();
    this.loadEmployees();

    this.assignFilterChangeHandlers();
  }
  //start pagination
  onItemChange(event: any) {
    this.pageSize = event.target.value;
  }

  // Start filter table function

  private assignFilterChangeHandlers() {
    this.assignedEmployeeControl.valueChanges.subscribe(() =>
      this.fetchTickets()
    );
    this.statusControl.valueChanges.subscribe(() => this.fetchTickets());
    this.priorityControl.valueChanges.subscribe(() => this.fetchTickets());
    this.searchControl.valueChanges.subscribe(() => this.fetchTickets());
  }

  private fetchTickets() {
    const priority =
      this.priorityControl.value !== null
        ? this.priorityControl.value.toString()
        : '0';
    const assignedToUserId =
      this.assignedEmployeeControl.value !== null
        ? this.assignedEmployeeControl.value
        : '0';
    const status =
      this.statusControl.value !== null
        ? this.statusControl.value.toString()
        : '0';
    const name = this.searchControl.value || '';

    const obj = {
      priority: parseInt(priority),
      assignedToUserId: assignedToUserId,
      clientId: -1,
      status: parseInt(status),
      name: name,
    };

    this.ticketService.getFilteredData(obj).subscribe((res) => {
      this.tickets = res.data || [];
    });
  }
  changeStatus(id: number) {
    this.alert.ask('Are you sure?', () => {
      this.ticketService
        .setTicketStatus(id, Status.Closed)
        .subscribe((res: APIResponse) => {
          if (res.status) {
            this.alert.success('Ticket closed Successfully');
            this.loadTickets();
          } else {
            this.alert.error(res.errors[0]);
          }
        });
    });
  }

  onEmployeeChange(event: Event): void {
    const selectedEmployeeId = (event.target as HTMLSelectElement).value;

    if (selectedEmployeeId == '-1') {
      this.loadTickets();
      return;
    }

    this.ticketService
      .getAssignedTicketsByAssigedID(parseInt(selectedEmployeeId))
      .subscribe((res: APIResponse) => {
        if (res.status) this.tickets = res.data || [];
      });
  }

  onPriorityChange(event: Event): void {
    const selectElement = (event.target as HTMLSelectElement).value;

    if (selectElement == '-1') {
      this.loadTickets();
      return;
    }

    this.ticketService
      .getTicketsByPriority(parseInt(selectElement))
      .subscribe((res: APIResponse) => {
        if (res.status) {
          if (res.data == null) {
            this.alert.error('Error', 'No data available for this status');
            return;
          }

          this.tickets = res.data || [];
        }
      });
  }

  onStatusChange(event: Event): void {
    const selectElement = (event.target as HTMLSelectElement).value;

    if (selectElement == '-1') {
      this.loadTickets();
      return;
    }

    this.ticketService
      .getTicketsByStatus(parseInt(selectElement))
      .subscribe((res) => {
        if (res.status && res.data) {
          this.tickets = res.data || [];
        }
      });
  }

  //end filter table

  //filter search for employee
  private _filter(value: any): EmployeeModal[] {
    if (typeof value !== 'string') {
      return this.employees;
    }

    const filterValue = value.toLowerCase();

    return this.employees.filter(
      (option) =>
        option.fullName.toLowerCase().includes(filterValue) ||
        option.userName.toLowerCase().includes(filterValue) ||
        option.email.toLowerCase().includes(filterValue) ||
        option.userId.toString().includes(filterValue)
    );
  }

  loadEmployees() {
    this.employeeService.getEmplyees().subscribe((res: APIResponse) => {
      if (res.status) {
        this.employees = res.data;
      }
    });
  }
  loadTickets() {
    this.ticketService.getAAllTickets().subscribe((res: APIResponse) => {
      if (res.status) {
        this.tickets = res.data || [];
      }
    });
  }
  isItResolve(status: Status) {
    return Status.Resolved == status;
  }
  openpopup(ticket: Ticket) {
    var _popup = this.dialog.open(PopupComponent, {
      width: '40%',
      height: '55%',
      data: {
        ticket: ticket,
        employees: this.employees,
      },
    });
    _popup.afterClosed().subscribe(() => {
      this.loadTickets();
    });
  }
  isClosedOrResolve(item: Status) {
    return item == Status.Closed || item == Status.Resolved;
  }
}
