import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { switchMap } from 'rxjs/operators';
import { ItemsClient, SearchBy, ShortItemVm } from '../shared/services/api.service';
import { AuthStorage } from '../shared/services/auth.storage';

@Component({
  selector: 'app-search-page',
  templateUrl: './search-page.component.html',
  styleUrls: ['./search-page.component.scss']
})
export class SearchPageComponent implements OnInit {

  foundItems: ShortItemVm[] = [];

  constructor(
    private route: ActivatedRoute,
    private itemsClient: ItemsClient,
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
    this.route.queryParams.pipe(
      switchMap((params: Params) => this.itemsClient.searchItems(params['query'], params['searchBy']))
    ).subscribe(
      (response: ShortItemVm[]) => { 
        this.foundItems = response;
      }
    );
  }

}
