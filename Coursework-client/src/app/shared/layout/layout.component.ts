import { LocationStrategy, PathLocationStrategy } from '@angular/common';
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { SearchBy } from '../enums';
import { SearchRequest, User } from '../interfaces';
import { AuthService } from '../services/auth.service';
import { AuthStorage } from '../services/auth.storage';
import { ItemsService } from '../services/items.service';
import { UsersService } from '../services/users.service';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss'],
})
export class LayoutComponent implements OnInit, OnDestroy {
  isXSmallResolution?: boolean;
  isSearchBarHidden?: boolean = true;
  searchForm: FormGroup;
  currentUser?: User;
  langCtrl = new FormControl(localStorage.lang);
  subs: Subscription[] = [];

  get searchCtrl(): FormControl {
    return this.searchForm.get('search') as FormControl;
  }

  get searchByCtrl(): FormControl {
    return this.searchForm.get('searchBy') as FormControl;
  }

  constructor(
    private usersService: UsersService,
    private itemsService: ItemsService,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private authStorage: AuthStorage,
    private translate: TranslateService,
    private changeDeterctorRef: ChangeDetectorRef,
    private responsive: BreakpointObserver
  ) {
    this.searchForm = new FormGroup({
      search: new FormControl(null, Validators.required),
      searchBy: new FormControl(SearchBy.Title.toString(), Validators.required)
    });

    translate.addLangs(['en', 'ru']);
    translate.setDefaultLang('en');

    this.subs.push(
      this.langCtrl.valueChanges.subscribe(
        (value: string) => this.authStorage.setLang(this.langCtrl.value)
      ),
      this.authStorage.lang$.subscribe(
        (response: string) => this.translate.use(response)
      )
    );

    this._observeXSmallResolution();
  }

  ngOnInit(): void {
    this.subs.push(
      this.usersService.currentUser$.subscribe((response: User) => this.currentUser = response)
    );
    
    this.route.queryParams.subscribe(
      (params: Params) => { 
        if (params['query'] && params['searchBy']) {
          this.searchCtrl.setValue(params['query']);
          this.searchByCtrl.setValue(params['searchBy']);
        }
      }
    );

    this.authStorage.setLang(this.langCtrl.value);
  }

  ngOnDestroy(): void {
    this.subs.forEach(
      s => s.unsubscribe()
    );
  }

  ngAfterViewChecked() {
    this.changeDeterctorRef.detectChanges();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/auth']);
  }

  onSubmit() {
    if (this.searchForm.invalid)
      return;

    const request: SearchRequest = {
      query: this.searchCtrl.value,
      searchBy: this.searchByCtrl.value
    };

    this.router.navigate(['search'], { queryParams: request });
  }

  private _observeXSmallResolution() {
    this.subs.push(
      this.responsive.observe(Breakpoints.XSmall).subscribe
      (result => {
        if (result.matches) {
          this.isXSmallResolution = true;
        } else {
          this.isXSmallResolution = false;
        }
      })
    );
  }
}
