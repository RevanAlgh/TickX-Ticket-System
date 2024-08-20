import { APIResponse } from './../../core/model/APIResponse';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Ticket } from '../../core/model/Ticket';
import { environment } from '../../../environments/environment.development';
import { Constant } from '../../core/constant/Constant';
import { SendTicketModel } from '../../core/model/SendTicketModal';

import { Status } from '../../core/model/enums/Status';
import { Observable } from 'rxjs';
import { Priorities } from '../../core/model/enums/Priorities';
import { Comment } from '../../core/model/Comment';

@Injectable({
  providedIn: 'root',
})
export class TicketServiceService {
  url = environment.API_URL;

  statusOptions = [
    { name: 'New', value: Status.New },
    { name: 'ReOpened', value: Status.ReOpened },
    { name: 'In Progress', value: Status.InProgress },
    { name: 'Resolved', value: Status.Resolved },
    { name: 'Closed', value: Status.Closed },
  ];
  priorityOptions = [
    { name: 'Low', value: Priorities.Low },
    { name: 'Medium', value: Priorities.Medium },
    { name: 'High', value: Priorities.High },
  ];

  constructor(private http: HttpClient) {}

  addTicket(ticket: SendTicketModel) {
    return this.http.post<SendTicketModel>(
      this.url + Constant.API_END_POINT.ADD_TICKET,
      ticket
    );
  }

  getTickets(id: number) {
    return this.http.get<APIResponse>(
      this.url + Constant.API_END_POINT.GET_TICKETS + id
    );
  }
  getAAllTickets() {
    return this.http.get<APIResponse>(
      this.url + Constant.API_END_POINT.GET_ALL_TICKETS
    );
  }

  getStatusIconClass(status: number): string {
    switch (status) {
      case Status.New:
        return 'fa fa-clock-o'; // Clock icon for "New"
      case Status.ReOpened:
        return 'fa fa-hourglass-half'; // Hourglass icon for "ReOpened"
      case Status.InProgress:
        return 'fa fa-spinner fa-spin'; // Spinner icon for "InProgress"
      case Status.Closed:
        return 'fa fa-check-circle'; // Check-circle icon for "Closed"
      case Status.Resolved:
        return 'fa fa-check-circle'; // Check-circle icon for "Resolved"
      default:
        return 'fa fa-question-circle'; // Question-circle icon for unknown status
    }
  }

  getTicketStatus(status: Status): string {
    switch (status) {
      case Status.New:
        return 'New';
      case Status.Resolved:
        return 'Resolved';
      case Status.InProgress:
        return 'InProgress';
      case Status.Closed:
        return 'closed';
      case Status.ReOpened:
        return 'ReOpened';
      default:
        return 'unknown';
    }
  }

  setAssignTo(obj: any): Observable<APIResponse> {
    return this.http.put<APIResponse>(
      this.url + Constant.API_END_POINT.ASSIGN_TICKET,
      obj
    );
  }

  loadRecentTickets(userId: number): Observable<APIResponse> {
    return this.http.put<APIResponse>(
      this.url + Constant.API_END_POINT.LOAD_RECENT_TICKETS,
      { userId: userId, ticketCount: 3 }
    );
  }

  setPriority(ticketId: number, priority: string): Observable<APIResponse> {
    return this.http.put<APIResponse>(
      this.url + Constant.API_END_POINT.ASSIGN_PRIORITY,
      { ticketId: ticketId, priority: parseInt(priority) }
    );
  }

  getAssignedTicketsByAssigedID(id: number): Observable<APIResponse> {
    return this.http.get<APIResponse>(
      this.url + Constant.API_END_POINT.GET_ASSIGN_TO_TICKETS + id
    );
  }
  getTicketsByPriority(p: Priorities): Observable<APIResponse> {
    return this.http.get<APIResponse>(
      this.url + Constant.API_END_POINT.GET_TICKETS_BY_PRIORITY + p
    );
  }
  getTicketsByStatus(s: Status): Observable<APIResponse> {
    return this.http.get<APIResponse>(
      this.url + Constant.API_END_POINT.GET_TICKETS_BY_PRIORITY + s
    );
  }

  getTicketsComments(ticketId: number): Observable<APIResponse> {
    return this.http.get<APIResponse>(
      this.url + Constant.API_END_POINT.GET_COMMENTS + ticketId
    );
  }

  addComment(obj: Comment): Observable<APIResponse> {
    return this.http.post<APIResponse>(
      this.url + Constant.API_END_POINT.ADD_COMMENT,
      obj
    );
  }

  setTicketStatus(ticketId: number, status: Status): Observable<APIResponse> {
    return this.http.put<APIResponse>(
      this.url + Constant.API_END_POINT.SET_TICKET_STATUS,
      { ticketId: ticketId, status: status }
    );
  }

  getFilteredData(params: any): Observable<any> {
    return this.http.post<APIResponse>(
      this.url + Constant.API_END_POINT.GET_FILTERED_TICKETS,
      params
    );
  }

  updateTicket(ticket: Ticket) {}
}
