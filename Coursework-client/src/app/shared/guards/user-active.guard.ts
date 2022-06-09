import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { AuthStorage } from "../services/auth.storage";

@Injectable()
export class UserActiveGuard implements CanActivate {
    constructor(
        private authStorage: AuthStorage,
        private router: Router
    ) {}
        
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): 
        boolean | Observable<boolean> | Promise<boolean> {
        const currentUser = this.authStorage.currentUser;

        if (currentUser) {
            if (currentUser.userState == 'Blocked') {
                this.authStorage.logout();
                this.router.navigate(['auth'], {
                    queryParams: {
                        "user-blocked": true
                    }
                });

                return false;
            }
        }
        
        return true;
    }
}