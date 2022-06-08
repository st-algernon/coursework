import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAccordion } from '@angular/material/expansion';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { concatMap, forkJoin, Observable, Subscription, switchMap } from 'rxjs';
import { CollectionFormComponent } from 'src/app/shared/components/collection-form/collection-form.component';
import { DndComponent } from 'src/app/shared/components/dnd/dnd.component';
import { Topic, FieldType, CollectionRequest, Field, User, ShortCollection, Collection } from 'src/app/shared/interfaces';
import { AuthStorage } from 'src/app/shared/services/auth.storage';
import { CollectionsService } from 'src/app/shared/services/collections.service';
import { TopicsService } from 'src/app/shared/services/topics.service';

@Component({
  selector: 'app-create-collection-page',
  templateUrl: './create-collection-page.component.html',
  styleUrls: ['./create-collection-page.component.scss']
})
export class CreateCollectionPageComponent implements OnInit, OnDestroy {
  subs: Subscription[] = [];

  @ViewChild(CollectionFormComponent) collectionForm?: CollectionFormComponent;

  constructor(
    private collectionsService: CollectionsService,
    private snackBar: MatSnackBar,
    private translate: TranslateService,
    private authStorage: AuthStorage
  ) { 
    translate.addLangs(['en', 'ru']);
    translate.setDefaultLang('en');
    
    this.authStorage.lang$.subscribe(
      (response: string) => this.translate.use(response)
    );
  }

  ngOnInit(): void { 

  }

  ngOnDestroy() {
    this.subs.forEach(
      s => s.unsubscribe()
    );
  }

  onSubmit(collection: Collection) {
    if (this.collectionForm) {
      this.collectionForm.isLoading = true;

      this.subs.push(
        this.collectionsService.createCollection(collection).subscribe(
          () => {
            this.snackBar.open('The collection was created successfully', '', { duration: 3000 });
            
            if (this.collectionForm) {
              this.collectionForm.isLoading = false;
              this.collectionForm.reset();
            }
          }
        )
      );
    }
  }
}
