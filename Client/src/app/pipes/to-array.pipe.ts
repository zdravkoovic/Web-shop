import { NgIterable, Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'toArray',
  standalone: true
})
export class ToArrayPipe implements PipeTransform {

  transform(iterable: NgIterable<any>): any[] {
    if(iterable){
      return Array.from(iterable);
    }
    return [];
  }

}
