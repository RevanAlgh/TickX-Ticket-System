import { LoaderService } from './../loader.service';
import { inject, Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpInterceptorFn,
  HttpHandlerFn,
} from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { finalize, timeout } from 'rxjs/operators';

Injectable({ providedIn: 'root' });
export const loaderInterceptorFn: HttpInterceptorFn = (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> => {
  const loaderService = inject(LoaderService);

  loaderService.show();

  // const timeout$ = of(null).pipe(timeout(10000));
  return next(req).pipe(finalize(() => loaderService.hide()));
};
