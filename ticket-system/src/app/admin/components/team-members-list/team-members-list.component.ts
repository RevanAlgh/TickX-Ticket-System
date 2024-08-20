import { Component } from '@angular/core';
import { NsPipe } from '../../../shared/pipes/ns.pipe';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterLink, RouterModule } from '@angular/router';
import { User } from '../../../core/model/User';
import { EmployeesService } from '../../services/employees.service';
import { APIResponse } from '../../../core/model/APIResponse';
import { FormsModule } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { PopupComponent } from '../../../shared/components/popup/popup.component';
import { AlertboxService } from '../../../shared/services/alertbox.service';

@Component({
  selector: 'app-team-members-list',
  standalone: true,
  imports: [
    NsPipe,
    CommonModule,
    TranslateModule,
    NgbPaginationModule,
    RouterLink,
    RouterModule,
    FormsModule,
  ],
  templateUrl: './team-members-list.component.html',
  styleUrl: './team-members-list.component.css',
})
export class TeamMembersListComponent {
  pageSize = 10;
  page = 1;
  employee: User[] = [];
  employeeObj: User = new User();

  constructor(
    private employeeService: EmployeesService,
    private dialog: MatDialog,
    private alert: AlertboxService
  ) {}
  ngOnInit(): void {
    this.loadData();
  }
  setActive(id: number, isActive: boolean) {
    this.alert.ask('Are You Sure', () => {
      this.employeeService
        .activateEmployee(id, isActive)
        .subscribe((res: APIResponse) => {
          this.loadData();
        });
    });
  }

  loadData() {
    this.employeeService.getEmplyees().subscribe((res: APIResponse) => {
      this.employee = res.data;
    });
  }

  onEdit(item: User) {
    this.employeeObj = item;
  }

  reset() {
    this.employeeObj = new User();
  }
  Openpopup(title: string, opration: string, user?: User) {
    var _popup = this.dialog.open(PopupComponent, {
      width: '80%',
      height: '80%',
      data: {
        title: title,
        operation: opration,
        user: user,
      },
    });
    _popup.afterClosed().subscribe(() => {
      this.loadData();
    });
  }

  addEmployee() {
    this.Openpopup('Add Employee', 'add');
  }

  editEmployee(user: User) {
    this.Openpopup('Edit Employee', 'edit', user);
  }
  //start pagination
  onItemChange(event: any) {
    this.pageSize = event.target.value;
  }
}
