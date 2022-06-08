import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { switchMap } from 'rxjs/operators';
import { ShortItem } from '../shared/interfaces';
import { AuthStorage } from '../shared/services/auth.storage';
import { ItemsService } from '../shared/services/items.service';

@Component({
  selector: 'app-search-page',
  templateUrl: './search-page.component.html',
  styleUrls: ['./search-page.component.scss']
})
export class SearchPageComponent implements OnInit {

  foundItems: ShortItem[] = [];

  constructor(
    private route: ActivatedRoute,
    private itemsService: ItemsService,
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
      switchMap((params: Params) => this.itemsService.searchItems({query: params['query'], searchBy: params['searchBy']}))
    ).subscribe(
      (response: ShortItem[]) => { 
        this.foundItems = response;
      }
    );
  }

}
