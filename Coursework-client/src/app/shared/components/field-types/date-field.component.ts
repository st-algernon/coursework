import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { BasicFieldType } from './basic-field-type';

@Component({
  selector: 'app-date-field',
  template: `              
    <mat-form-field class="form-field" appearance="standard">
      <mat-label>{{ label }}</mat-label>
      <input matInput [formControl]="formControl" [matDatepicker]="picker">
      <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
      <mat-datepicker #picker></mat-datepicker>
    </mat-form-field>
  `,
  styles: [`.form-field { width: 100% }`]
})
export class DateFieldComponent extends BasicFieldType {}
