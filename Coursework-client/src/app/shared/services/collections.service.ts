import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, tap } from "rxjs";
import { environment } from "src/environments/environment";
import { ShortCollection, CollectionRequest, FieldType, Collection, FieldWithTypeName, Tag } from "../interfaces";

@Injectable({ providedIn: 'root' }) 
export class CollectionsService {
  constructor(private http: HttpClient) {}

  createCollection(collection: Collection) {    
    return this.http.post(`${environment.apiHost}/api/collections/create`, collection);
  }

  editCollection(collection: Collection) {    
    return this.http.put(`${environment.apiHost}/api/collections/edit`, collection);
  }

  deleteCollection(id: string) {    
    return this.http.delete(`${environment.apiHost}/api/collections/${id}`);
  }

  getShortCollection(id: string): Observable<ShortCollection> {
      return this.http.get<ShortCollection>(`${environment.apiHost}/api/collections/short/${id}`);
  }
  
  getCollection(id: string): Observable<Collection> {
    return this.http.get<Collection>(`${environment.apiHost}/api/collections/${id}`);
  }

  getUserCollections(userId: string): Observable<ShortCollection[]> {
    return this.http.get<ShortCollection[]>(`${environment.apiHost}/api/collections/owner/${userId}`);
  }

  getLargestCollections(): Observable<ShortCollection[]> {
    return this.http.get<ShortCollection[]>(`${environment.apiHost}/api/collections/largest`);
  }

  getCollectionFields(id: string): Observable<FieldWithTypeName[]> {
    return this.http.get<FieldWithTypeName[]>(`${environment.apiHost}/api/collections/${id}/fields`);
  }

  getCollectionTags(id: string): Observable<Tag[]> {
    return this.http.get<Tag[]>(`${environment.apiHost}/api/collections/${id}/tags`);
  }

  getFieldTypes(): Observable<FieldType[]> {
    return this.http.get<FieldType[]>(`${environment.apiHost}/api/collections/field-types`);
  }

  uploadCover(formData: FormData) {    
    return this.http.post(`${environment.apiHost}/api/collections/cover`, formData, { responseType: 'text' });
  }
}