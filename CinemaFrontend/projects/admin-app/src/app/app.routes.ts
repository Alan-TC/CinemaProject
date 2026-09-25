import { Routes } from '@angular/router';
import { authGuard } from './core/auth/guards/auth-guard';
import { guestGuard } from './core/auth/guards/guest-guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./pages/login/login.component').then(m => m.LoginComponent),
    canActivate: [guestGuard()],
  },
  { path: '',
    loadComponent: () => import('./pages/main/main.component').then(m => m.MainComponent),
    canActivate: [authGuard()],
    runGuardsAndResolvers: 'always'
  },
];
