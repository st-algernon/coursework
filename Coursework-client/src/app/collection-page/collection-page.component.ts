import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { AddItemDialogComponent } from 'src/app/shared/components/add-item-dialog/add-item-dialog.component';
import { CollectionsClient, ItemsClient, ShortCollectionVm, ShortItemVm, TagVm, UserVm } from '../shared/services/api.service';
import { AuthStorage } from '../shared/services/auth.storage';

@Component({
  selector: 'app-collection-page',
  templateUrl: './collection-page.component.html',
  styleUrls: ['./collection-page.component.scss']
})
export class CollectionPageComponent implements OnInit {
  isSmallResolution?: boolean;
  currentUser?: UserVm;
  collection?: ShortCollectionVm;
  collectionTags: TagVm[] = [];
  items: ShortItemVm[] = [];
  subs: Subscription[] = [];

  constructor(
    private route: ActivatedRoute,
    private collectionsClient: CollectionsClient,
    private itemsClient: ItemsClient,
    private dialog: MatDialog,
    private router: Router,
    private translate: TranslateService,
    private authStorage: AuthStorage,
    private responsive: BreakpointObserver
  ) { 
    translate.addLangs(['en', 'ru']);
    translate.setDefaultLang('en');

    this.authStorage.lang$.subscribe(
      (response: string) => this.translate.use(response)
    );
  }

  ngOnInit(): void {
    const collectionId = this.route.snapshot.params['id'];

    this.subs.push(
      this.collectionsClient.getShortCollection(collectionId).subscribe(
        (response: ShortCollectionVm) => this.collection = response
      ),
      this.collectionsClient.getCollectionTags(collectionId).subscribe(
        (response: TagVm[]) => this.collectionTags = response
      ),
      this.itemsClient.getShortItems(collectionId).subscribe(
        (response: ShortItemVm[]) => this.items = response
      ),
      this.authStorage.currentUser$.subscribe(
        (response: UserVm) => this.currentUser = response
      )
    );

    this._observeSmallResolution();
  }

  ngOnDestroy(): void {
    this.subs.forEach(
      s => s.unsubscribe()
    );
  }

  openAddItemDialog() {
    this.dialog.open(AddItemDialogComponent, { 
      width: this.isSmallResolution ? '100%' : '50%', 
      data: this.collection?.id 
    });
  }

  deleteCollection() {
    if (this.collection?.id) {
      this.subs.push(
        this.collectionsClient.removeCollection(this.collection?.id).subscribe(
          () => this.router.navigate(['/', this.currentUser?.id])
        )
      );
    }
  }

  private _observeSmallResolution() {
    this.subs.push(
      this.responsive.observe([Breakpoints.XSmall, Breakpoints.Small]).subscribe(
        result => {
        if (result.matches) {
          this.isSmallResolution = true;
        } else {
          this.isSmallResolution = false;
        }
      })
    );
  }
}
