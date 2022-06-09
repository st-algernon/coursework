import { SelectionChange, SelectionModel } from '@angular/cdk/collections';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { PageEvent } from '@angular/material/paginator';
import { MatTable } from '@angular/material/table';
import { TranslateService } from '@ngx-translate/core';
import { Observable, Subscription } from 'rxjs';
import { UsersClient, UserVm } from 'src/app/shared/services/api.service';
import { AuthStorage } from 'src/app/shared/services/auth.storage';

@Component({
  selector: 'app-users-page',
  templateUrl: './users-page.component.html',
  styleUrls: ['./users-page.component.scss']
})
export class UsersPageComponent implements OnInit, OnDestroy {

  @ViewChild(MatTable) table: MatTable<UserVm> | undefined;

  isButtonsDisabled = {
    adminBtn: true,
    blockBtn: true,
    unblockBtn: true,
    removeBtn: true
  }

  isLoading: boolean = false;
  displayedColumns = ['checkbox', 'name', 'email', 'userrole', 'userstate'];
  selection = new SelectionModel<UserVm>(true);
  users: UserVm[] = [];

  allUsersCount: number | undefined;
  pageIndex: number = 0;
  pageSize: number = 5;
  pageSizeOptions: number[] = [5, 10, 25, 50];

  subs: Subscription[] = [];

  constructor(
    private usersClient: UsersClient,
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
    this.loadUsers();
    this.loadUsersCount();
    this.subs.push(
      this.selection.changed.subscribe(
        (change: SelectionChange<UserVm>) => this.configActionButtons(change.source.selected)
      )
    );
  }

  ngOnDestroy(): void {
    this.subs.forEach(
      s => s.unsubscribe()
    );
  }

  loadUsers() {
    this.isLoading = true;

    this.subs.push(
      this.usersClient.getUsers(this.pageIndex + 1, this.pageSize).subscribe(
        (response: UserVm[]) => { 
          console.log(response);
          this.users = response 
          this.isLoading = false;
        },
        () => this.isLoading = false
      )
    );
  }

  loadUsersCount() {
    this.subs.push(
      this.usersClient.getUsersCount().subscribe(
        (response: number) => this.allUsersCount = response
      )
    );
  }

  searchUser(event: Event) {
    const query = (event.target as HTMLInputElement).value;

    if(query.trim() != '') {
      this.subs.push(
        this.usersClient.searchUsers(query).subscribe(
          (response: UserVm[]) => this.users = response
        )
      )
    } else {
      this.loadUsers();
    }
  }

  appointAdmin() {
    this.selection.selected.forEach(su => {
      this.subs.push(
        this.usersClient.addAdmin(su.id).subscribe(
          () => { 
            su.userRole = 'Admin';
            this.table?.renderRows();
          }
        )
      );
    });
  }

  blockUser() {
    this.selection.selected.forEach(su => {
      this.subs.push(
        this.usersClient.blockUser(su.id).subscribe(
          () => {
            su.userState = 'Blocked';
            this.table?.renderRows();
          }
        )
      );
    });
  }

  
  unblockUser() {
    this.selection.selected.forEach(su => {
      this.subs.push(
        this.usersClient.unblockUser(su.id).subscribe(
          () => {
            su.userState = 'Active';
            this.table?.renderRows();
          }
        )
      );
    });
  }

  removeUser() {
    this.selection.selected.forEach(su => {
      this.subs.push(
        this.usersClient.removeUser(su.id).subscribe(
          () => { 
            this.users = this.users.filter(u => u !== su);
            this.table?.renderRows();
          }
        )
      );
    });
  }

  pageChanged(event: PageEvent) {
    this.pageSize = event.pageSize;
    this.pageIndex = event.pageIndex;
    this.loadUsers();
  }

  isAllSelected() {
    return this.selection.selected.length === this.users.length;
  }
  
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.users.forEach(u => this.selection.select(u));
  }

  configActionButtons(selectedUsers: UserVm[]) {
    if (selectedUsers.length > 0) {
      this._configRemoveButton(selectedUsers);

      this._configAdminButton(selectedUsers);

      this._configBlockButton(selectedUsers);

      this._configUnblockButton(selectedUsers);
    } else {
      this._resetButtonConfigs();
    }
  }

  private _configAdminButton(selectedUsers: UserVm[]) {
    if (selectedUsers.some(u => u.userRole == 'Admin'))
      this.isButtonsDisabled.adminBtn = true;
    else
      this.isButtonsDisabled.adminBtn = false;
  }

  private _configBlockButton(selectedUsers: UserVm[]) {
    if (selectedUsers.some(u => u.userState == 'Blocked') 
        || selectedUsers.some(u => u.userRole == 'Admin'))
      this.isButtonsDisabled.blockBtn = true;
    else 
      this.isButtonsDisabled.blockBtn = false;
  }

  private _configUnblockButton(selectedUsers: UserVm[]) {
    if (selectedUsers.some(u => u.userState == 'Active'))
      this.isButtonsDisabled.unblockBtn = true;
    else 
      this.isButtonsDisabled.unblockBtn = false;
  }

  private _configRemoveButton(selectedUsers: UserVm[]) {
    if (selectedUsers.some(u => u.userRole == 'Admin'))
      this.isButtonsDisabled.removeBtn = true;
    else 
      this.isButtonsDisabled.removeBtn = false;
  }

  private _resetButtonConfigs() {
    this.isButtonsDisabled = {
      adminBtn: true,
      blockBtn: true,
      unblockBtn: true,
      removeBtn: true
    }
  }
}
