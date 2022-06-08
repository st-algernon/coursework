import { Pipe, PipeTransform } from "@angular/core";
import { FieldTypes } from "../types";
import * as moment from 'moment';

@Pipe({name: 'displayField'})
export class DisplayFieldPipe implements PipeTransform {
  transform(value: any, type: FieldTypes): string {
    switch (type) {
        case 'String':
          return value;
        case 'Text':
          return value;
        case 'Numerical':
          return value;
        case 'Date':
          return moment(value).format('ll');
        case 'Checkbox':
          return value == 'on' ? 'Yes' : 'No';
        default:
          console.warn(`Unknown of type ${type}!`)
    }

    return value;
  }
}