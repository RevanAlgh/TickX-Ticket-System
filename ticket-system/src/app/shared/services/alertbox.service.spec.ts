import { TestBed } from '@angular/core/testing';

import { AlertboxService } from './alertbox.service';

describe('AlertboxService', () => {
  let service: AlertboxService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AlertboxService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
