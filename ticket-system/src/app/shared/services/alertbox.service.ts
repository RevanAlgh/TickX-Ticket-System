import { Inject, Injectable } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import Swal from 'sweetalert2';
import { PopupComponent } from '../components/popup/popup.component';
import { MatDialogRef } from '@angular/material/dialog';
@Injectable({
  providedIn: 'root',
})
export class AlertboxService {
  tranlsate = Inject(TranslateModule);
  constructor() {}

  success(title: string, message?: string) {
    Swal.fire({
      title: title,
      icon: 'success',
      confirmButtonText: 'Close',
      text: message,
    });
  }

  error(title: string, message?: string) {
    Swal.fire({
      icon: 'error',
      title: title,
      text: message,
    });
  }

  ask(title: string, cal: CallableFunction) {
    // translate service
    Swal.fire({
      title: title,
      showDenyButton: true,

      confirmButtonText: 'Yes',
      denyButtonText: 'No',
    }).then((result) => {
      if (result.isConfirmed) {
        cal(); // Execute the callback function
      } else if (result.isDenied) {
        Swal.fire('No changes were made', '', 'info');
      }
    });
  }

  showEvaluationAlert() {
    Swal.fire({
      title: 'Rate Our Service',
      icon: 'success',
      text: 'We hope you had a great experience! Did our service meet your expectations? Your feedback helps us improve.',
      showCloseButton: true,
      showCancelButton: true,
      focusConfirm: false,
      confirmButtonText: `
        <i class="fa fa-thumbs-up"></i> Great!
      `,
      confirmButtonAriaLabel: 'Thumbs up, great!',
      cancelButtonText: `
        <i class="fa fa-thumbs-down"></i>
      `,
      cancelButtonAriaLabel: 'Thumbs down',
    });
  }
}
