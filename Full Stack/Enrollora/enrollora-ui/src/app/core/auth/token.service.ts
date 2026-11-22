
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  private readonly TOKEN_KEY = 'enrollora_token';
  private readonly ROLE_KEY = 'enrollora_role';
  private readonly NAME_KEY = 'enrollora_name';

  private readonly roleSubject = new BehaviorSubject<string | null>(this.getRole());
  readonly role$ = this.roleSubject.asObservable();

  private readonly nameSubject = new BehaviorSubject<string | null>(this.getName());
  readonly name$ = this.nameSubject.asObservable();

  setToken(token: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  setRole(role: string): void {
    localStorage.setItem(this.ROLE_KEY, role);
    this.roleSubject.next(role);
  }

  getRole(): string | null {
    return localStorage.getItem(this.ROLE_KEY);
  }

  setName(name: string): void {
    localStorage.setItem(this.NAME_KEY, name);
    this.nameSubject.next(name);
  }

  getName(): string | null {
    return localStorage.getItem(this.NAME_KEY);
  }

  removeToken(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.ROLE_KEY);
    localStorage.removeItem(this.NAME_KEY);
    this.roleSubject.next(null);
    this.nameSubject.next(null);
  }

  hasToken(): boolean {
    return this.getToken() !== null;
  }
}
