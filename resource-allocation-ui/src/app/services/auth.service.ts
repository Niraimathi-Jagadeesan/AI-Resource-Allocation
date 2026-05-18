import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { Observable, tap } from 'rxjs';
import { ApiService } from './api.service';

interface LoginRequest {
  username: string;
  password: string;
}

interface LoginResponse {
  token: string;
  username: string;
  role: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    private api: ApiService,
    private router: Router
  ) {}

  login(
    username: string,
    password: string
  ): Observable<LoginResponse> {
    const request: LoginRequest = {
      username,
      password
    };

    return this.api
      .post<LoginResponse>('Auth/login', request)
      .pipe(
        tap(response => {
          localStorage.setItem(
            'token',
            response.token
          );
          localStorage.setItem(
            'username',
            response.username
          );
          localStorage.setItem(
            'role',
            response.role
          );
        })
      );
  }

  logout(): void {
    localStorage.clear();
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  getToken(): string {
    return localStorage.getItem('token') || '';
  }

  getUsername(): string {
    return localStorage.getItem('username') || '';
  }

  getRole(): string {
    return localStorage.getItem('role') || '';
  }

  hasRole(role: string): boolean {
    return this.getRole() === role;
  }

  getDecodedToken(): any {
    const token = this.getToken();

    if (!token) {
      return null;
    }

    return jwtDecode(token);
  }
}