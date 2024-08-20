import { Routes } from '@angular/router';
import { LoginComponent } from './shared/components/login/login.component';
import { RegisterComponent } from './shared/components/register/register.component';
import { AuthGuard } from './shared/guards/auth.guard';
import { Roles } from './core/model/enums/Roles';
import { NotAuthGuard } from './shared/guards/not-auth.guard';
import { ForgotPasswordComponent } from './shared/components/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './shared/components/reset-password/reset-password.component';

export const routes: Routes = [
  {
    path: 'admin',
    loadChildren: () =>
      import('./admin/admin.module').then((m) => m.AdminModule),
    canActivate: [AuthGuard],
    data: {
      allowedRole: Roles.SupportManager,
    },
  },
  {
    path: 'employee',
    loadChildren: () =>
      import('./employee/employee.module').then((m) => m.EmployeeModule),
    canActivate: [AuthGuard],
    data: {
      allowedRole: Roles.TeamMember,
    },
  },
  {
    path: 'client',
    loadChildren: () =>
      import('./client/client.module').then((m) => m.ClientModule),
    canActivate: [AuthGuard],
    data: {
      allowedRole: Roles.Client,
    },
  },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [NotAuthGuard],
  },
  {
    path: 'register',
    component: RegisterComponent,
    canActivate: [NotAuthGuard],
  },
  {
    path: 'forgotPassword',
    component: ForgotPasswordComponent,
    canActivate: [NotAuthGuard],
  },
  {
    path: 'resetPassword',
    component: ResetPasswordComponent,
  },
];
