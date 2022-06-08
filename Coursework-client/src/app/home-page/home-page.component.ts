import { Component, OnDestroy, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { SearchBy } from '../shared/enums';
import { Item, ShortCollection, ShortItem, Tag } from '../shared/interfaces';
import { AuthStorage } from '../shared/services/auth.storage';
import { CollectionsService } from '../shared/services/collections.service';
import { ItemsService } from '../shared/services/items.service';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit, OnDestroy {
  lastAddedItems: {
    items: ShortItem[]
    pageSize: number
  };
  largestCollections: {
    items: ShortCollection[]
    pageSize: number
  };
  topTags: Tag[] = [];
  subs: Subscription[] = [];

  constructor(
    private collectionsService: CollectionsService,
    private itemsService: ItemsService,
    private translate: TranslateService,
    private authStorage: AuthStorage
  ) { 
    translate.addLangs(['en', 'ru']);
    translate.setDefaultLang('en');
    
    this.authStorage.lang$.subscribe(
      (response: string) => this.translate.use(response)
    );
    
    this.lastAddedItems = {
      items: [],
      pageSize: 3
    };

    this.largestCollections = {
      items: [],
      pageSize: 2
    };
  }

  ngOnInit(): void {
    this.subs.push(
      this.collectionsService.getLargestCollections().subscribe(
        (response: ShortCollection[]) => this.largestCollections.items = response
      ),
      this.itemsService.getLastItems().subscribe(
        (response: ShortItem[]) => this.lastAddedItems.items = response
      ),
      this.itemsService.getTopTags().subscribe(
        (response: Tag[]) => this.topTags = response
      )
    );
  }

  ngOnDestroy(): void {
    this.subs.forEach(
      s => s.unsubscribe()
    )
  }

  loadMoreItems() {
    this.lastAddedItems.pageSize += this.lastAddedItems.pageSize;
  }

  loadMoreCollections() {
    this.largestCollections.pageSize += this.largestCollections.pageSize;
  }
}
