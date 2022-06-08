import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { BasicFieldType } from './basic-field-type';

@Component({
  selector: 'app-text-field',
  template: `              
    <mat-form-field class="form-field" appearance="standard">
      <mat-label>{{ label }}</mat-label>
      <textarea [formControl]="formControl" matInput></textarea>
    </mat-form-field>
  `,
  styles: [`.form-field { width: 100%}`]
})
export class TextFieldComponent extends BasicFieldType {}
