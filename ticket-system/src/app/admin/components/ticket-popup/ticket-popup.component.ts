import { Ticket } from './../../../core/model/Ticket';
import { Component, inject, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInput, MatInputModule } from '@angular/material/input';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { User } from '../../../core/model/User';
import { APIResponse } from '../../../core/model/APIResponse';
import { TranslateModule } from '@ngx-translate/core';
import { TicketServiceService } from '../../../shared/services/ticket-service.service';
import { EmployeeModal } from '../../../core/model/Employee';
import { forkJoin, map, Observable, startWith } from 'rxjs';
import {
  MatAutocomplete,
  MatAutocompleteModule,
} from '@angular/material/autocomplete';
import { Status } from '../../../core/model/enums/Status';
import { CommonModule } from '@angular/common';
import { AlertboxService } from '../../../shared/services/alertbox.service';
import { StatusPipe } from '../../../shared/pipes/status.pipe';
import { PriorityPipe } from '../../../shared/pipes/priority.pipe';
import { error } from 'jquery';

@Component({
  selector: 'app-popup',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatFormFieldModule,
    MatInput,
    TranslateModule,
    MatInputModule,
    MatAutocompleteModule,
    StatusPipe,
    PriorityPipe,
  ],
  templateUrl: './ticket-popup.component.html',
  styleUrl: './ticket-popup.component.css',
})
export class PopupComponent {
  ticket!: Ticket;
  employees: EmployeeModal[] = [];
  inputdata: any;
  filteredOptions!: Observable<EmployeeModal[]>;
  selectedEmployee!: number;
  selectedPriority!: string;

  employeeControl = new FormControl();
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: User,
    private ref: MatDialogRef<PopupComponent>,
    private alert: AlertboxService,
    private ticketService: TicketServiceService
  ) {}
  ngOnInit(): void {
    this.inputdata = this.data;
    this.ticket = this.inputdata.ticket;
    this.employees = this.inputdata.employees;
    this.selectedPriority = this.ticket.priority.toString();
    this.selectedEmployee = this.ticket.assignedTo;
    this.filteredOptions = this.employeeControl.valueChanges.pipe(
      startWith(''),
      map((value) => this._filter(value))
    );
  }

  getStatus() {
    return this.ticketService.statusOptions;
  }

  getPriority() {
    return this.ticketService.priorityOptions;
  }
  onPriorityChange(newPriority: string) {
    this.selectedPriority = newPriority;
    console.log('Selected Priority:', this.selectedPriority);
  }

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
  closepopup() {
    this.ref.close('Closed using function');
  }

  saveTicket() {
    const x = this.selectedEmployee;
    const y = this.selectedPriority;
    forkJoin({
      assigned: this.ticketService.setAssignTo({
        ticketId: this.ticket.ticketId,
        assignedTo: x,
      }),
      pro: this.ticketService.setPriority(this.ticket.ticketId, y),
    }).subscribe(
      (res) => {
        this.alert.success('Data updated Successfully');
        this.closepopup();
      },

      () => {
        this.alert.success('Data updated Successfully');
        this.closepopup();
      }
    );
  }
  onOptionSelected(event: any) {
    this.selectedEmployee = event.option.value;
  }
}
