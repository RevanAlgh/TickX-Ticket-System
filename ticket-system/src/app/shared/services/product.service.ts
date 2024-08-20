import { APIResponse } from './../../core/model/APIResponse';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { Constant } from '../../core/constant/Constant';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  url = environment.API_URL;
  constructor(private http: HttpClient) {}

  getPorudcts(): Observable<APIResponse> {
    return this.http.get<APIResponse>(
      this.url + Constant.API_END_POINT.GET_PRODUCTS
    );
  }

  addProduct(name: string): Observable<APIResponse> {
    return this.http.post<APIResponse>(
      this.url + Constant.API_END_POINT.ADD_PRODUCT,
      { name: name }
    );
  }
}
