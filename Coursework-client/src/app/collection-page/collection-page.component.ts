import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subscription, switchMap, tap } from 'rxjs';
import { AddItemDialogComponent } from 'src/app/shared/components/add-item-dialog/add-item-dialog.component';
import { ItemsService } from 'src/app/shared/services/items.service';
import { EditItemDialogComponent } from '../shared/components/edit-item-dialog/edit-item-dialog.component';
import { Item, ShortCollection, ShortItem, Tag, User } from '../shared/interfaces';
import { AuthStorage } from '../shared/services/auth.storage';
import { CollectionsService } from '../shared/services/collections.service';
import { UsersService } from '../shared/services/users.service';

@Component({
  selector: 'app-collection-page',
  templateUrl: './collection-page.component.html',
  styleUrls: ['./collection-page.component.scss']
})
export class CollectionPageComponent implements OnInit {
  isSmallResolution?: boolean;
  currentUser?: User;
  collection?: ShortCollection;
  collectionTags: Tag[] = [];
  items: ShortItem[] = [];
  subs: Subscription[] = [];

  constructor(
    private route: ActivatedRoute,
    private collectionsService: CollectionsService,
    private itemsService: ItemsService,
    private dialog: MatDialog,
    private router: Router,
    private translate: TranslateService,
    private authStorage: AuthStorage,
    private responsive: BreakpointObserver,
    private usersService: UsersService
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
      this.collectionsService.getShortCollection(collectionId).subscribe(
        (response: ShortCollection) => this.collection = response
      ),
      this.collectionsService.getCollectionTags(collectionId).subscribe(
        (response: Tag[]) => this.collectionTags = response
      ),
      this.itemsService.getShortItems(collectionId).subscribe(
        (response: ShortItem[]) => this.items = response
      ),
      this.usersService.currentUser$.subscribe(
        (response: User) => this.currentUser = response
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
        this.collectionsService.deleteCollection(this.collection?.id).subscribe(
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
