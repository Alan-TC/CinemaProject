import { Component, DestroyRef, inject, signal } from '@angular/core';
import { SessionsMoviesService } from '../sessions-movies/services/sessions-movies.service';
import { SessionsService } from '../../core/sessions/services/sessions.service';
import { ISessionSummary } from '../../core/sessions/models/session-summary.interface';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { distinctUntilChanged, switchMap, tap } from 'rxjs';
import { SessionSummaryComponent } from './session-summary/session-summary.component';
import { Router } from '@angular/router';

@Component({
  imports: [
    SessionSummaryComponent
  ],
  selector: 'cln-sessions',
  styleUrl: './sessions.component.scss',
  templateUrl: './sessions.component.html',
})
export class SessionsComponent {
  private readonly _sessionsMoviesService = inject(SessionsMoviesService);
  private readonly _sessionsService = inject(SessionsService);
  private readonly _destroyRef = inject(DestroyRef);

  private readonly _router = inject(Router);

  sessions = signal<ISessionSummary[]>([]);

  currPage = 1;
  pageSize = 16;
  hasNext = true;
  isLoading = false;

  constructor() {
    toObservable(this._sessionsMoviesService.searchQuery)
      .pipe(
        distinctUntilChanged(),
        tap(() => {
          this.currPage = 1;
          this.hasNext = true;
          this.sessions.set([]);
          this.isLoading = true;
        }),
        switchMap(query =>
          this._sessionsService.getSessions(1, this.pageSize, query)
        ),
        takeUntilDestroyed()
      )
      .subscribe({
        next: response => {
          this.sessions.set(response.items);
          this.hasNext = response.hasNext;
          this.currPage = 2;
          this.isLoading = false;
        },
        error: () => {
          this.isLoading = false;
        }
      });

    this._sessionsMoviesService.onScroll$
    .pipe(takeUntilDestroyed())
    .subscribe(scrollInfo => {
      this.handleScroll(scrollInfo);
    });
  }

  sessionClicked(sessionId: string): void {
    this._router.navigate(['/sessions', sessionId]);
  }

  handleScroll(scrollInfo: {scrollTop: number, clientHeight: number, scrollHeight: number}): void {
    const chegouNoFinal =
      scrollInfo.scrollTop + scrollInfo.clientHeight >=
      scrollInfo.scrollHeight - 240;

    if (chegouNoFinal) {
      this.loadMoreSessions();
    }
  }

  loadMoreSessions(): void {
    if (this.isLoading || !this.hasNext) {
      return;
    }

    this.isLoading = true;

    this._sessionsService
      .getSessions(
        this.currPage,
        this.pageSize,
        this._sessionsMoviesService.searchQuery()
      )
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe({
        next: response => {
          this.sessions.update(current => [
            ...current,
            ...response.items
          ]);

          this.currPage++;
          this.hasNext = response.hasNext;
          this.isLoading = false;
        },
        error: () => {
          this.isLoading = false;
        }
      });
  }
}
