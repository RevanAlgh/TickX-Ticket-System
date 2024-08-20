import { LoginAPIResponce } from './../../../core/model/LoginAPIResponse';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { LoginModal } from '../../../core/model/Login';
import { AuthService } from '../../../core/services/auth.service';
import { APIResponse } from '../../../core/model/APIResponse';
import { User } from '../../../core/model/User';
import { Roles } from '../../../core/model/enums/Roles';
import { AlertboxService } from '../../services/alertbox.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [TranslateModule, CommonModule, FormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent implements OnInit {
  loginObj: LoginModal = new LoginModal();
  isRemembered = false;
  user!: User;

  isArabic: boolean = false;
  loginDataAPi!: LoginAPIResponce;
  constructor(
    private router: Router,
    private auth: AuthService,
    private alert: AlertboxService
  ) {}
  ngOnInit(): void {
    // const user = localStorage.getItem('userData');
    // if (user) {
    //   this.auth.user = JSON.parse(user);
    //   const role = this.getPath(this.auth.getRole());
    //   this.router.navigate(['/' + role]);
    // }

    const lang = localStorage.getItem('lang');

    if (lang == 'ar') {
      this.isArabic = true;
    } else this.isArabic = false;
  }

  getPath(role: any) {
    if (!role) return;

    if (role == 1) return 'client';
    else if (role == 2) return 'employee';
    else if (role == 3) return 'admin';
    return;
  }
  onLogin() {
    if (this.loginObj.nena == '' || this.loginObj.password == '') {
      this.alert.error('Enter valid data');
      return;
    }
    this.auth.login(this.loginObj).subscribe(
      (res: APIResponse) => {
        if (res.status) {
          this.loginDataAPi = Object(res.data);
          this.loginDataAPi.user.token = this.loginDataAPi.token;
          this.auth.user = this.loginDataAPi.user;
          // if (this.isRemembered)
          localStorage.setItem(
            'userData',
            JSON.stringify(this.loginDataAPi.user)
          );
          localStorage.setItem('token', this.loginDataAPi.token);
          const role = this.getPath(this.loginDataAPi.user.role);

          this.router.navigate(['/' + role]);
        } else {
          this.alert.error(
            'Error while Login',
            'Invalid email, number, username , password, or user is inactive'
          );
        }
      },
      (error) => {
        console.error('Login error:', error);
        this.alert.error(
          'Login Failed',
          'Invalid email, username , password, or user is inactive.'
        );
      }
    );
  }
}
