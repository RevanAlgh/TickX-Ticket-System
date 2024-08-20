import { ClientsService } from './../../services/clients.service';
import { Status } from './../../../core/model/enums/Status';
import { Component, OnInit } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { NsPipe } from '../../../shared/pipes/ns.pipe';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterLink, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { APIResponse } from '../../../core/model/APIResponse';

import { FormsModule } from '@angular/forms';
import { User } from '../../../core/model/User';
import { PopupComponent } from '../../../shared/components/popup/popup.component';
import { MatDialog } from '@angular/material/dialog';
import { AlertboxService } from '../../../shared/services/alertbox.service';

@Component({
  selector: 'app-client-list',
  standalone: true,
  imports: [
    NsPipe,
    CommonModule,
    TranslateModule,
    NgbPaginationModule,
    RouterLink,
    RouterModule,
    FormsModule,
  ],
  templateUrl: './client-list.component.html',
  styleUrl: './client-list.component.css',
})
export class ClientListComponent implements OnInit {
  clients: User[] = [];
  page = 1;
  pageSize = 10;
  client: User = new User();
  constructor(
    private clientService: ClientsService,
    private dialog: MatDialog,
    private alert: AlertboxService
  ) {}
  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.clientService.getClients().subscribe((res: APIResponse) => {
      this.clients = res.data;
    });
  }

  setActive(id: number, isActive: boolean) {
    this.alert.ask('Are you sure?', () => {
      this.clientService
        .activateClient(id, isActive)
        .subscribe((res: APIResponse) => {
          this.loadData();
        });
    });
  }

  Openpopup(title: string, opration: string, user?: User) {
    var _popup = this.dialog.open(PopupComponent, {
      width: '80%',
      height: '80%',
      data: {
        title: title,
        operation: opration,
        user: user,
      },
    });
    _popup.afterClosed().subscribe(() => {
      this.loadData();
    });
  }

  editEmployee(user: User) {
    this.Openpopup('Edit Client', 'edit', user);
  }

  //start pagination
  onItemChange(event: any) {
    this.pageSize = event.target.value;
  }
}
