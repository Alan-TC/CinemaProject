import { Routes } from '@angular/router';
import { authGuard } from './core/auth/guards/auth-guard';
import { guestGuard } from './core/auth/guards/guest-guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/main/main.component').then((m) => m.MainComponent),
    children: [
      {
        path: '',
        loadChildren: () => import('./pages/sessions-movies/sessios-movies.routes').then((m) => m.routes)
      },
      {
        path: 'my-account',
        canActivate: [authGuard],
        loadChildren: () => import("./pages/my-account/my-account.routes").then((m) => m.routes)
      },
      {
        path: 'movies/:movieId',
        loadComponent: () => import("./pages/movie/movie.component").then((m) => m.MovieComponent)
      },
      {
        path: 'tickets/:ticketId',
        loadComponent: () => import("./pages/ticket/ticket.component").then((m) => m.TicketComponent)
      },
      {
        path: 'sessions/:sessionId',
        loadChildren: () => import("./pages/session/session.routes").then((m) => m.routes)
      }
    ]
  },
  {
    path: 'login',
    canActivate: [guestGuard],
    loadComponent: () => import('./pages/login/login.component').then((m) => m.LoginComponent),
  },
  {
    path: 'register',
    canActivate: [guestGuard],
    loadComponent: () => import('./pages/register/register.component').then((m) => m.RegisterComponent),
  }
];
