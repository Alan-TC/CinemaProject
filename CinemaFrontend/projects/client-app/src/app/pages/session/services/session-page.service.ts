import { computed, inject, Service, signal } from '@angular/core';
import { ISessionSeat } from '../../../core/sessions/models/session-seat.interface';
import { SessionsService } from '../../../core/sessions/services/sessions.service';
import { ISession } from '../../../core/sessions/models/session.interface';
import { Router } from '@angular/router';
import { take } from 'rxjs';

@Service()
export class SessionPageService {
private readonly MAX_SELECTED_SEATS = 6;

  private readonly _sessionService = inject(SessionsService);
  private readonly _router = inject(Router);

  private readonly _rawSession = signal<ISession | null>(null);
  private readonly _selectedSeatIds = signal<string[]>([]);
  private readonly _totalHalfPrice = signal<number>(0);
  readonly totalHalfPrice = this._totalHalfPrice.asReadonly();

  readonly totalPrice = computed<number>(() => {
    const rawSession = this._rawSession();
    const totalSeats = this._selectedSeatIds().length;

    if (!rawSession || totalSeats === 0) return 0;

    const validHalfTickets = Math.min(Math.max(0, this._totalHalfPrice()), totalSeats);
    const fullTickets = totalSeats - validHalfTickets;

    const unitPrice = rawSession.pricing;
    return (fullTickets * unitPrice) + (validHalfTickets * (unitPrice / 2));
  });

  readonly session = computed<ISession | null>(() => {
    const raw = this._rawSession();
    if (!raw) return null;

    const selectedIds = new Set(this._selectedSeatIds());

    return {
      ...raw,
      sessionRoom: {
        ...raw.sessionRoom,
        seats: raw.sessionRoom.seats.map(seat => ({
          ...seat,
          Status: seat.Status === 'Unavailable'
            ? 'Unavailable'
            : selectedIds.has(seat.id) ? 'Selected' : 'Available'
        }))
      }
    };
  });

  readonly selectedSeats = computed<ISessionSeat[]>(() => {
    const currentSession = this.session();
    if (!currentSession) return [];

    return currentSession.sessionRoom.seats.filter(seat => seat.Status === 'Selected');
  });

  loadSession(sessionId: string): void {
    this._sessionService.getSessionById(sessionId)
      .pipe(take(1))
      .subscribe({
        next: (incomingSession) => {
          const isSameSession = this._rawSession()?.id === incomingSession.id;
          this._rawSession.set(incomingSession);

          if (!isSameSession) {
            this._selectedSeatIds.set([]);
            this._totalHalfPrice.set(0);
          } else {
            const unavailableIds = new Set(
              incomingSession.sessionRoom.seats
                .filter(s => s.Status === 'Unavailable')
                .map(s => s.id)
            );

            this._selectedSeatIds.update(ids => ids.filter(id => !unavailableIds.has(id)));

            if (this._totalHalfPrice() > this._selectedSeatIds().length) {
              this._totalHalfPrice.set(this._selectedSeatIds().length);
            }
          }
        },
        error: () => {
          this._router.navigate(['/']);
        }
      });
  }

  setHalfPriceCount(count: number): void {
    const maxSeats = this._selectedSeatIds().length;
    const clampedCount = Math.min(Math.max(0, count), maxSeats);
    this._totalHalfPrice.set(clampedCount);
  }

  addSelectedSeat(seatId: string): string {
    const currentSession = this.session();
    if (!currentSession) return 'Sessão não carregada.';

    if (this._selectedSeatIds().length >= this.MAX_SELECTED_SEATS) {
      return `É permitido selecionar no máximo ${this.MAX_SELECTED_SEATS} assentos por pedido.`;
    }

    if (this._selectedSeatIds().includes(seatId)) {
      return 'Este assento já está selecionado.';
    }

    const seat = currentSession.sessionRoom.seats.find(s => s.id === seatId);
    if (!seat || seat.Status !== 'Available') {
      return 'Este assento não está disponível.';
    }

    this._selectedSeatIds.update(ids => [...ids, seatId]);
    return '';
  }

  removeSelectedSeat(seatId: string): void {
    this._selectedSeatIds.update(ids => ids.filter(id => id !== seatId));

    const remainingSeats = this._selectedSeatIds().length;
    if (this._totalHalfPrice() > remainingSeats) {
      this._totalHalfPrice.set(remainingSeats);
    }
  }
}
