import { DashboardComponent } from './components/dashboard/dashboard.component';

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeeLayoutComponent } from './components/employee-layout/employee-layout.component';
import { AssignedTicketsComponent } from './components/assigned-tickets/assigned-tickets.component';

import { ProfileComponent } from '../shared/components/profile/profile.component';
import { TicketDetailsComponent } from '../client/components/ticket-details/ticket-details.component';

const routes: Routes = [
  {
    path: '',
    component: EmployeeLayoutComponent,
    children: [
      {
        path: 'home',
        component: DashboardComponent,
      },
      {
        path: 'tickets',
        component: AssignedTicketsComponent,
      },
      {
        path: 'ticket-details',
        component: TicketDetailsComponent,
      },

      {
        path: 'profile',
        component: ProfileComponent,
      },
      {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class EmployeeRoutingModule {}
