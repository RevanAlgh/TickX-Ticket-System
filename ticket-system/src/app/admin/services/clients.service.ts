import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../../core/model/User';
import { environment } from '../../../environments/environment.development';
import { Constant } from '../../core/constant/Constant';
import { APIResponse } from '../../core/model/APIResponse';
import { Observable } from 'rxjs';
import { Status } from '../../core/model/enums/Status';
import { Priorities } from '../../core/model/enums/Priorities';
import { RegisterModal } from '../../core/model/Register';

@Injectable({
  providedIn: 'root',
})
export class ClientsService {
  url = environment.API_URL;
  constructor(private http: HttpClient) {}

  getClients(): Observable<APIResponse> {
    return this.http.get<APIResponse>(
      this.url + Constant.API_END_POINT.GET_CLIENTS
    );
  }

  activateClient(id: number, isActive: boolean) {
    return this.http.put<APIResponse>(
      this.url + Constant.API_END_POINT.CLIENT_TOGGLE_ACTIVATION,
      { userId: id, isActive: !isActive }
    );
  }

  getClientById(id: number) {
    return this.http.get<APIResponse>(
      this.url + Constant.API_END_POINT.GET_CLIENTS + id
    );
  }

  editClient(obj: any): Observable<APIResponse> {
    return this.http.put<APIResponse>(
      this.url + Constant.API_END_POINT.UPDATE_CLIENT,
      obj
    );
  }
}
