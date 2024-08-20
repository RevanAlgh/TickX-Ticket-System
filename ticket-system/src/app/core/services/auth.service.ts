import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { APIResponse } from '../model/APIResponse';
import { environment } from '../../../environments/environment.development';
import { LoginModal } from '../model/Login';
import { Constant } from '../constant/Constant';
import { User } from '../model/User';
import { RegisterModal } from '../model/Register';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient) {
    const user = localStorage.getItem('userData');
    if (user) {
      this.user = JSON.parse(user);
    }
  }

  getAuthToken() {
    return localStorage.getItem('token');
  }

  url = environment.API_URL;
  user!: User;

  getRole() {
    if (!this.user) return null;
    return this.user.role;
  }

  login(obj: LoginModal): Observable<APIResponse> {
    return this.http.post<APIResponse>(
      this.url + Constant.API_END_POINT.USER_LOGIN,
      obj
    );
  }

  register(obj: any) {
    return this.http.post<APIResponse>(
      this.url + Constant.API_END_POINT.USER_REGISTER,
      obj
    );
  }

  logout() {
    if (!this.user) return;

    localStorage.clear();
    this.user = new User();
  }
  isLoggedIn(): boolean {
    return localStorage.getItem('token') && localStorage.getItem('userData')
      ? true
      : false;
  }

  getUserID() {
    if (!this.isLoggedIn()) return -1;

    return this.user.userId;
  }

  resetPasswordByLink(email: string): Observable<APIResponse> {
    return this.http.post<APIResponse>(
      this.url + Constant.API_END_POINT.RESET_PASSWORD,
      {
        email: email,
      }
    );
  }

  resetPasswordWithURl(obj: any): Observable<APIResponse> {
    return this.http.post<APIResponse>(
      this.url + Constant.API_END_POINT.RESET_PASSWORD_WITH_URL,
      obj
    );
  }
}
