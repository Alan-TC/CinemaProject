import { Component, inject, input, signal } from '@angular/core';
import { OrdersService } from '../../core/orders/services/orders.service';
import { Router } from '@angular/router';

@Component({
  imports: [],
  selector: 'cln-order',
  styleUrl: './order.component.scss',
  templateUrl: './order.component.html',
})
export class OrderComponent {
  private readonly _ordersService = inject(OrdersService);

  private readonly _router = inject(Router);

  orderId = input.required<string>();
  state = signal<'loading' | 'processing' | 'success' | 'error'>('loading');

  ngOnInit(): void {
    setTimeout(() => {
      this._ordersService.getOrderById(this.orderId()).subscribe({
        next: (_) => {
          this.state.set('processing');
          setTimeout(() => {
            this._ordersService.payOrder(this.orderId()).subscribe({
              next: (_) => {
                this.state.set('success');
                setTimeout(() => {
                  this._router.navigate(['/my-account', 'tickets']);
                }, 3000);
              },
              error: () => {
                this.state.set('error');
                setTimeout(() => {
                  this._router.navigate(['/']);
                }, 3000);
              }
            });
          }, 3000);
        },
        error: () => {
          this.state.set('error');
          setTimeout(() => {
            this._router.navigate(['/']);
          }, 3000);
        },
      });
    }, 3000);
  }
}
