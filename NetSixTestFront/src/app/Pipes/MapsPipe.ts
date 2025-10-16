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

@Pipe({name: 'joinNames'})
export class JoinNamesPipe implements PipeTransform {
  transform(value: any[], key: string): string {
    if (!value || value.length === 0) return '';
    return value.map(item => item[key]).join(', ');
  }
}
