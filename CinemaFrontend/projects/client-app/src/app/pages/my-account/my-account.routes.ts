import { Routes } from "@angular/router";

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import("./my-account.component").then((m) => m.MyAccountComponent),
    children: [
      {
        path: '',
        redirectTo: 'profile',
        pathMatch: 'full'
      },
      {
        path: 'profile',
        loadComponent: () => import("../profile/profile.component").then((m) => m.ProfileComponent)
      }
    ]
  }
];
