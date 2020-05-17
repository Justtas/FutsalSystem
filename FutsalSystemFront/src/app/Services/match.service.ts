import { Observable } from 'rxjs';
import { BaseService } from './base.service';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatchDto } from '../Models/MatchDto';

@Injectable({
    providedIn: 'root'
})

export class MatchService extends BaseService {

    constructor(http: HttpClient) {
        super(http);
    }

    public getMatches(url: string): Observable<MatchDto[]>{
        return this.get<any>(url);
    }
    
    public getMatch(url: string): Observable<MatchDto>{
        return this.get<any>(url);
    }

    public postMatch(url:string, match : any): Observable<any>{
        return this.post<any>(url, match);
    }

    public deleteMatch(url: string): Observable<any> {
        return this.delete<any>(url);
    }
    
    public UpdateMatch(url: string, match: any): Observable<any> {
        return this.update(url, match);
    }

    public UpdateMatchWithMatchEvents(url: string, match: any): Observable<any> {
        return this.update(url, match);
    }
}