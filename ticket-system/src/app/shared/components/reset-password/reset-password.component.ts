import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { ActivatedRoute, Params } from '@angular/router';
import { AlertboxService } from '../../services/alertbox.service';
import { APIResponse } from '../../../core/model/APIResponse';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule, TranslateModule],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css',
})
export class ResetPasswordComponent implements OnInit {
  password: string = '';
  cnfrmPassword: string = '';
  email!: string;
  token!: string;

  constructor(
    private auth: AuthService,
    private activatedRoute: ActivatedRoute,
    private alert: AlertboxService
  ) {}
  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe((queryParams) => {
      this.email = queryParams['email'];
      this.token = queryParams['token'];
    });
  }

  onSubmit() {
    if (this.password != this.cnfrmPassword) {
      this.alert.error('Confirm Password not match password');
      return;
    }

    this.auth
      .resetPasswordWithURl({
        email: this.email,
        newPassword: this.password,
        token: this.token,
      })
      .subscribe((res: APIResponse) => {
        if (res.status) {
          this.alert.success('Your password has been reset successfully');
        }
      });
  }
}
