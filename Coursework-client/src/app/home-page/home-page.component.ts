import { Component, OnDestroy, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { CollectionsClient, ItemsClient, ShortCollectionVm, ShortItemVm, TagVm } from '../shared/services/api.service';
import { AuthStorage } from '../shared/services/auth.storage';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit, OnDestroy {
  lastAddedItems: {
    items: ShortItemVm[]
    pageSize: number
  };
  largestCollections: {
    items: ShortCollectionVm[]
    pageSize: number
  };
  topTags: TagVm[] = [];
  subs: Subscription[] = [];

  constructor(
    private collectionsClient: CollectionsClient,
    private itemsClient: ItemsClient,
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
      this.collectionsClient.getLargestCollections().subscribe(
        (response: ShortCollectionVm[]) => this.largestCollections.items = response
      ),
      this.itemsClient.getLastAddedItems().subscribe(
        (response: ShortItemVm[]) => this.lastAddedItems.items = response
      ),
      this.itemsClient.getTopTags().subscribe(
        (response: TagVm[]) => this.topTags = response
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
