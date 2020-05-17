import { Observable } from 'rxjs';
import { BaseService } from './base.service';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TeamDto } from '../Models/TeamDto';

@Injectable({
    providedIn: 'root'
})

export class TeamService extends BaseService {

    constructor(http: HttpClient) {
        super(http);
    }

    public getTeams(url: string): Observable<any[]>{
        return this.get<any>(url);
    }

    public getTeam(url: string): Observable<TeamDto>{
        return this.get<any>(url);
    }

    public postTeam(url:string, team : any): Observable<any>{
        return this.post<any>(url, team);
    }

    public deleteTeam(url: string): Observable<any> {
        return this.delete<any>(url);
    }
    
    public UpdateTeam(url: string, team: any): Observable<any> {
        return this.update(url, team);
    }
}