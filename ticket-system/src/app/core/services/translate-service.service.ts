import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TranslateService {
  translateService = inject(TranslateService);
}
