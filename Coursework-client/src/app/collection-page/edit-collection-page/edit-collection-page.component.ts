import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { CollectionFormComponent } from 'src/app/shared/components/collection-form/collection-form.component';
import { Collection } from 'src/app/shared/interfaces';
import { AuthStorage } from 'src/app/shared/services/auth.storage';
import { CollectionsService } from 'src/app/shared/services/collections.service';

@Component({
  selector: 'app-edit-collection-page',
  templateUrl: './edit-collection-page.component.html',
  styleUrls: ['./edit-collection-page.component.scss']
})
export class EditCollectionPageComponent implements OnInit {
  seed?: Collection;
  subs: Subscription[] = [];

  @ViewChild(CollectionFormComponent) collectionForm?: CollectionFormComponent;

  constructor(
    private collectionsService: CollectionsService,
    private route: ActivatedRoute,
    private router: Router,
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
    const id = this.route.snapshot.params['id'];

    this.subs.push(
      this.collectionsService.getCollection(id).subscribe(
        (response: Collection) => this.seed = response
      )
    );
  }

  ngOnDestroy(): void {
    this.subs.forEach(
      u => u.unsubscribe()
    );
  }

  onSubmit(collection: Collection) {
    this.subs.push(
      this.collectionsService.editCollection(collection).subscribe(
        () => {
          this.snackBar.open('The collection was edited successfully', '', { duration: 3000 });
          this.collectionForm?.reset();
          this.router.navigate(['../'],  { relativeTo: this.route });
        }
      )
    );
  }
}
