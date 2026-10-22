import { Routes } from "@angular/router";

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import("./sessions-movies.component").then((m) => m.SessionsMoviesComponent),
    children: [
      {
        path: '',
        redirectTo: 'sessions',
        pathMatch: 'full'
      },
      {
        path: 'sessions',
        loadComponent: () => import("../sessions/sessions.component").then((m) => m.SessionsComponent)
      },
      {
        path: 'movies',
        loadComponent: () => import("../movies/movies.component").then((m) => m.MoviesComponent)
      }
    ]
  }
];
