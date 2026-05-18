import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');

    let headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    if (token) {
      headers = headers.set(
        'Authorization',
        `Bearer ${token}`
      );
    }

    return headers;
  }

  get<T>(url: string): Observable<T> {
    return this.http.get<T>(
      `${this.baseUrl}/${url}`,
      { headers: this.getHeaders() }
    );
  }

  post<T>(url: string, body: any): Observable<T> {
    return this.http.post<T>(
      `${this.baseUrl}/${url}`,
      body,
      { headers: this.getHeaders() }
    );
  }

  put<T>(url: string, body: any): Observable<T> {
    return this.http.put<T>(
      `${this.baseUrl}/${url}`,
      body,
      { headers: this.getHeaders() }
    );
  }

  delete<T>(url: string): Observable<T> {
    return this.http.delete<T>(
      `${this.baseUrl}/${url}`,
      { headers: this.getHeaders() }
    );
  }
}