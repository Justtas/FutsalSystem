import { Observable } from 'rxjs';
import { BaseService } from './base.service';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AnnouncementDto } from '../Models/AnnouncementDto';

@Injectable({
    providedIn: 'root'
})

export class AnnouncementService extends BaseService {

    constructor(http: HttpClient) {
        super(http);
    }

    public getAnnouncements(url: string): Observable<AnnouncementDto[]>{
        return this.get<any>(url);
    }

    public postAnnouncement(url:string, announcement : any): Observable<any>{
        return this.post<any>(url, announcement);
    }

    public deleteAnnouncement(url: string): Observable<any> {
        return this.delete<any>(url);
    }
    
    public UpdateAnnouncement(url: string, announcement: any): Observable<any> {
        return this.update(url, announcement);
    }
}