import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { ShortCollection, User } from 'src/app/shared/interfaces';
import { CollectionsService } from 'src/app/shared/services/collections.service';
import { AuthStorage } from '../shared/services/auth.storage';
import { UsersService } from '../shared/services/users.service';

@Component({
  selector: 'app-user-collections-page',
  templateUrl: './user-collections-page.component.html',
  styleUrls: ['./user-collections-page.component.scss']
})
export class UserCollectionsPageComponent implements OnInit {

  user?: User;
  currentUser?: User;
  collections: ShortCollection[] = [];
  subs: Subscription[] = [];

  constructor(
    private collectionsService: CollectionsService,
    private usersService: UsersService,
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
      this.collectionsService.getUserCollections(userId).subscribe(
        (response: ShortCollection[]) => this.collections = response
      ),
      this.usersService.currentUser$.subscribe(
        (response: User) => this.currentUser = response
      ),
      this.usersService.getUser(userId).subscribe(
        (response: User) => this.user = response
      )
    );
  }

  ngOnDestroy(): void {
    this.subs.forEach(
      s => s.unsubscribe()
    );
  }
}
