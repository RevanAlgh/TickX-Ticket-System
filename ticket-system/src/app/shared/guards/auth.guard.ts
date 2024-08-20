import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { inject } from '@angular/core';
import { AlertboxService } from '../services/alertbox.service';
import { routes } from '../../app.routes';

export const AuthGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const alert = inject(AlertboxService);

  if (authService.isLoggedIn()) {
    if (authService.user?.role == route.data['allowedRole']) {
      //user got banned

      return true;
    } else {
      router.navigate(['/login']);
      return false;
    }
  }
  return false;
};
