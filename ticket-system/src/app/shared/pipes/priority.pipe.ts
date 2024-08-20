import { Pipe, PipeTransform } from '@angular/core';
import { Priorities } from '../../core/model/enums/Priorities';

@Pipe({
  name: 'priority',
  standalone: true,
})
export class PriorityPipe implements PipeTransform {
  transform(value: Priorities): string {
    switch (value) {
      case Priorities.Low:
        return 'low';
      case Priorities.Medium:
        return 'medium';
      case Priorities.High:
        return 'high';
      default:
        return 'unknown';
    }
  }
}
