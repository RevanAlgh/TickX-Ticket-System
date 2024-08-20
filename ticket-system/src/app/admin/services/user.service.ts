import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { Observable } from 'rxjs';
import { APIResponse } from '../../core/model/APIResponse';
import { Constant } from '../../core/constant/Constant';
import { User } from '../../core/model/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  url = environment.API_URL
  constructor(private http: HttpClient) { }


  getUserById(id: number): Observable<APIResponse>{
   return  this.http.get<User>(this.url + Constant.API_END_POINT.)
  }
}
