import { HttpHandlerFn, HttpRequest } from "@angular/common/http";
import { AuthService } from "../services/auth.service";
import { Router } from "@angular/router";
import { catchError, switchMap, throwError } from "rxjs";
import { addTokenHeader } from "./request-utils";

export function handle401Error(
  request: HttpRequest<unknown>,
  next: HttpHandlerFn,
  authService: AuthService,
  router: Router
) {
    return authService.refreshToken().pipe(
      switchMap((token) => {
        if (token === 'error') {
          return throwError(() => new Error('Sessão expirada'));
        }
        return next(addTokenHeader(request, token!.accessToken))
      }),
      catchError((error) => {
        router.navigate(['/login'], {
          queryParams: { returnUrl: router.url }
        });
        return throwError(() => error);
      })
    )
}
