import {
  ApplicationConfig,
  importProvidersFrom,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter, RouterModule } from '@angular/router';

import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { routes } from './app.routes';
import { AppTranslateModule } from './shared/modules/app-translate/app-translate.module';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { MatDialogRef } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import {
  authInterceptor,
  loggingInterceptor,

  // cachingInterceptor,
  // loggingInterceptor,
} from './shared/services/intereptor/interceptor.interceptor';

import { loaderInterceptorFn } from './shared/services/intereptor/loader.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(),
    importProvidersFrom(AppTranslateModule.forRoot()),
    importProvidersFrom(RouterModule),
    importProvidersFrom(MatDialogRef),
    importProvidersFrom(MatProgressSpinnerModule),

    provideAnimationsAsync(),
    provideAnimationsAsync(),
    provideHttpClient(
      withInterceptors([
        loggingInterceptor,
        authInterceptor,
        loaderInterceptorFn, //dont miss this
      ])
    ),
  ],
};
