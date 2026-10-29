import { Routes } from "@angular/router";
import { authGuard } from "../../core/auth/guards/auth-guard";

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'choose-seats',
    pathMatch: 'full'
  },
  {
    path: 'choose-seats',
    loadComponent: () => import("./pages/choose-seats/choose-seats.component").then((m) => m.ChooseSeatsComponent)
  },
  {
    path: 'choose-tickets',
    loadComponent: () => import("./pages/choose-tickets/choose-tickets.component").then((m) => m.ChooseTicketsComponent)
  },
  {
    path: 'payment-method',
    canActivate: [authGuard],
    loadComponent: () => import("./pages/payment-method/payment-method.component").then((m) => m.PaymentMethodComponent)
  }
];
