import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { CollectionsClient, ShortCollectionVm, UsersClient, UserVm } from '../shared/services/api.service';
import { AuthStorage } from '../shared/services/auth.storage';

@Component({
  selector: 'app-user-collections-page',
  templateUrl: './user-collections-page.component.html',
  styleUrls: ['./user-collections-page.component.scss']
})
export class UserCollectionsPageComponent implements OnInit {

  user?: UserVm;
  currentUser?: UserVm;
  collections: ShortCollectionVm[] = [];
  subs: Subscription[] = [];

  constructor(
    private collectionsClient: CollectionsClient,
    private usersClient: UsersClient,
    private route: ActivatedRoute,
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
    const userId = this.route.snapshot.params['userId'];

    this.subs.push(
      this.collectionsClient.getUserCollections(userId).subscribe(
        (response: ShortCollectionVm[]) => this.collections = response
      ),
      this.authStorage.currentUser$.subscribe(
        (response: UserVm) => this.currentUser = response
      ),
      this.usersClient.getUserById(userId).subscribe(
        (response: UserVm) => this.user = response
      )
    );
  }

  ngOnDestroy(): void {
    this.subs.forEach(
      s => s.unsubscribe()
    );
  }
}
