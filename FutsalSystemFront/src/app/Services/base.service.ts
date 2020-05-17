import { Injectable } from "@angular/core";
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {catchError} from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class BaseService {
    constructor(private http: HttpClient) { }

    protected handleError<T>(operation = 'operation', result? : T){
        return (error:any): Observable<T> => {
            //console.error(error);
            return of(result as T);
        };
    }

    get<T>(url: string):Observable<any>{
        return this.http.get<any>(url, this.httpOptions()).pipe(catchError(this.handleError('url',[])));
    }
    post<T>(url: string,body: object): Observable<any>{
        return this.http.post<any>(url,body,this.httpOptions()).pipe(catchError(this.handleError('url',[])));
    }
    delete<T>(url: string): Observable<any> {
        return this.http.delete<any>(url, this.httpOptions()).pipe(catchError(this.handleError('url', [])));
    }
    update<T>(url: string, body: object): Observable<any> {
        return this.http.put<any>(url, body, this.httpOptions()).pipe(catchError(this.handleError('url', [])));
    }

    httpOptions(){
        const httpOptions = {
          async: true,
          crossDomain: true,
          headers: new HttpHeaders({
            'Content-Type': 'application/json'
          })
        };
        return httpOptions;
      }
}