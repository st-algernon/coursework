import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { AuthStorage } from "../services/auth.storage";

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(
        private authStorage: AuthStorage,
        private router: Router
    ) {}
        
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): 
        boolean | Observable<boolean> | Promise<boolean> {
        if (this.authStorage.isAuthenticated()) {
            const userState = this.authStorage.currentUser?.userState;
            
            if (userState == 'Blocked') {
                this.authStorage.logout();
                this.router.navigate(['auth'], {
                    queryParams: {
                        "user-blocked": true
                    }
                });

                return false;
            }

            return true;

        } else {
            this.authStorage.logout();
            this.router.navigate(['auth'], {
                queryParams: {
                    "login-again": true
                }
            });

            return false;
        }
    }
}