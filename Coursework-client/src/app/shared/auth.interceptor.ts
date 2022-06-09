import { HttpErrorResponse, HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Observable, throwError } from "rxjs";
import { catchError, switchMap } from "rxjs/operators";
import { AuthClient, RefreshTokenQuery } from "./services/api.service";
import { AuthStorage } from "./services/auth.storage";

@Injectable()
export class AuthInterceptor implements HttpInterceptor{

    constructor(
        private authStorage: AuthStorage,
        private authClient: AuthClient,
        private router: Router
    ) {}
    
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (this.authStorage.isAuthenticated()) {
            req = this._addTokenToHeader(req);
        }

        return next.handle(req).pipe(
            catchError((error: HttpErrorResponse) => {
                console.log('Interceptor Error: ', error)

                if (error.status === 401 && !req.url.includes('auth')) {
                    const request: RefreshTokenQuery = {
                        accessToken: this.authStorage.accessToken as string,
                        refreshToken: this.authStorage.refreshToken as string
                    };

                    return this.authClient.refreshToken(request).pipe(
                        switchMap(() => next.handle(this._addTokenToHeader(req))),
                        catchError((err) => {
                            this.authStorage.logout();

                            return throwError(err);
                        })
                    );

                }
                
                return throwError(error)
            })
        )
    }

    private _addTokenToHeader(req: HttpRequest<any>) {
        return req.clone({
            setHeaders: {
                Authorization: `Bearer ${this.authStorage.accessToken}`
            }
        });
    }
}