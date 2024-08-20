import { Pipe, PipeTransform } from '@angular/core';
import { Status } from '../../core/model/enums/Status';

@Pipe({
  name: 'status',
  standalone: true,
})
export class StatusPipe implements PipeTransform {
  transform(value: Status): string {
    switch (value) {
      case Status.New:
        return 'New'; // Clock icon for "New"
      case Status.ReOpened:
        return 'Re-Opened'; // Hourglass icon for "ReOpened"
      case Status.InProgress:
        return 'In Progress'; // Spinner icon for "InProgress"
      case Status.Closed:
        return 'Closed'; // Check-circle icon for "Closed"
      case Status.Resolved:
        return 'Resolved'; // Check-circle icon for "Resolved"
      default:
        return 'Unkown'; // Question-circle icon for unknown status
    }
  }
}
