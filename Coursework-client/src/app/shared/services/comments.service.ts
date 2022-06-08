import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { Comment } from "../interfaces";

@Injectable() 
export class CommentsService {
  constructor(private http: HttpClient) {}

  getComments(itemId: string): Observable<Comment[]> {    
    return this.http.get<Comment[]>(`${environment.apiHost}/api/comments/${itemId}`);
  }

}