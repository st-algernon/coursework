import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { BasicFieldType } from './basic-field-type';

@Component({
  selector: 'app-checkbox-field',
  template: `   
    <p class="form-field">
        <mat-checkbox [formControl]="formControl">{{ label }}</mat-checkbox>
    </p>
  `,
  styles: [`.form-field { margin: 16px 0px 4px 0px; }`]
})
export class CheckboxFieldComponent extends BasicFieldType {}
