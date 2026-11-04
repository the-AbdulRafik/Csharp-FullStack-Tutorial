import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { Observable, catchError, throwError, tap } from 'rxjs';
import { ApiError, AuthResponse, LoginRequest, RegisterRequest } from './auth.models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5169/api/auth';
  private readonly tokenKey = 'movie-ticket-auth-token';
  readonly authenticated = signal(Boolean(localStorage.getItem(this.tokenKey)));

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, request).pipe(
      catchError((error: HttpErrorResponse) => this.handleError(error)),
    );
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, request).pipe(
      tap((response) => {
        if (response.token) {
          localStorage.setItem(this.tokenKey, response.token);
          this.authenticated.set(true);
        }
      }),
      catchError((error: HttpErrorResponse) => this.handleError(error)),
    );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.authenticated.set(false);
  }

  isLoggedIn(): boolean {
    return this.authenticated();
  }

  private handleError(error: HttpErrorResponse) {
    const body = error.error as ApiError | null;
    const message = body?.message ?? body?.title ?? 'The request could not be completed.';
    return throwError(() => new Error(message));
  }
}
