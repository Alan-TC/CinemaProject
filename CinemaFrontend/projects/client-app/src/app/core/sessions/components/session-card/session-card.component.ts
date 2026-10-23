import { Component, input, output } from '@angular/core';
import { ISessionSummary } from '../../models/session-summary.interface';
import { ClassIndPipe } from '../../../movies/pipes/class-ind-pipe';
import { DatePipe } from '@angular/common';

@Component({
  imports: [
    ClassIndPipe,
    DatePipe],
  selector: 'cln-session-card',
  styleUrl: './session-card.component.scss',
  templateUrl: './session-card.component.html',
})
export class SessionCardComponent {
  session = input.required<ISessionSummary>();
  sessionClicked = output<string>();
}
