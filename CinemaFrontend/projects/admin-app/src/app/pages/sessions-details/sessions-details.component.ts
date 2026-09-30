import { Component, inject, input } from '@angular/core';
import { SessionsService } from '../../core/sessions/services/sessions.service';
import { OptionsMenuComponent, OptionsMenuContentDirective, OptionsMenuHeaderDirective } from '../../../../../shared-lib/src/lib/pages/options/options-menu/options-menu.component';
import { ISessionResponse } from '../../core/sessions/models/session-response.interface';
import { Observable, switchMap } from 'rxjs';
import { toObservable } from '@angular/core/rxjs-interop';
import { AsyncPipe, DatePipe } from '@angular/common';

@Component({
  imports: [
    AsyncPipe,
    DatePipe,
    OptionsMenuComponent,
    OptionsMenuHeaderDirective,
    OptionsMenuContentDirective,
  ],
  selector: 'app-sessions-details',
  styleUrl: './sessions-details.component.scss',
  templateUrl: './sessions-details.component.html',
})
export class SessionsDetailsComponent {
  private readonly _sessionsService = inject(SessionsService);

  id = input.required<string>();

  session$: Observable<ISessionResponse> = toObservable(this.id)
    .pipe(
      switchMap(id => this._sessionsService.getSessionById(id))
    );
}
