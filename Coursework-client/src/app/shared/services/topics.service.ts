import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { Topic } from "../interfaces";

@Injectable() 
export class TopicsService {
    constructor(private http: HttpClient) {}

    getTopics(): Observable<Topic[]> {
        return this.http.get<Topic[]>(`${environment.apiHost}/api/topics`);
    }
}