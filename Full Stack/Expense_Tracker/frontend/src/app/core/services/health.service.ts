import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { catchError, finalize, of } from 'rxjs';
import { environment } from '../../../environments/environment';

export type HealthState = 'loading' | 'connected' | 'failed';

@Injectable({ providedIn: 'root' })
export class HealthService {
  private readonly http = inject(HttpClient);
  readonly status = signal<HealthState>('loading');

  check(): void {
    this.status.set('loading');
    this.http.get(`${environment.apiBaseUrl}/api/health`)
      .pipe(
        catchError(() => {
          this.status.set('failed');
          return of(null);
        }),
        finalize(() => {
          if (this.status() === 'loading') {
            this.status.set('connected');
          }
        })
      )
      .subscribe();
  }
}
