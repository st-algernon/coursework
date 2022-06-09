import { LocationStrategy, PathLocationStrategy } from '@angular/common';
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { AuthStorage } from '../services/auth.storage';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { ItemsClient, SearchBy, UsersClient, UserVm } from '../services/api.service';
import { SearchItemsQuery } from '../interfaces';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss'],
})
export class LayoutComponent implements OnInit, OnDestroy {
  isXSmallResolution?: boolean;
  isSearchBarHidden?: boolean = true;
  searchForm: FormGroup;
  currentUser?: UserVm;
  langCtrl = new FormControl(localStorage.lang);
  subs: Subscription[] = [];

  get searchCtrl(): FormControl {
    return this.searchForm.get('search') as FormControl;
  }

  get searchByCtrl(): FormControl {
    return this.searchForm.get('searchBy') as FormControl;
  }

  constructor(
    private itemsClient: ItemsClient,
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
      this.authStorage.currentUser$.subscribe((response: UserVm) => this.currentUser = response)
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
    this.authStorage.logout();
    this.router.navigate(['/auth']);
  }

  onSubmit() {
    if (this.searchForm.invalid)
      return;

    const request: SearchItemsQuery = {
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
