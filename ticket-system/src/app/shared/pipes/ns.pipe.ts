import { Pipe, PipeTransform } from '@angular/core';
import { empty } from 'rxjs';

@Pipe({
  name: 'ns',
  standalone: true,
})
export class NsPipe implements PipeTransform {
  transform(value: unknown): unknown {
    if (value != null && value != undefined && value != '') return value;
    else return 'NA';
  }
}
