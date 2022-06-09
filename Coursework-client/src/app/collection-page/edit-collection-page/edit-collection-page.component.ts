import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { CollectionFormComponent } from 'src/app/shared/components/collection-form/collection-form.component';
import { CollectionsClient, CollectionVm } from 'src/app/shared/services/api.service';
import { AuthStorage } from 'src/app/shared/services/auth.storage';

@Component({
  selector: 'app-edit-collection-page',
  templateUrl: './edit-collection-page.component.html',
  styleUrls: ['./edit-collection-page.component.scss']
})
export class EditCollectionPageComponent implements OnInit {
  seed?: CollectionVm;
  subs: Subscription[] = [];

  @ViewChild(CollectionFormComponent) collectionForm?: CollectionFormComponent;

  constructor(
    private collectionsClient: CollectionsClient,
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
      this.collectionsClient.getCollection(id).subscribe(
        (response: CollectionVm) => this.seed = response
      )
    );
  }

  ngOnDestroy(): void {
    this.subs.forEach(
      u => u.unsubscribe()
    );
  }

  onSubmit(collection: CollectionVm) {
    this.subs.push(
      this.collectionsClient.editCollection(collection).subscribe(
        () => {
          this.snackBar.open('The collection was edited successfully', '', { duration: 3000 });
          this.collectionForm?.reset();
          this.router.navigate(['../'],  { relativeTo: this.route });
        }
      )
    );
  }
}
