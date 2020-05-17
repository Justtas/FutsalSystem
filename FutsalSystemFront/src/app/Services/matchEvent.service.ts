import { Observable } from 'rxjs';
import { BaseService } from './base.service';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatchEventDto } from '../Models/MatchEventDto';

@Injectable({
    providedIn: 'root'
})

export class MatchEventService extends BaseService {

    constructor(http: HttpClient) {
        super(http);
    }

    public getMatchEvents(url: string): Observable<MatchEventDto[]>{
        return this.get<any>(url);
    }
    
    public getMatchEvent(url: string): Observable<MatchEventDto>{
        return this.get<any>(url);
    }

    public postMatchEvent(url:string, matchEvent : any): Observable<any>{
        return this.post<any>(url, matchEvent);
    }

    public deleteMatchEvent(url: string): Observable<any> {
        return this.delete<any>(url);
    }
    
    public UpdateMatchEvent(url: string, matchEvent: any): Observable<any> {
        return this.update(url, matchEvent);
    }
}