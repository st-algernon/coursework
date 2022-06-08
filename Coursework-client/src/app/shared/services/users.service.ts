import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, ReplaySubject } from "rxjs";
import { tap } from "rxjs/operators";
import { environment } from "src/environments/environment";
import { User } from "../interfaces";

@Injectable({ providedIn: 'root' }) 
export class UsersService {
    private _currentUser$ = new ReplaySubject<User>();

    get currentUser$(): ReplaySubject<User> {
        if (localStorage.currentUser == null) {
            this.getCurrentUser().subscribe(
                (response: User) => {
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

    constructor(
        private http: HttpClient
    ) {}

    getUser(id: string): Observable<User> {
        return this.http.get<User>(`${environment.apiHost}/api/users/${id}`);
    }

    searchUsers(query: string): Observable<User[]> {
        return this.http.get<User[]>(`${environment.apiHost}/api/users/search/${query}`)
        .pipe(tap(console.log));
    } 

    getUsers(page: number, size?: number): Observable<User[]> {
        const params = new HttpParams()
        .set('Page', page.toString())
        .set('Size', size ? size.toString() : '10');

        return this.http.get<User[]>(`${environment.apiHost}/api/admin/users`, { params })
    } 

    addAdmin(id: string) {
        return this.http.put(`${environment.apiHost}/api/admin/add/${id}`, null);
    } 

    blockUser(id: string) {
        return this.http.put(`${environment.apiHost}/api/admin/users/block/${id}`, null);
    }
    
    unblockUser(id: string) {
        return this.http.put(`${environment.apiHost}/api/admin/users/unblock/${id}`, null);
    }

    removeUser(id: string) {
        return this.http.delete(`${environment.apiHost}/api/admin/users/remove/${id}`);
    } 

    getUsersCount(): Observable<number> {
        return this.http.get<number>(`${environment.apiHost}/api/admin/users/count`)
    }

    private getCurrentUser(): Observable<User> {
        return this.http.get<User>(`${environment.apiHost}/api/users/current`);
    }
}