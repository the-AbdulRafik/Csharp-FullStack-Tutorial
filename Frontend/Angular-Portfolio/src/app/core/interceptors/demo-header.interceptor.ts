import { HttpInterceptorFn } from '@angular/common/http';

export const demoHeaderInterceptor: HttpInterceptorFn = (req, next) => {
  const clonedRequest = req.clone({
    setHeaders: {
      'X-Portfolio-Client': 'Angular-Portfolio-v1',
      'X-Demo-Timestamp': new Date().toISOString()
    }
  });

  return next(clonedRequest);
};
