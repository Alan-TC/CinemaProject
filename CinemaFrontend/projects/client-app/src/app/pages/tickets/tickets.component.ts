import { Component, inject, signal } from '@angular/core';
import { TicketCardComponent } from './ticket-card/ticket-card.component';
import { OptionsMenuComponent, OptionsMenuContentDirective, OptionsMenuHeaderDirective } from '../../../../../shared-lib/src/lib/pages/options/options-menu/options-menu.component';
import { ITicketSummary } from '../../core/tickets/models/tickets-summary.interface';
import { TicketsService } from '../../core/tickets/services/tickets.service';
import { Router } from '@angular/router';

@Component({
  imports: [
    TicketCardComponent,
    OptionsMenuComponent,
    OptionsMenuHeaderDirective,
    OptionsMenuContentDirective
  ],
  selector: 'cln-tickets',
  styleUrl: './tickets.component.scss',
  templateUrl: './tickets.component.html',
})
export class TicketsComponent {
  private readonly _ticketsService = inject(TicketsService);

  private readonly _router = inject(Router);

  tickets = signal<ITicketSummary[]>([]);

  currPage = 1;
  pageSize = 10;
  hasNext = true;
  isLoading = false;

  ngOnInit(): void {
    this.loadMoreTickets();
  }

  onTicketClicked(ticketId: string): void {
    this._router.navigate(['/tickets', ticketId]);
  }

  onScroll(event: Event): void {
    const element = event.target as HTMLElement;

    const chegouNoFinal =
      element.scrollTop + element.clientHeight >=
      element.scrollHeight - 240;

    if (chegouNoFinal) {
      this.loadMoreTickets();
    }
  }

  private loadMoreTickets(): void {
        if (this.isLoading || !this.hasNext) {
      return;
    }
    this.isLoading = true;

    this._ticketsService.getTickets(this.currPage, this.pageSize)
      .subscribe({
        next: (response) => {
          this.currPage++;

          this.hasNext = response.hasNext;
          this.tickets.update((curr) => curr.concat(response.items));
          this.isLoading = false;
        },
        error: (_) => {
          this.isLoading = false;
        }
      });
  }
}
