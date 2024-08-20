import { Observable, Subscriber } from 'rxjs';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { DEFAULT_LANGUAGE, TranslateModule } from '@ngx-translate/core';
import { User } from '../../../core/model/User';
import { AuthService } from '../../../core/services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { RegisterModal } from '../../../core/model/Register';
import { Roles } from '../../../core/model/enums/Roles';
import { AlertboxService } from '../../services/alertbox.service';
import * as intlTelInput from 'intl-tel-input';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    FormsModule,
    TranslateModule,
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent implements OnInit {
  img: string = 'chatbot.png';
  imgName: string = '';
  user!: User;
  errorMessage: string = '';

  registerValid = true;
  registerForm = new FormGroup({
    fullName: new FormControl('', Validators.required),
    userName: new FormControl('', Validators.required),
    mobileNumber: new FormControl('', Validators.required),
    email: new FormControl('', Validators.email),
    dob: new FormControl('', Validators.required),
    address: new FormControl('', Validators.required),
    userImage: new FormControl(''),
    password: new FormControl('', Validators.required),
    cnfrmPassword: new FormControl('', Validators.required),
  });

  constructor(
    private auth: AuthService,
    private router: Router,
    private alert: AlertboxService
  ) {}

  ngOnInit(): void {
    const inputTel = document.getElementById('number');
    if (inputTel) {
      intlTelInput.default(inputTel, {
        initialCountry: 'sa',

        preferredCountries: ['sa', 'kw', 'eg', 'jo', 'qa'],
        utilsScript:
          'https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/23.8.2/js/intlTelInput.min.js',
      });
    }
  }

  checkPasswords(group: FormGroup): ValidationErrors | null {
    const pass = group.controls['password'].value;
    const confirmPass = group.controls['confirmPassword'].value;
    return pass === confirmPass ? null : { notSame: true };
  }

  onSubmit() {
    const img = this.img.split(',');
    if (this.registerForm.invalid) {
      this.errorMessage = 'Complate all fields';
      this.registerValid = false;
      return;
    }

    if (
      this.registerForm.get('password')!.value !=
      this.registerForm.get('cnfrmPassword')!.value
    ) {
      this.alert.error('password not match');
      return;
    }

    this.user = Object(this.registerForm.value);
    this.user.userImage = img[1];

    const u = {
      fullName: this.user.fullName,
      userName: this.user.userName,
      mobileNumber: this.user.mobileNumber.toString(),
      email: this.user.email,
      dob: this.user.dob.toString(),
      address: this.user.address,
      userImage: img[1],
      fileName: this.imgName.toString(),
      password: this.user.password,
      role: 1,
    };
    console.log(u);
    this.auth.register(u).subscribe(
      (res) => {
        this.alert.success(
          'Your Registration Has bees successful',
          'Please Confirm Your Email to Be able to login '
        );

        this.router.navigate(['/login']);
      },
      (error) => {
        this.alert.error(error);
        console.error('Register error:', error);
        this.alert.error('Register Failed', '');
      }
    );
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
