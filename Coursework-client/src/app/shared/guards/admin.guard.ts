import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { AuthStorage } from "../services/auth.storage";

@Injectable()
export class AdminGuard implements CanActivate {
    constructor(
        private authStorage: AuthStorage,
        private router: Router
    ) {}
        
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): 
        boolean | Observable<boolean> | Promise<boolean> {
        if (this.authStorage.isAuthenticated()) {
            const currentUser = this.authStorage.currentUser;

            if (currentUser) {
                if (currentUser.userRole == 'Admin') {
                    return true;
                }
            }
        }
        
        return false;
    }
}