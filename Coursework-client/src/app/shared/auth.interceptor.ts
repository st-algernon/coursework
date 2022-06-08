import { HttpErrorResponse, HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Observable, throwError } from "rxjs";
import { catchError, switchMap } from "rxjs/operators";
import { AuthService } from "./services/auth.service";
import { AuthStorage } from "./services/auth.storage";

@Injectable()
export class AuthInterceptor implements HttpInterceptor{

    constructor(
        private auth: AuthService,
        private authStorage: AuthStorage,
        private router: Router
    ) {}
    
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (this.auth.isAuthenticated()) {
            req = this._addTokenToHeader(req);
        }

        return next.handle(req).pipe(
            catchError((error: HttpErrorResponse) => {
                console.log('Interceptor Error: ', error)

                if (error.status === 401 && !req.url.includes('auth')) {
                    return this.auth.refreshToken().pipe(
                        switchMap(() => next.handle(this._addTokenToHeader(req))),
                        catchError((err) => {
                            this.auth.logout();

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