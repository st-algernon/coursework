import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { AuthService } from "../services/auth.service";
import { AuthStorage } from "../services/auth.storage";

@Injectable()
export class AdminGuard implements CanActivate {
    constructor(
        private auth: AuthService,
        private authStorage: AuthStorage,
        private router: Router
    ) {}
        
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): 
        boolean | Observable<boolean> | Promise<boolean> {
        if (this.auth.isAuthenticated()) {
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