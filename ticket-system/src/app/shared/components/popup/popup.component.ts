import { error } from 'jquery';
import { TranslateModule } from '@ngx-translate/core';
import { Component, inject, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { User } from '../../../core/model/User';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Roles } from '../../../core/model/enums/Roles';
import { ClientsService } from '../../../admin/services/clients.service';
import { APIResponse } from '../../../core/model/APIResponse';
import { EmployeesService } from '../../../admin/services/employees.service';
import { AlertboxService } from '../../services/alertbox.service';
import Swal from 'sweetalert2';
import { RegisterModal } from '../../../core/model/Register';
import { AuthService } from '../../../core/services/auth.service';
@Component({
  selector: 'app-popup',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormsModule,
    MatFormFieldModule,
    MatInput,
    TranslateModule,
  ],
  templateUrl: './popup.component.html',
  styleUrl: './popup.component.css',
})
export class PopupComponent {
  isEdit = false;
  user!: User;
  inputdata: any;
  editdata: any;
  phoneNum: number = 0;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: User,
    private ref: MatDialogRef<PopupComponent>,
    private alert: AlertboxService,

    private clientService: ClientsService,
    private employeeservice: EmployeesService,
    private auth: AuthService
  ) {}
  ngOnInit(): void {
    this.inputdata = this.data;

    if (this.inputdata.operation == 'add') {
      this.user = new User();
    } else {
      this.user = this.inputdata.user;
      this.phoneNum = parseInt(this.user.mobileNumber);
    }
  }

  closepopup() {
    this.ref.close('Closed using function');
  }
  formatDateToShortVersion(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Months are zero-based
    const day = String(date.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }
  saveUser() {
    const date = new Date();
    const formattedDate = this.formatDateToShortVersion(date);

    if (this.inputdata.operation == 'add') {
      if (this.user.userId == -1) {
        // to add employee

        const newEmp = new RegisterModal();
        newEmp.address = this.user.address;
        newEmp.dob = formattedDate;
        newEmp.fileName = '';
        newEmp.fullName = this.user.fullName;
        newEmp.mobileNumber = this.phoneNum.toString();
        newEmp.password = '';
        newEmp.email = this.user.email;
        newEmp.role = Roles.TeamMember;
        newEmp.userImage = '';
        newEmp.userName = this.user.userName;

        console.log(newEmp);

        this.auth.register(newEmp).subscribe(
          (res: APIResponse) => {
            if (res.status) {
              this.alert.success('Employee added Successfully', '');
              this.closepopup();
            } else {
              this.alert.error('Error occurs while adding data', res.message);
            }
          },
          (err) => {
            this.alert.error(err.error.message);
          }
        );
      }
    } else {
      if (this.user.role == Roles.TeamMember) {
        // to edit employee

        Swal.fire({
          title: `Are you sure want update Employee #${this.user.userId}`,
          icon: 'info',

          showCancelButton: true,
          confirmButtonText: 'Yes',
          denyButtonText: 'No',
        }).then((result) => {
          if (result.isConfirmed) {
            const updatedEmp = new RegisterModal();
            updatedEmp.address = this.user.address;
            updatedEmp.dob = formattedDate;
            updatedEmp.fileName = this.user.fileName;
            updatedEmp.fullName = this.user.fullName;
            updatedEmp.mobileNumber = this.phoneNum.toString();
            updatedEmp.password = this.user.password;
            updatedEmp.email = this.user.email;
            updatedEmp.role = Roles.TeamMember;
            updatedEmp.userImage = this.user.userImage;
            updatedEmp.userId = this.user.userId;
            updatedEmp.userName = this.user.userName;

            this.employeeservice.addEmployee(updatedEmp).subscribe(
              (res: APIResponse) => {
                this.alert.success(
                  'User updated Successfully',
                  `User ID: ${this.user.userId} Has been updated successfully`
                );
                this.closepopup();
              },
              (err) => {
                this.alert.error(err.error.message);
              }
            );
          } else if (result.isDenied) {
            Swal.fire('No changes were made', '', 'info');
          }
        });
      } else {
        Swal.fire({
          title: `Are you sure want update Client #${this.user.userId}`,
          icon: 'info',
          showCancelButton: true,
          confirmButtonText: 'Yes',
          denyButtonText: 'No',
        }).then((result) => {
          if (result.isConfirmed) {
            const updatedEmp = new RegisterModal();
            updatedEmp.address = this.user.address;
            updatedEmp.dob = formattedDate;
            updatedEmp.fileName = this.user.fileName;
            updatedEmp.fullName = this.user.fullName;
            updatedEmp.mobileNumber = this.phoneNum.toString();
            updatedEmp.password = this.user.password;
            updatedEmp.email = this.user.email;
            updatedEmp.role = Roles.Client;
            updatedEmp.userImage = this.user.userImage;
            updatedEmp.userId = this.user.userId;
            updatedEmp.userName = this.user.userName;
            this.clientService.editClient(updatedEmp).subscribe(
              (res: APIResponse) => {
                if (res.status) {
                  this.alert.success(
                    'Client updated Successfully',
                    `Client ID: ${this.user.userId} Has been updated successfully`
                  );
                  this.closepopup();
                } else {
                  this.alert.error(res.message);
                  this.closepopup();
                }
              },
              (error) => {
                this.alert.error(error);
              }
            );
          } else if (result.isDenied) {
            Swal.fire('No changes were made', '', 'info');
            this.closepopup();
          }
        });
      }
    }
  }
}
