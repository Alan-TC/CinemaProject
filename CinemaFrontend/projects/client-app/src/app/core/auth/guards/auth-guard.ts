import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, GuardResult, MaybeAsync, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { map } from 'rxjs';

export const authGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot):
  MaybeAsync<GuardResult> =>
{
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.validateTokens().pipe(
    map((isValid) =>
      isValid
        ? true
        : router.createUrlTree(['/login'],
          { queryParams: { returnUrl: state.url } })
    )
  );
};
