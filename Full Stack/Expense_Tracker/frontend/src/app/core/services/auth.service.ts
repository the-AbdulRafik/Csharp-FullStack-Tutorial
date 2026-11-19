import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  AuthResponse,
  LoginRequest,
  RegisterRequest,
  RegisterResponse
} from '../models/auth.models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);

  readonly isAuthenticated = signal(this.hasValidStoredSession());

  constructor() {
    window.addEventListener('auth:session-cleared', () => {
      this.isAuthenticated.set(false);
    });
  }

  register(request: RegisterRequest): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(
      `${environment.apiBaseUrl}/api/auth/register`,
      request
    );
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      `${environment.apiBaseUrl}/api/auth/login`,
      request
    ).pipe(
      tap(response => {
        localStorage.setItem('expense_tracker_token', response.token);
        localStorage.setItem('expense_tracker_user', JSON.stringify(response.user));
        localStorage.setItem('expense_tracker_expires_at', response.expiresAt);
        this.isAuthenticated.set(true);
      })
    );
  }

  logout(): void {
    localStorage.removeItem('expense_tracker_token');
    localStorage.removeItem('expense_tracker_user');
    localStorage.removeItem('expense_tracker_expires_at');
    this.isAuthenticated.set(false);
    void this.router.navigate(['/login']);
  }

  getToken(): string | null {
    if (!this.hasValidStoredSession()) {
      this.logout();
      return null;
    }

    return localStorage.getItem('expense_tracker_token');
  }

  private hasValidStoredSession(): boolean {
    const token = localStorage.getItem('expense_tracker_token');
    const expiresAt = localStorage.getItem('expense_tracker_expires_at');
    return Boolean(token && expiresAt && new Date(expiresAt).getTime() > Date.now());
  }
}
