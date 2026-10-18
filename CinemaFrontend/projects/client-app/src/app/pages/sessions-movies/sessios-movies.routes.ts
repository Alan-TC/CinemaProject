import { Routes } from "@angular/router";

export const routes: Routes = [
  {
    path: "",
    redirectTo: "sessions",
    pathMatch: "full"
  },
  {
    path: "sessions",
    loadComponent: () => import("./sessions-movies.component").then((m) => m.SessionsMoviesComponent)
  }
];
