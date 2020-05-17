import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PlayerDto } from '../Models/PlayerDto';

@Injectable({
    providedIn: 'root'
})

export class PlayerService extends BaseService {

    constructor(http: HttpClient) {
        super(http);
    }

    public getPlayers(url: string): Observable<PlayerDto[]>{
        return this.get<any>(url);
    }
    public getPlayer(url: string): Observable<PlayerDto>{
        return this.get<any>(url);
    }
    public postPlayer(url: string, player : any): Observable<any>{
        return this.post<any>(url, player);
    }
    public deletePlayer(url: string): Observable<any> {
        return this.delete<any>(url);
    }
    public UpdatePlayer(url: string, player: any): Observable<any> {
        return this.update(url, player);
    }
}