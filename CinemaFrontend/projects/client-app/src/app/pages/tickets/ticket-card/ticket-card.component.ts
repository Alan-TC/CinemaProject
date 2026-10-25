import { Component, input, output } from '@angular/core';
import { ITicketSummary } from '../../../core/tickets/models/tickets-summary.interface';
import { DatePipe } from '@angular/common';

@Component({
  imports: [
    DatePipe
  ],
  selector: 'cln-ticket-card',
  styleUrl: './ticket-card.component.scss',
  templateUrl: './ticket-card.component.html',
})
export class TicketCardComponent {
  ticket = input.required<ITicketSummary>();
  ticketClicked = output<string>();
}
