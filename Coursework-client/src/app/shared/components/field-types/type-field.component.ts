import { Component, Input, OnInit, Type } from '@angular/core';
import { FormControl } from '@angular/forms';
import { FieldTypes } from '../../types';
import { StringFieldComponent } from './string-field.component';
import { BasicFieldType } from './basic-field-type';
import { TextFieldComponent } from './text-field.component';
import { NumericalFieldComponent } from './numerical-field.component';
import { DateFieldComponent } from './date-field.component';
import { CheckboxFieldComponent } from './checkbox-field.components';

@Component({
  selector: 'app-type-field',
  template: `
    <ng-container *ngxComponentOutlet="$any(this) | resolve: fieldType">
    </ng-container>
  `
})
export class TypeFieldComponent extends BasicFieldType {

  @Input() fieldType: string = 'String';

  resolve(type: string): Type<BasicFieldType> {
    switch (type) {
      case 'String':
        return StringFieldComponent;
      case 'Text':
        return TextFieldComponent;
      case 'Numerical':
        return NumericalFieldComponent;
      case 'Date':
        return DateFieldComponent;
      case 'Checkbox':
        return CheckboxFieldComponent;
      default:
        console.warn(`Unknown of type ${type}!`)

      return StringFieldComponent;
    }
  } 
}