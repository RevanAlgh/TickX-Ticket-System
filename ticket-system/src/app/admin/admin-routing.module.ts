import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClientListComponent } from './components/client-list/client-list.component';
import { LayoutComponent } from './components/layout/layout.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { TeamMembersListComponent } from './components/team-members-list/team-members-list.component';
import { TicketsComponent } from './components/tickets/tickets.component';
import { TicketDetailsComponent } from '../client/components/ticket-details/ticket-details.component';
import { ProductComponent } from './components/product/product.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      {
        path: 'dashboard',
        component: DashboardComponent,
      },
      {
        path: 'clients',
        component: ClientListComponent,
      },
      {
        path: 'employees',
        component: TeamMembersListComponent,
      },
      {
        path: 'tickets',
        component: TicketsComponent,
      },
      {
        path: 'ticket-details',
        component: TicketDetailsComponent,
      },
      {
        path: 'products',
        component: ProductComponent,
      },
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
