import { EmployeesService } from './../../../admin/services/employees.service';
import { MatTabsModule } from '@angular/material/tabs';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { User } from '../../../core/model/User';
import { FormsModule } from '@angular/forms';
import { AlertboxService } from '../../services/alertbox.service';
import { ClientsService } from '../../../admin/services/clients.service';
import { Roles } from '../../../core/model/enums/Roles';
import Swal from 'sweetalert2';
import { use } from 'echarts';
import { RegisterModal } from '../../../core/model/Register';
import { APIResponse } from '../../../core/model/APIResponse';
import { Observable, Subscriber } from 'rxjs';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [RouterLink, CommonModule, TranslateModule, FormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css',
})
export class ProfileComponent implements OnInit {
  user!: User;
  imgName: string = '';
  errorMessage: string = '';
  img: string = 'chatbot.png';

  constructor(
    private alert: AlertboxService,
    private empServ: EmployeesService,
    private clientSer: ClientsService
  ) {}
  ngOnInit(): void {
    if (!(localStorage.getItem('userData') && localStorage.getItem('token'))) {
      console.log('No user found');
      return;
    }

    this.user = JSON.parse(localStorage.getItem('userData')!);
    this.img = 'data:image/jpeg;base64,' + this.user.userImage;
  }
  activeSection: string = 'account-general';

  open(sectionId: string) {
    if (this.user.role == Roles.Client) {
    }
  }
  updateData() {
    const img = this.img.split(',');
    console.log(img[1]);
    Swal.fire({
      title: 'Are you sure want to update data?',
      showDenyButton: true,
      icon: 'info',

      confirmButtonText: 'Yes',
      denyButtonText: 'No',
    }).then((result) => {
      if (result.isConfirmed) {
        const usr = {
          userId: this.user.userId,
          fullName: this.user.fullName,
          userName: this.user.userName,
          mobileNumber: this.user.mobileNumber.toString(),
          email: this.user.email,
          dob: this.user.dob.toString(),
          address: this.user.address,
          userImage: img[1],
          fileName: 'userProfile.jpg',
          isActive: true,
          role: this.user.role,
        };

        console.log(usr);
        if (this.user.role == Roles.Client) {
          this.clientSer.editClient(usr).subscribe((res: APIResponse) => {
            if (res.status) {
              console.log(res.data);
              localStorage.setItem('userData', JSON.stringify(usr));
              Swal.fire(
                'Your data has been succcessfully udpated',
                '',
                'success'
              );
            }
          });
        } else if (this.user.role == Roles.TeamMember) {
          this.empServ.editEmployee(usr).subscribe(
            (res) => {
              if (res.status) {
                localStorage.setItem('userData', JSON.stringify(usr));
                Swal.fire(
                  'Your data has been succcessfully udpated',
                  '',
                  'success'
                );
              }
            },
            (err) => {
              this.alert.error(err.error.message);
            }
          );
        }
      } else if (result.isDenied) {
        Swal.fire('No changes were made', '', 'info');
      }
    });
  }

  onChange($event: any) {
    this.errorMessage = '';
    const file = ($event.target as HTMLInputElement).files![0];
    this.imgName = file.name;

    if (file) {
      this.validateImageSize(file)
        .then((isValid) => {
          if (isValid) {
            this.convertToBase64(file);
          } else {
            this.errorMessage = 'Image exceeds 2 megapixels.';
          }
        })
        .catch((error) => {
          console.error('Error validating image size:', error);
        });
    }
  }
  convertToBase64(file: File) {
    const obs = new Observable((subscriber: Subscriber<any>) => {
      this.readFile(file, subscriber);
    });

    obs.subscribe((res) => {
      this.img = res;
    });
  }

  validateImageSize(file: File): Promise<boolean> {
    return new Promise((resolve, reject) => {
      const img = new Image();
      img.onload = () => {
        const width = img.width;
        const height = img.height;
        const megapixels = (width * height) / 1000000;
        resolve(megapixels <= 2);
      };
      img.onerror = (error) => {
        reject(error);
      };
      img.src = URL.createObjectURL(file);
    });
  }

  readFile(file: File, subscriber: Subscriber<any>) {
    const fileReader = new FileReader();
    fileReader.readAsDataURL(file);

    fileReader.onload = () => {
      subscriber.next(fileReader.result);
      subscriber.complete();
    };
    fileReader.onerror = (error) => {
      subscriber.error(error);
      subscriber.complete();
    };
  }
}
