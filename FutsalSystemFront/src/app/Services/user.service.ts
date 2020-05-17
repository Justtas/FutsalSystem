import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserDto } from '../Models/UserDto';

@Injectable({
  providedIn: 'root'
})
export class UserService extends BaseService {

  constructor(http: HttpClient) {
    super(http);
  }

  public getUsers(url: string): Observable<UserDto[]> {
    return this.get<any>(url);
  }

  public getUser(url: string): Observable<UserDto> {
    return this.get<any>(url);
  }

  public postUser(url: string, user: any): Observable<any> {
    return this.post<any>(url, user);
  }

  public deleteUser(url: string): Observable<any> {
    return this.delete<any>(url);
  }

  public UpdateUser(url: string, user: any): Observable<any> {
    return this.update(url, user);
  }
}
