import { Status } from './../../core/model/enums/Status';
import { Constant } from './../../core/constant/Constant';
import { APIResponse } from './../../core/model/APIResponse';
import { catchError, Observable, throwError } from 'rxjs';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Priorities } from '../../core/model/enums/Priorities';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  url = environment.API_URL;

  constructor(private http: HttpClient) {}

  getTopEmployeesManger(): Observable<APIResponse> {
    return this.http.get<APIResponse>(
      this.url + Constant.API_END_POINT.GET_TOP_EMPLOYEES
    );
  }
  getTicketsByPriority() {}

  getTicketsPerMonthManger(
    year: number,
    month: number
  ): Observable<APIResponse> {
    return this.http.post<APIResponse>(
      this.url + Constant.API_END_POINT.GET_MANGER_TICKETS_PER_MONTH,
      { year: year, month: month }
    );
  }

  getTicketsPerYearManger(year: number): Observable<APIResponse> {
    return this.http.post<APIResponse>(
      this.url + Constant.API_END_POINT.GET_MANGER_TICKETS_PER_YEAR,
      { year: year }
    );
  }
  getStat(): Observable<APIResponse> {
    return this.http
      .get<APIResponse>(this.url + Constant.API_END_POINT.GET_STAT)
      .pipe(
        catchError((error) => {
          console.error('Error fetching statistics:', error);
          return throwError(() => new Error('Error fetching statistics'));
        })
      );
  }

  getTicketGroupByPrioruty(): Observable<APIResponse> {
    return this.http
      .get<APIResponse>(
        this.url + Constant.API_END_POINT.GET_TICKETS_GROUP_BY_PRIORITY
      )
      .pipe(
        catchError((error) => {
          console.error('Error fetching tickets by priority:', error);
          return throwError(() => new Error('Error fetching tickets'));
        })
      );
  }

  getTotalTicketsSummary(
    status: Status,
    priority: Priorities
  ): Observable<APIResponse> {
    return this.http.post<APIResponse>(
      this.url + Constant.API_END_POINT.GET_MANGER_TICKETS_PER_YEAR,
      { status: status, priority: priority }
    );
  }
  getTicketsByStatus(): Observable<APIResponse> {
    return this.http
      .get<APIResponse>(
        this.url + Constant.API_END_POINT.GET_TICKETS_BY_STATUS_MANGER
      )
      .pipe(
        catchError((error) => {
          console.error('Error fetching tickets by status:', error);
          return throwError(
            () => new Error('Error fetching tickets by status')
          );
        })
      );
  }
  ticketsOfTheYearGroupByMonths(): Observable<APIResponse> {
    return this.http
      .get<APIResponse>(
        this.url + Constant.API_END_POINT.GET_TICKETS_OF_YEAR_GROUP_BY_MONTHS
      )
      .pipe(
        catchError((error) => {
          console.error('Error fetching tickets of the year:', error);
          return throwError(
            () =>
              new Error('Error fetching tickets of the year grouped by months')
          );
        })
      );
  }
  getTicketsPerMonthUser(
    userId: number,
    month: number,
    year: number
  ): Observable<APIResponse> {
    return this.http.post<APIResponse>(
      this.url + Constant.API_END_POINT.GET_TICKETS_USER_PER_MONTH,
      { userId: userId, month: month, year: year }
    );
  }

  userGetTicketByPriority(
    userId: number,
    priority: number
  ): Observable<APIResponse> {
    return this.http.post<APIResponse>(
      this.url + Constant.API_END_POINT.USER_GET_TICKETS_BY_PRIORITY,
      { userId: userId, priorty: priority }
    );
  }

  getTicketsPerYearUser(userId: number, year: number): Observable<APIResponse> {
    return this.http.post<APIResponse>(
      this.url + Constant.API_END_POINT.GET_TICKETS_USER_PER_YEAR,
      { userId: userId, year: year }
    );
  }

  getTicketsForUser(
    userId: number,
    status: Status,
    priority: Priorities
  ): Observable<APIResponse> {
    return this.http.post<APIResponse>(
      this.url + Constant.API_END_POINT.GET_TOTAL_TICKETS,
      { userId: userId, status: status, priority: priority }
    );
  }

  userGetTicketsGroupByStatus(userId: number): Observable<APIResponse> {
    return this.http.get<APIResponse>(
      this.url + Constant.API_END_POINT.USER_TICKETS_STATUS_COUNT + userId
    );
  }

  userGetTicketByPriority2(userId: number): Observable<APIResponse> {
    return this.http.get<APIResponse>(
      this.url + Constant.API_END_POINT.USER_TICKET_PRIORITY_COUNT + userId
    );
  }
}
