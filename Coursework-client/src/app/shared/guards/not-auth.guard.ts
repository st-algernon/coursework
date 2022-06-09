import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { AuthStorage } from "../services/auth.storage";

@Injectable()
export class NotAuthGuard implements CanActivate {
    constructor(
        private authStorage: AuthStorage,
        private router: Router
    ) {}
        
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): 
        boolean | Observable<boolean> | Promise<boolean> {
        if (!this.authStorage.isAuthenticated()) {
            return true;

        } else {
            this.router.navigate(['']);
            return false;
        }
    }
}