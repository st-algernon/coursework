import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, ReplaySubject, Subject, throwError } from "rxjs";
import { catchError, map, tap } from "rxjs/operators";
import { environment } from "src/environments/environment";
import { AuthResponse, UsersClient, UserVm } from "./api.service";

@Injectable({ providedIn: "root" })
export class AuthStorage {

  public lang$ = new Subject<string>();
  private _currentUser$ = new ReplaySubject<UserVm>();

  get currentUser$(): ReplaySubject<UserVm> {
      if (localStorage.currentUser == null) {
          this.usersClient.getCurrentUser().subscribe(
              (response: UserVm) => {
                  localStorage.currentUser = JSON.stringify(response);
              },
              (error) => {
                  console.log(error);
              },
              () => { 
                  this._currentUser$.next(JSON.parse(localStorage.currentUser));
              }
          );
      } else {
          this._currentUser$.next(JSON.parse(localStorage.currentUser));
      }

      return this._currentUser$;
  }

  get accessToken(): string | null {

    return localStorage.getItem('access-token');
  }

  get refreshToken(): string | null {
    return localStorage.getItem('refresh-token');
  }

  get currentUser(): UserVm | null {
    if (localStorage.currentUser) {
      return JSON.parse(localStorage.currentUser);
    } 

    return null;
  }

  constructor(
    private http: HttpClient,
    private usersClient: UsersClient
  ) {}

  setTokens(response: AuthResponse | null) {
    if (response) {
      localStorage.setItem("access-token", response.accessToken);
      localStorage.setItem("refresh-token", response.refreshToken);
    } else {
      localStorage.clear();
    }
  }

  logout() {
    this.setTokens(null);
  }

  isAuthenticated(): boolean {
    return !!this.accessToken;
  }

  setLang(lang: string) {
    if (lang) {
      localStorage.setItem('lang', lang);

      this.lang$.next(lang);
    }
  }
}
