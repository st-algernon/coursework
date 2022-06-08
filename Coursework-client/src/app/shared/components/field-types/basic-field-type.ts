import { Directive, Input } from "@angular/core";
import { FormControl } from "@angular/forms";

@Directive({})
export abstract class BasicFieldType {
  @Input() label: string = '';
  @Input() formControl: FormControl = new FormControl();
}