import { HttpErrorResponse, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { handle401Error } from './error401-handler';
import { addTokenHeader } from './request-utils';

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const token = authService.getAccessToken();
  let newRequest = request;
  if (token) {
    newRequest = addTokenHeader(request, token);
  }

  return next(newRequest).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && !request.url.includes(authService.getRefreshUrlRelativePath())) {
        return handle401Error(newRequest, next, authService, router);
      }
      return throwError(() => error);
    })
  );
};
