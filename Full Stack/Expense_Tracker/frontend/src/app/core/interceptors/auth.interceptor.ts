import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const router = inject(Router);
  const token = localStorage.getItem('expense_tracker_token');
  const expiresAt = localStorage.getItem('expense_tracker_expires_at');
  const isExpired = !expiresAt || new Date(expiresAt).getTime() <= Date.now();

  const authenticatedRequest = token && !isExpired
    ? request.clone({
        setHeaders: { Authorization: `Bearer ${token}` }
      })
    : request;

  return next(authenticatedRequest).pipe(
    catchError(error => {
      if (error.status === 401) {
        localStorage.removeItem('expense_tracker_token');
        localStorage.removeItem('expense_tracker_user');
        localStorage.removeItem('expense_tracker_expires_at');
        window.dispatchEvent(new CustomEvent('auth:session-cleared'));
        void router.navigate(['/login']);
      }

      return throwError(() => error);
    })
  );
};
