import { Component, inject, input, output } from '@angular/core';
import { ISession } from '../../../../core/sessions/models/session.interface';
import { DatePipe } from '@angular/common';
import { ClassIndPipe } from '../../../../core/movies/pipes/class-ind-pipe';
import { ButtonComponent } from '../../../../../../../shared-lib/src/lib/io/button/button.component';
import { Router } from '@angular/router';
import { ISessionSeat } from '../../../../core/sessions/models/session-seat.interface';

@Component({
  imports: [
    DatePipe,
    ClassIndPipe,
    ButtonComponent
  ],
  selector: 'cln-buy-process-card',
  styleUrl: './buy-process-card.component.scss',
  templateUrl: './buy-process-card.component.html',
})
export class BuyProcessCardComponent {
  private readonly _router = inject(Router);

  progressType = input<'seats' | 'tickets' | 'payment'>('seats');
  session = input<ISession | null>(null);
  selectedSeats = input<ISessionSeat[]>([]);

  buttonClicked = output<'left' | 'right'>();
  seatClicked = output<string>();

  canClickSeat = false;

  ngOnInit() {
    this.canClickSeat = this.progressType() == 'seats' || this.progressType() == 'tickets';
  }

  onClick(button: 'left' | 'right') {
    this.buttonClicked.emit(button);
  }

  onSeatClicked(seatId: string) {
    if (!this.canClickSeat)
      return;
    this.seatClicked.emit(seatId);
  }
}
