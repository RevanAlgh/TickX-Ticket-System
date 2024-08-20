import { APIResponse } from './../../core/model/APIResponse';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../../core/model/User';
import { environment } from '../../../environments/environment.development';
import { Constant } from '../../core/constant/Constant';
import { RegisterModal } from '../../core/model/Register';

@Injectable({
  providedIn: 'root',
})
export class EmployeesService {
  url = environment.API_URL;

  constructor(private http: HttpClient) {}

  getEmplyees() {
    return this.http.get<APIResponse>(
      this.url + Constant.API_END_POINT.GET_EMPLOYEES
    );
  }
  getEmployeeById(id: number) {
    return this.http.get<APIResponse>(
      this.url + Constant.API_END_POINT.GET_EMPLOYEE + id
    );
  }
  addEmployee(obj: RegisterModal) {
    return this.http.put<APIResponse>(
      this.url + Constant.API_END_POINT.ADD_EMPLOYEE,
      obj
    );
  }
  activateEmployee(id: number, isActive: boolean) {
    return this.http.put<APIResponse>(
      this.url + Constant.API_END_POINT.TOGGLE_ACTIVATION,
      { userId: id, isActive: !isActive }
    );
  }

  editEmployee(obj: any) {
    return this.http.put<APIResponse>(
      this.url + Constant.API_END_POINT.ADD_EMPLOYEE,
      obj
    );
  }
}
