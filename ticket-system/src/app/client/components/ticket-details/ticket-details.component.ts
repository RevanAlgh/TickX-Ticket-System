import { Comment } from './../../../core/model/Comment';
import { Ticket } from './../../../core/model/Ticket';
import { Component, OnInit } from '@angular/core';
import { TicketServiceService } from '../../../shared/services/ticket-service.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { TranslateModule } from '@ngx-translate/core';
import { DateService } from '../../../shared/services/date.service';
import { FileReaderService } from '../../../shared/services/file-reader.service';
import { AlertboxService } from '../../../shared/services/alertbox.service';
import { APIResponse } from '../../../core/model/APIResponse';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import Username from '../../../core/model/username';
import { EmployeesService } from '../../../admin/services/employees.service';
import { User } from '../../../core/model/User';
import { Roles } from '../../../core/model/enums/Roles';
import { AuthService } from '../../../core/services/auth.service';
import { Status } from '../../../core/model/enums/Status';
import { StatusPipe } from '../../../shared/pipes/status.pipe';
import { PriorityPipe } from '../../../shared/pipes/priority.pipe';

@Component({
  selector: 'app-ticket-details',
  standalone: true,
  imports: [
    TranslateModule,
    RouterLink,
    CommonModule,
    FormsModule,
    StatusPipe,
    PriorityPipe,
  ],
  templateUrl: './ticket-details.component.html',
  styleUrl: './ticket-details.component.css',
})
export class TicketDetailsComponent implements OnInit {
  ticket!: Ticket;
  comments!: Comment[];
  showInputField = false;
  newComment: string = '';

  constructor(
    private ticketService: TicketServiceService,

    private router: Router,
    private activeRout: ActivatedRoute,
    private dService: DateService,
    private fileService: FileReaderService,
    private alert: AlertboxService,
    private employeeeService: EmployeesService,
    private auth: AuthService
  ) {}
  ngOnInit(): void {
    this.ticket = history.state.ticket;
    this.loadComments();
    console.log(this.ticket.fileName);
  }

  toggleInputField() {
    this.showInputField = !this.showInputField;
  }
  loadComments() {
    this.ticketService
      .getTicketsComments(this.ticket.ticketId)
      .subscribe((res: APIResponse) => {
        if (res.status) this.comments = res.data;
      });
  }

  getDataFormat(s: string) {
    const parsedDate = new Date(s);
    const formattedDate = this.dService.formatDateToShortVersion(parsedDate);

    return formattedDate;
  }

  onConfirm(id: number) {
    if (this.ticket.status === Status.Resolved) {
      this.alert.error('Ticket Error', 'This ticket is resolved already');
      return;
    }
    this.ticketService
      .setTicketStatus(id, Status.Resolved)
      .subscribe((res: APIResponse) => {
        if (res.status) this.alert.showEvaluationAlert();
        else {
          this.alert.error('Error', 'Error while updating the data');
        }
      });
  }
  getNameById(id: number): string | void {}

  addComment() {
    if (this.newComment.trim()) {
      const comment: Comment = new Comment();
      comment.replie = this.newComment;
      comment.ticketId = this.ticket.ticketId;
      comment.userId = this.auth.getUserID();

      this.ticketService.addComment(comment).subscribe(
        (res: APIResponse) => {
          this.loadComments();
        },
        (err) => {
          this.alert.error(err.error.message);
        }
      );

      this.newComment = '';
      this.showInputField = false;
    }
  }

  getRole() {
    return this.auth.getRole() == Roles.SupportManager;
  }
  isItUser() {
    return this.auth.getRole() == Roles.Client;
  }
  getRouterLink() {
    const userRol = this.auth.getRole();

    if (userRol == Roles.Client) {
      return '../view-tickets';
    } else if (userRol == Roles.TeamMember) {
      return '../tickets';
    } else return '../tickets';
  }

  isClosed() {
    return (
      this.ticket.status == Status.Closed ||
      this.ticket.status == Status.Resolved
    );
  }
  getAttachmentUrl(base64: string) {
    return `data:${'application/octet-stream'};base64,` + base64;
  }
}
