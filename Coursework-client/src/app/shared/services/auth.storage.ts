import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, ReplaySubject, Subject, throwError } from "rxjs";
import { catchError, map, tap } from "rxjs/operators";
import { environment } from "src/environments/environment";
import { AuthResponse, LoginRequest, RegisterRequest, RefreshTokenRequest, User } from "../interfaces";

@Injectable({ providedIn: "root" })
export class AuthStorage {

  public lang$ = new Subject<string>();

  get accessToken(): string | null {

    return localStorage.getItem('access-token');
  }

  get refreshToken(): string | null {
    return localStorage.getItem('refresh-token');
  }

  get currentUser(): User | null {
    if (localStorage.currentUser) {
      return JSON.parse(localStorage.currentUser);
    } 

    return null
  }

  constructor(private http: HttpClient) {}

  setTokens(response: AuthResponse | null) {
    if (response) {
      localStorage.setItem("access-token", response.accessToken);
      localStorage.setItem("refresh-token", response.refreshToken);
    } else {
      localStorage.clear();
    }
  }

  setLang(lang: string) {
    if (lang) {
      localStorage.setItem('lang', lang);

      this.lang$.next(lang);
    }
  }
}
