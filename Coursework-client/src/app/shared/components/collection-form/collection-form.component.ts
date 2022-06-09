import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAccordion } from '@angular/material/expansion';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { FileParameter, CollectionsClient, CollectionVm, FieldTypeVm, FieldVm, Topic, TopicsClient } from '../../services/api.service';
import { DndComponent } from '../dnd/dnd.component';

@Component({
  selector: 'app-collection-form',
  templateUrl: './collection-form.component.html',
  styleUrls: ['./collection-form.component.scss']
})
export class CollectionFormComponent implements OnInit, OnDestroy, OnChanges {
  @Input() submitBtnName: string = 'Create';
  @Input() seed?: CollectionVm;
  @Output() submitted = new EventEmitter<CollectionVm>();

  isLoading: boolean = false;
  ownerId: string;
  collectionForm: FormGroup;
  topics: Topic[] = [];
  selectedTopics: Topic[] = [];
  fieldTypes: FieldTypeVm[] = [];
  subs: Subscription[] = [];

  @ViewChild(MatAccordion) accordion?: MatAccordion;
  @ViewChild(DndComponent) dndComponent?: DndComponent;

  get cover(): FormControl {
    return this.collectionForm.get('cover') as FormControl;
  }

  get title(): FormControl {
    return this.collectionForm.get('title') as FormControl;
  }

  get description(): FormControl {
    return this.collectionForm.get('description') as FormControl;
  }

  get topicId(): FormControl {
    return this.collectionForm.get('topicId') as FormControl;
  }

  get fieldsArray(): FormArray {
    return this.collectionForm.get('fields') as FormArray;
  }

  constructor(
    private topicsClient: TopicsClient,
    private collectionsClient: CollectionsClient,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private route: ActivatedRoute
  ) {
    this.ownerId = this.route.snapshot.params['userId'];

    this.collectionForm = this.formBuilder.group({
      cover: new FormControl(),
      title: new FormControl(null, [Validators.required]),
      description: new FormControl(null, [Validators.required]),
      topicId: new FormControl(null, [Validators.required]),
      fields: this.formBuilder.array([])
    });
  }

  ngOnInit(): void {
    this.subs.push(
      this.topicsClient.getTopics()
      .subscribe(
        (response: Topic[]) => { 
          this.topics = response;
          this.selectedTopics = this.topics;
        }
      ),
      this.collectionsClient.getFieldTypes()
      .subscribe(
        (response: FieldTypeVm[]) => this.fieldTypes = response
      )
    );
  }

  ngOnChanges(): void {
    if (this.seed) {
      this.collectionForm.patchValue(this.seed); 
      this.seed.fieldVMs.forEach(f => this.addField(f));
    }  
  }

  ngOnDestroy() {
    this.subs.forEach(
      s => s.unsubscribe()
    );
  }

  addField(defaultValue?: FieldVm) {
    this.fieldsArray.push(this.createField(defaultValue));
    this.accordion?.closeAll();
  }

  createField(defaultValue?: FieldVm): FormGroup {
    return this.formBuilder.group({
      name: new FormControl({ value: defaultValue?.name, disabled: !!defaultValue }, [Validators.required]),
      fieldTypeId: new FormControl({ value: defaultValue?.fieldTypeId, disabled: !!defaultValue }, [Validators.required])
    });
  }

  topicsFilter(event: KeyboardEvent) {
    const value = (event.target as HTMLInputElement).value.trim();

    if (value) {
      let filter = value.toLowerCase();
      this.selectedTopics = this.topics.filter(t => t.name.toLowerCase().startsWith(filter));
    } else {
      this.selectedTopics = this.topics;
    }
  }

  reset() {
    this.collectionForm.reset();
    this.fieldsArray.clear();
    this.dndComponent?.resetFiles();
  }

  deleteDefaultCover() {
    if (this.seed)
      this.seed.coverUrl = undefined;
  }

  onCoverChange(files: File[]) {
    this.cover.setValue(files[0]);
  }

  onSubmit() {
    if (this.collectionForm.invalid) {
      return
    }

    this.isLoading = true;

    const collection: CollectionVm = {
      id: this.seed?.id ?? '',
      title: this.title.value,
      description: this.description.value,
      coverUrl: this.seed?.coverUrl,
      topicId: this.topicId.value,
      ownerId: this.ownerId ?? this.seed?.ownerId,
      fieldVMs: this.fieldsArray.value as FieldVm[]
    };

    if (this.cover.value instanceof File) {
      const file = this.cover.value as File;
      const fileParameter: FileParameter = {
        data: file,
        fileName: file.name
      };

      this.collectionsClient.uploadCover(fileParameter).subscribe(
        (response: string) => {
          this.isLoading = false;
          collection.coverUrl = response;
          this.submitted.emit(collection);
        },
        () => this.isLoading = false
      );
    } else {
      this.submitted.emit(collection);
    }
  }
}
