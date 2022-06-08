import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { BasicFieldType } from './basic-field-type';

@Component({
  selector: 'app-string-field',
  template: `              
    <mat-form-field class="form-field" appearance="standard">
      <mat-label>{{ label }}</mat-label>
      <input type="text" [formControl]="formControl" matInput />
    </mat-form-field>
  `,
  styles: [`.form-field { width: 100%}`]
})
export class StringFieldComponent extends BasicFieldType {}
