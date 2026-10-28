import { Component, inject, input } from '@angular/core';
import { Router } from '@angular/router';
import { SessionPageService } from '../../services/session-page.service';
import { BuyProcessCardComponent } from '../../components/buy-process-card/buy-process-card.component';

@Component({
  imports: [
    BuyProcessCardComponent
  ],
  selector: 'cln-payment-method',
  styleUrl: './payment-method.component.scss',
  templateUrl: './payment-method.component.html',
})
export class PaymentMethodComponent {
  private readonly _sessionPageService = inject(SessionPageService);

  private readonly _router = inject(Router);

  protected readonly session = this._sessionPageService.session;
  protected readonly selectedSeats = this._sessionPageService.selectedSeats;

  readonly sessionId = input.required<string>();

  ngOnInit(): void {
    this._sessionPageService.loadSession(this.sessionId());
  }

  progressButtonClicked(button: 'left' | 'right') {
    if (button == 'left') {
      this._router.navigate(['sessions', this.sessionId(), 'choose-tickets']);
    } else if (button == 'right') {
      console.log('TODO: ...');
    }
  }

  progressSeatClicked(seatId: string) {
    this._sessionPageService.removeSelectedSeat(seatId);
  }
}
