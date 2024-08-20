import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { inject } from '@angular/core';
import { AlertboxService } from '../services/alertbox.service';
import { routes } from '../../app.routes';

export const NotAuthGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const alert = inject(AlertboxService);

  if (authService.isLoggedIn()) {
    let role = authService.getRole(),
      rolePath = '';
    if (role == 1) rolePath = 'client';
    else if (role == 2) rolePath = 'employee';
    else if (role == 3) rolePath = 'admin';
    router.navigate(['/' + rolePath]);
    return false;
  } else return true;
};
