import { error } from 'jquery';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService } from '../../../core/services/auth.service';
import { AlertboxService } from '../../services/alertbox.service';
import {
  FormControl,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { APIResponse } from '../../../core/model/APIResponse';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [RouterLink, TranslateModule, ReactiveFormsModule, FormsModule],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css',
})
export class ForgotPasswordComponent {
  email = new FormControl('', Validators.email);

  constructor(private auth: AuthService, private alert: AlertboxService) {}

  onSubmit() {
    if (this.email.value == '' || this.email === undefined) return;

    if (this.email.invalid) {
      this.alert.error('Error', 'Please Enter Valid Email');
      return;
    }
    this.alert.ask('Are you sure want to reset password?', () => {
      this.auth.resetPasswordByLink(this.email.value!).subscribe({
        next: (res: APIResponse) => {
          if (res.status) {
            console.log(res);
            this.alert.success(
              'Successful',
              'Link sent to your email successfully'
            );
          } else {
            this.alert.error('Error', res.message);
          }
        },
        error: (err) => {
          console.error('HTTP Error:', err);
          this.alert.error('Error', err.error.message);
        },
      });
    });

    console.log(this.email.value);
  }
}
