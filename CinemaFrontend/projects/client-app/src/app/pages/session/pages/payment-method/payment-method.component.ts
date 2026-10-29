import { Component, DestroyRef, inject, input, signal } from '@angular/core';
import { Router } from '@angular/router';
import { SessionPageService } from '../../services/session-page.service';
import { BuyProcessCardComponent } from '../../components/buy-process-card/buy-process-card.component';
import { CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IOrderRequest } from '../../../../core/orders/models/order-request.interface';
import { OrdersService } from '../../../../core/orders/services/orders.service';

@Component({
  imports: [
    FormsModule,
    BuyProcessCardComponent,
    CurrencyPipe
  ],
  selector: 'cln-payment-method',
  styleUrl: './payment-method.component.scss',
  templateUrl: './payment-method.component.html',
})
export class PaymentMethodComponent {
  private readonly _sessionPageService = inject(SessionPageService);
  private readonly _ordersService = inject(OrdersService);

  private readonly _destroyRef = inject(DestroyRef);
  private readonly _router = inject(Router);

  protected readonly session = this._sessionPageService.session;
  protected readonly selectedSeats = this._sessionPageService.selectedSeats;
  protected readonly totalPrice = this._sessionPageService.totalPrice;
  protected readonly totalHalfPrice = this._sessionPageService.totalHalfPrice;

  readonly sessionId = input.required<string>();

  isChecked = false;
  errorMessage = signal<string>('');
  private errorTimeoutId: number | null = null;

  constructor() {
    this._destroyRef.onDestroy(() => {
      if (this.errorTimeoutId) {
        clearTimeout(this.errorTimeoutId);
      }
    });
  }

  ngOnInit(): void {
    this._sessionPageService.loadSession(this.sessionId());
  }

  incrementHalfPrice(value: number) {
    const newCount = this.totalHalfPrice() + value;
    this._sessionPageService.setHalfPriceCount(newCount);
  }

  updateSeat(seatId: string) {
    this._sessionPageService.updateSelectedSeat(seatId);
  }

  progressButtonClicked(button: 'left' | 'right') {
    if (button == 'left') {
      this._router.navigate(['sessions', this.sessionId(), 'choose-tickets']);
    } else if (button == 'right') {
      if (!this.isChecked) {
        this.setErrorMessage('Selecione a forma de pagamento.')
        return;
      }
      if (this.selectedSeats().length <= 0) {
        this.setErrorMessage('Selecione pelo menos um assento.')
        return;
      }

      const selectedSeatsIds = this.selectedSeats().map(seat => seat.id);

      const orderRequest: IOrderRequest = {
        sessionId: this.sessionId(),
        fullPriceQuantity: selectedSeatsIds.length - this.totalHalfPrice(),
        halfPriceQuantity: this.totalHalfPrice(),
        seats: selectedSeatsIds
      };

      this._ordersService.makeOrder(orderRequest).subscribe({
        next: (order) => {
          this._router.navigate(['orders', order.id]);
        },
        error: (error) => {
          if (error.status === 400 || error.status === 409) {
            this.setErrorMessage(error.error.errorMessages.join(', '));
            return;
          }
          this.setErrorMessage('Erro desconhecido.')
        }
      });
    }
  }

  private setErrorMessage(message: string) {
    if (this.errorTimeoutId) {
      clearTimeout(this.errorTimeoutId);
    }

    this.errorMessage.set(message);

    this.errorTimeoutId = window.setTimeout(() => {
      this.errorMessage.set('');
      this.errorTimeoutId = null;
    }, 5000);
  }
}
