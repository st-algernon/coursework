import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';
import { Observable, Subscription, switchMap } from 'rxjs';
import { FieldWithTypeName, FullField, Item, ShortItem, Tag } from '../../interfaces';
import { CollectionsService } from '../../services/collections.service';
import { ItemsService } from '../../services/items.service';

@Component({
  selector: 'app-item-form',
  templateUrl: './item-form.component.html',
  styleUrls: ['./item-form.component.scss']
})
export class ItemFormComponent implements OnInit {
  @Input() seed?: Item;
  @Input() parentCollectionId?: string;
  @Output() submitted = new EventEmitter<ShortItem>();
  @Output() loading = new EventEmitter<boolean>();

  separatorKeysCodes: number[] = [ENTER, COMMA];
  foundTags$: Observable<Tag[]>;
  tagNames: string[] = [];

  itemForm: FormGroup;
  fields: FieldWithTypeName[] = [];
  subs: Subscription[] = [];

  @ViewChild('tagInput') tagInput?: ElementRef<HTMLInputElement>;

  get titleCtrl(): FormControl {
    return this.itemForm?.controls.title as FormControl;
  }

  get coverCtrl(): FormControl {
    return this.itemForm?.controls.cover as FormControl;
  }

  get tagCtrl(): FormControl {
    return this.itemForm?.controls.tag as FormControl;
  }

  get fieldCtrls(): FormControl[] {
    return (this.itemForm?.get('fields') as FormArray).controls as FormControl[];
  }

  get fieldsArray(): FormArray {
    return this.itemForm?.get('fields') as FormArray;
  }

  constructor(
    private itemsService: ItemsService,
    private collectionsService: CollectionsService
  ) {
    this.itemForm = new FormGroup({
      title: new FormControl(null, [Validators.required]),
      cover: new FormControl(),
      tag: new FormControl(),
      fields: new FormArray([])
    });

    this.foundTags$ = this.tagCtrl.valueChanges.pipe(
      switchMap((value: string) => { 
        if (value == '')
          return new Observable<Tag[]>();

        return itemsService.searchTags(value) 
      })
    );
  }

  ngOnInit(): void {
    if (this.parentCollectionId) {
      this.subs.push(
        this.collectionsService.getCollectionFields(this.parentCollectionId)
        .subscribe(
          (response: FieldWithTypeName[]) => {
            this.fields = response;
            response.forEach(
              f => this.fieldsArray.push(new FormControl())
            );

            if (this.seed) {
              this.seedFieldsValue(this.seed);
            }
          }
        )
      );
    }
  }

  ngOnChanges(): void {
    if (this.seed) {
      this.itemForm.patchValue(this.seed); 
      this.tagNames = this.seed.tagVMs.map(t => t.name);
    }
  }

  addTag(event: MatChipInputEvent) {
    const value = (event.value || '').trim();

    if(this.tagNames.includes(value) == false) {
      this.tagNames.push(value);
    }

    event.chipInput?.clear();
    this.tagCtrl.setValue(null);
  }

  selectTag(event: MatAutocompleteSelectedEvent) {
    const tag = event.option.value as Tag;

    if (this.tagNames.includes(tag.name) == false) {
      this.tagNames.push(tag.name);
    }

    if (this.tagInput) 
      this.tagInput.nativeElement.value = '';

    this.tagCtrl.setValue(null);
  }

  removeTagById(index: number) {
    this.tagNames.splice(index, 1);
  }

  seedFieldsValue(seed: Item): void {
    for (let i = 0; i < this.fields.length; i++) {
      if (this.fields[i].id == seed.fullFieldVMs[i].id) {
        this.fieldCtrls[i].setValue(seed.fullFieldVMs[i].value);
      }
    }
  }

  deleteDefaultCover() {
    if (this.seed)
      this.seed.coverUrl = undefined;
  }

  submit() {
    if (this.itemForm.invalid || !this.parentCollectionId) {
      return
    }

    this.loading.emit(true);

    const fullFields = this.getFullFields();
    const request: ShortItem = {
      id: this.seed?.id,
      title: this.titleCtrl.value,
      coverUrl: this.seed?.coverUrl,
      collectionId: this.parentCollectionId,
      tagNames: this.tagNames,
      fullFieldVMs: fullFields
    };
    const file = this.coverCtrl.value as File;

    if (this.coverCtrl.value instanceof File) {
      const file = this.coverCtrl.value as File;
      const formData = new FormData();

      formData.set('file', file, file.name);

      this.itemsService.uploadCover(formData).subscribe(
        (response: string) => {
          this.loading.emit(false)
          request.coverUrl = response;
          this.submitted.emit(request);
        },
        () => this.loading.emit(false)
      );
    } else {
      this.submitted.emit(request);
    }
  }

  onCoverChange(files: File[]) {
    this.coverCtrl.setValue(files[0]);
  }

  private getFullFields(): FullField[] {
    const fullFields: FullField[] = [];

    for (let i = 0; i < this.fieldCtrls.length; i++) {
      fullFields.push({ 
        id: this.fields[i].id,
        name: this.fields[i].name,
        typeName: this.fields[i].typeName,
        value: this.fieldCtrls[i].value
      });
    }

    return fullFields;
  }
}
