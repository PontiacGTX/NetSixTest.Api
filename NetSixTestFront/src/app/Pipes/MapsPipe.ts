import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'map' })
export class MapPipe implements PipeTransform {
  transform(value: any[], property: string): any[] {
    if (!Array.isArray(value) || !property) {
      return value;
    }
    return value.map(item => item[property]);
  }
}
