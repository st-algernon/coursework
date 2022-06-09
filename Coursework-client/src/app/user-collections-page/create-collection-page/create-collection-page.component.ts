import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { CollectionFormComponent } from 'src/app/shared/components/collection-form/collection-form.component';
import { CollectionsClient, CollectionVm } from 'src/app/shared/services/api.service';
import { AuthStorage } from 'src/app/shared/services/auth.storage';

@Component({
  selector: 'app-create-collection-page',
  templateUrl: './create-collection-page.component.html',
  styleUrls: ['./create-collection-page.component.scss']
})
export class CreateCollectionPageComponent implements OnInit, OnDestroy {
  subs: Subscription[] = [];

  @ViewChild(CollectionFormComponent) collectionForm?: CollectionFormComponent;

  constructor(
    private collectionsClient: CollectionsClient,
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

  onSubmit(collection: CollectionVm) {
    if (this.collectionForm) {
      this.collectionForm.isLoading = true;

      this.subs.push(
        this.collectionsClient.createCollection(collection).subscribe(
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
