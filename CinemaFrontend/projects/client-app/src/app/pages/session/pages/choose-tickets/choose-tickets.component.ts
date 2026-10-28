import { Component, inject, input } from '@angular/core';
import { Router } from '@angular/router';
import { SessionPageService } from '../../services/session-page.service';
import { BuyProcessCardComponent } from '../../components/buy-process-card/buy-process-card.component';

@Component({
  imports: [
    BuyProcessCardComponent
  ],
  selector: 'cln-choose-tickets',
  styleUrl: './choose-tickets.component.scss',
  templateUrl: './choose-tickets.component.html',
})
export class ChooseTicketsComponent {
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
      this._router.navigate(['sessions', this.sessionId(), 'choose-seats']);
    } else if (button == 'right') {
      this._router.navigate(['sessions', this.sessionId(), 'payment-method']);
    }
  }

  progressSeatClicked(seatId: string) {
    this._sessionPageService.removeSelectedSeat(seatId);
  }
}
