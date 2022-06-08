import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, tap } from "rxjs";
import { environment } from "src/environments/environment";
import { Item, SearchRequest, ShortItem, Tag } from "../interfaces";

@Injectable({providedIn: 'root'}) 
export class ItemsService {
  constructor(private http: HttpClient) {}

  addItem(request: ShortItem) {    
    return this.http.post(`${environment.apiHost}/api/items/create`, request);
  }

  editItem(request: ShortItem) {    
    return this.http.put(`${environment.apiHost}/api/items/edit`, request);
  }

  deleteItem(id: string) {
    return this.http.delete(`${environment.apiHost}/api/items/${id}`);
  }

  likeItem(itemId: string) {
    return this.http.put(`${environment.apiHost}/api/items/like/${itemId}`, null);
  }

  getItems(collectionId: string): Observable<Item[]> {
    const params = new HttpParams()
    .set('collectionId', collectionId);

    return this.http.get<Item[]>(`${environment.apiHost}/api/items`, { params });
  }

  getShortItems(collectionId: string): Observable<ShortItem[]> {
    const params = new HttpParams()
    .set('collectionId', collectionId);

    return this.http.get<ShortItem[]>(`${environment.apiHost}/api/items/short`, { params });
  }

  getLastItems(page: number = 1, size: number = 10): Observable<ShortItem[]> {
    const params = new HttpParams()
    .set('Page', page.toString())
    .set('Size', size.toString());

    return this.http.get<ShortItem[]>(`${environment.apiHost}/api/items/last`, { params });
  }

  searchItems(request: SearchRequest): Observable<ShortItem[]> {
    const params = new HttpParams()
    .set('query', request.query)
    .set('searchBy', request.searchBy);

    return this.http.get<ShortItem[]>(`${environment.apiHost}/api/items/search`, { params });
  }
  
  getItem(itemId: string): Observable<Item> {
    return this.http.get<Item>(`${environment.apiHost}/api/items/${itemId}`);
  }

  searchTags(query: string): Observable<Tag[]> {   
    return this.http.get<Tag[]>(`${environment.apiHost}/api/items/search-tags/${query}`);
  }

  getTopTags(): Observable<Tag[]> {   
    return this.http.get<Tag[]>(`${environment.apiHost}/api/items/top-tags`);
  }

  uploadCover(formData: FormData) {    
    return this.http.post(`${environment.apiHost}/api/items/cover`, formData, { responseType: 'text' });
  }
}