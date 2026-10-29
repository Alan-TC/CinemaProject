import { ISeatType } from './../components/theater-layout-editor/models/seat-type.interface';
import { computed, inject, Service, signal } from '@angular/core';
import { ISessionSeat } from '../../../core/sessions/models/session-seat.interface';
import { SessionsService } from '../../../core/sessions/services/sessions.service';
import { ISession } from '../../../core/sessions/models/session.interface';
import { Router } from '@angular/router';
import { take } from 'rxjs';
import { ITheaterLayout } from '../components/theater-layout-editor/models/theater-layout.interface';

@Service()
export class SessionPageService {
private readonly MAX_SELECTED_SEATS = 5;

  private readonly _sessionService = inject(SessionsService);
  private readonly _router = inject(Router);

  private readonly _errorMessage = signal<string>('');
  private readonly _rawSession = signal<ISession | null>(null);
  private readonly _selectedSeats = signal<ISessionSeat[]>([]);
  private readonly _totalHalfPrice = signal<number>(0);

  readonly errorMessage = this._errorMessage.asReadonly();
  readonly session = this._rawSession.asReadonly();
  readonly totalHalfPrice = this._totalHalfPrice.asReadonly();
  readonly selectedSeats = this._selectedSeats.asReadonly();

  readonly theaterLayout = computed<ITheaterLayout>(() => {
    const rawSession = this._rawSession();

    if (!rawSession) {
      return { seats: [] };
    }

    const selectedSeats = this._selectedSeats();

    const seats: ISeatType[][] = Array.from({ length: rawSession.sessionRoom.height }, () =>
      Array.from({ length: rawSession.sessionRoom.width }, () => ({ id: '', type: 'Empty' } as ISeatType))
    );

    rawSession.sessionRoom.seats.forEach((seat) => {
      seats[rawSession.sessionRoom.height - 1 - seat.posY][seat.posX] = {
        id: seat.id,
        code: seat.code,
        type: seat.status
      };
    });

    selectedSeats.forEach((seat) => {
      seats[rawSession.sessionRoom.height - 1 - seat.posY][seat.posX] = {
        id: seat.id,
        code: seat.code,
        type: 'Selected'
      };
    });

    return { seats: seats };
  });

  readonly totalPrice = computed<number>(() => {
    const rawSession = this._rawSession();
    const totalSeats = this._selectedSeats().length;

    if (!rawSession || totalSeats === 0) return 0;

    const validHalfTickets = Math.min(Math.max(0, this._totalHalfPrice()), totalSeats);
    const fullTickets = totalSeats - validHalfTickets;

    const unitPrice = rawSession.pricing;
    return (fullTickets * unitPrice) + (validHalfTickets * (unitPrice / 2));
  });

  loadSession(sessionId: string): void {
    this._errorMessage.set('');
    this._sessionService.getSessionById(sessionId)
      .pipe(take(1))
      .subscribe({
        next: (incomingSession) => {
          const isSameSession = this._rawSession()?.id === incomingSession.id;
          this._rawSession.set(incomingSession);

          if (!isSameSession) {
            this._selectedSeats.set([]);
            this._totalHalfPrice.set(0);
          } else {
            const unavailableIds = new Set(
              incomingSession.sessionRoom.seats
                .filter(s => s.status === 'Unavailable')
                .map(s => s.id)
            );

            this._selectedSeats.update(seats => seats.filter(seat => !unavailableIds.has(seat.id)));

            if (this._totalHalfPrice() > this._selectedSeats().length) {
              this._totalHalfPrice.set(this._selectedSeats().length);
            }
          }
        },
        error: () => {
          this._router.navigate(['/']);
        }
      });
  }

  setHalfPriceCount(count: number): void {
    const maxSeats = this._selectedSeats().length;
    const clampedCount = Math.min(Math.max(0, count), maxSeats);
    this._totalHalfPrice.set(clampedCount);
  }

  updateSelectedSeat(seatId: string){
    const currentSession = this.session();
    if (!currentSession) {
     this._errorMessage.set('Sessão não carregada.');
     return;
    }

    const existingSeat = this._selectedSeats().find(seat => seat.id === seatId);
    if (!existingSeat) {
      this._addSelectedSeat(seatId, currentSession);
      return;
    }

    this._removeSelectedSeat(seatId);
  }

  private _addSelectedSeat(seatId: string, currentSession: ISession) {
    if (this._selectedSeats().length >= this.MAX_SELECTED_SEATS) {
      this._errorMessage.set(`É permitido selecionar no máximo ${this.MAX_SELECTED_SEATS} assentos por pedido.`);
      return;
    }

    if (this._selectedSeats().some(seat => seat.id === seatId)) {
      this._errorMessage.set('Este assento já está selecionado.');
      return;
    }

    const seat = currentSession.sessionRoom.seats.find(s => s.id === seatId);
    if (!seat || seat.status !== 'Available') {
      this._errorMessage.set('Este assento não está disponível para seleção.');
      return;
    }

    this._selectedSeats.update(seats => [...seats, seat]);
    this._errorMessage.set('');
  }

  private _removeSelectedSeat(seatId: string): void {
    this._selectedSeats.update(seats => seats.filter(seat => seat.id !== seatId));
    this._errorMessage.set('');

    const remainingSeats = this._selectedSeats().length;
    if (this._totalHalfPrice() > remainingSeats) {
      this._totalHalfPrice.set(remainingSeats);
    }
  }
}
