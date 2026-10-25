import { Component, inject, input, signal } from '@angular/core';
import { OptionsMenuComponent, OptionsMenuContentDirective, OptionsMenuHeaderDirective } from '../../../../../shared-lib/src/lib/pages/options/options-menu/options-menu.component';
import { ITicket } from '../../core/tickets/models/ticket.interface';
import { TicketsService } from '../../core/tickets/services/tickets.service';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { ClassIndPipe } from '../../core/movies/pipes/class-ind-pipe';

@Component({
  imports: [
    ClassIndPipe,
    DatePipe,
    OptionsMenuComponent,
    OptionsMenuHeaderDirective,
    OptionsMenuContentDirective
  ],
  selector: 'cln-ticket',
  styleUrl: './ticket.component.scss',
  templateUrl: './ticket.component.html',
})
export class TicketComponent {
  private readonly _ticketService = inject(TicketsService);

  private readonly _router = inject(Router);

  ticketId = input.required<string>();

  ticket = signal<ITicket | null>(null);

  ngOnInit() {
    this._ticketService.getTicketById(this.ticketId()).subscribe({
      next: (ticket) => {
        this.ticket.set(ticket);
      },
      error: (_) => {
        this._router.navigate(['/']);
      }
    });
  }
}
