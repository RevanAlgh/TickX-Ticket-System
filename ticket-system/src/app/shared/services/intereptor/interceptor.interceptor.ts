import {
  HttpErrorResponse,
  HttpEvent,
  HttpEventType,
  HttpHandler,
  HttpHandlerFn,
  HttpHeaders,
  HttpRequest,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';
import { AlertboxService } from '../alertbox.service';
import { LoaderService } from '../loader.service';

export function loggingInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  const router = inject(Router);
  const alert = inject(AlertboxService);
  const loader = inject(LoaderService);
  return next(req).pipe(
    tap((event) => {
      if (event.type === HttpEventType.Response) {
        console.log(req.url, 'returned a response with status', event.status);
      }
    }),
    catchError((error) => {
      loader.hide();
      console.log(error.message);
      return throwError(() => error); // Re-throw the error after logging
    })
  );
}

// export function loggingInterceptor(
//   req: HttpRequest<unknown>,
//   next: HttpHandlerFn
// ): Observable<HttpEvent<unknown>> {
//   console.log(req.url);
//   return next(req);
// }

export function authInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
) {
  // Inject the current `AuthService` and use it to get an authentication token:
  const authToken = inject(AuthService).getAuthToken();
  // Clone the request to add the authentication header.

  const headers = new HttpHeaders({
    Authorization: 'Bearer ' + authToken!,
  });
  const cloneReq = req.clone({ headers });

  return next(cloneReq);
}
