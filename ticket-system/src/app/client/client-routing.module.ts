import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClientDashboardComponent } from './components/client-dashboard/client-dashboard.component';
import { LayoutComponent } from './components/layout/layout.component';
import { ViewTicketComponent } from './components/view-ticket/view-ticket.component';
import { AddTicketComponent } from './components/add-ticket/add-ticket.component';
import { ProfileComponent } from '../shared/components/profile/profile.component';
import { TicketDetailsComponent } from './components/ticket-details/ticket-details.component';
import { StatusPipe } from '../shared/pipes/status.pipe';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: 'home', component: ClientDashboardComponent },
      {
        path: 'view-tickets',
        component: ViewTicketComponent,
      },
      {
        path: 'ticket-details',
        component: TicketDetailsComponent,
      },
      { path: 'add-ticket', component: AddTicketComponent },
      { path: 'profile', component: ProfileComponent },
      { path: '', redirectTo: 'home', pathMatch: 'full' },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ClientRoutingModule {}
