import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, Subject, throwError } from "rxjs";
import { catchError, map, tap } from "rxjs/operators";
import { environment } from "src/environments/environment";
import { AuthResponse, LoginRequest, RegisterRequest, RefreshTokenRequest } from "../interfaces";
import { AuthStorage } from "./auth.storage";

@Injectable({ providedIn: "root" })
export class AuthService {

  constructor(
    private http: HttpClient, 
    private authStorage: AuthStorage
  ) {}

  login(loginRequest: LoginRequest) {
    return this.http.post<AuthResponse>(`${environment.apiHost}/api/auth/login`, loginRequest)
    .pipe(
      tap(this.authStorage.setTokens),
      catchError((error) => {
        console.log(error);
        return throwError(error);
      })
    );
  }

  register(registerRequest: RegisterRequest) {
    return this.http.post<AuthResponse>(`${environment.apiHost}/api/auth/register`, registerRequest)
    .pipe(
      tap(this.authStorage.setTokens),
      catchError((error) => {
        console.log(error);
        return throwError(error);
      })
    );
  }

  refreshToken() {
    const request: RefreshTokenRequest = {
      accessToken: this.authStorage.accessToken as string,
      refreshToken: this.authStorage.refreshToken as string
    }

    return this.http.put<AuthResponse>(`${environment.apiHost}/api/auth/refresh`, request)
      .pipe(
        tap(this.authStorage.setTokens)
      );
  }

  logout() {
    this.authStorage.setTokens(null);
  }

  isAuthenticated(): boolean {
    return !!this.authStorage.accessToken;
  }
}
