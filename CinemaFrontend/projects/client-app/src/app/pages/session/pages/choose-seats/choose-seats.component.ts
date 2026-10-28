import { Component, inject, input, signal } from '@angular/core';
import { SessionPageService } from '../../services/session-page.service';
import { BuyProcessCardComponent } from '../../components/buy-process-card/buy-process-card.component';
import { Router } from '@angular/router';

@Component({
  imports: [
    BuyProcessCardComponent
  ],
  selector: 'cln-choose-seats',
  styleUrl: './choose-seats.component.scss',
  templateUrl: './choose-seats.component.html',
})
export class ChooseSeatsComponent {
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
      this._router.navigate(['/']);
    } else if (button == 'right') {
      this._router.navigate(['sessions', this.sessionId(), 'choose-tickets']);
    }
  }

  progressSeatClicked(seatId: string) {
    this._sessionPageService.removeSelectedSeat(seatId);
  }
}
