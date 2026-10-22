import { Component, input, output } from '@angular/core';
import { ISessionSummary } from '../../../core/sessions/models/session-summary.interface';
import { ClassIndPipe } from '../../../core/movies/pipes/class-ind-pipe';
import { DatePipe } from '@angular/common';

@Component({
  imports: [
    ClassIndPipe,
    DatePipe],
  selector: 'cln-session-summary',
  styleUrl: './session-summary.component.scss',
  templateUrl: './session-summary.component.html',
})
export class SessionSummaryComponent {
  session = input.required<ISessionSummary>();
  sessionClicked = output<string>();
}
